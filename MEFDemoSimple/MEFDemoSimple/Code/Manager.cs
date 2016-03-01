using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.IO;
using System.ComponentModel.Composition;
using System.Text;
using System.Web.Hosting;
using System.Collections.Concurrent;

namespace MEFDemoSimple.Code
{

    public class PluginsEngine
    {        
        [Import]
        public IContent ContentGenerator { get; set; }
        [ImportMany(typeof(IOperation))]
        public IEnumerable<IOperation> Operations { get; set; }
    }


    public class Manager
    {
        private static readonly PluginsEngine _manager;
       
        private static readonly AggregateCatalog _catalog;
        private static readonly CompositionContainer _container;


        static Manager()
        {
            //_manager = new PluginManager()
            //{
            //    ContentGenerator = new LorumIpsumContent(),
            //    Operations = new IOperation[]
            //    {
            //        new ReverseOperation()
            //    }
            //}

            string pluginsPath = HostingEnvironment.MapPath(String.Join("/", HostingEnvironment.ApplicationVirtualPath, "bin"));
            DirectoryCatalog directorywatcher = new DirectoryCatalog(pluginsPath, "*.dll");

            _catalog = new AggregateCatalog();
            _catalog.Catalogs.Add(directorywatcher);
            _container = new CompositionContainer(_catalog, true);
            _manager = new PluginsEngine();
            _container.SatisfyImportsOnce(_manager);

            LoadPluginStates();            
        }

        //public static IContent ContentProvider { get { return _manager.ContentGenerator; } }
        //public static IEnumerable<IOperation> Operations { get { return _manager.Operations; } }

        public static IContent ContentProvider
        {
            get
            {
                return PluginEnabled(_manager.ContentGenerator.ToString()) ?
                    _manager.ContentGenerator : null;
            }
        }
        public static IEnumerable<IOperation> Operations
        {
            get
            {
                return from n in _manager.Operations
                       where PluginEnabled(n.ToString())
                       select n;
            }
        }

        public static string Execute(string operationName, string text)
        {
            var op = (from n in Operations
                      where n.Name == operationName
                      select n).FirstOrDefault();
            if (op == null)
                throw new Exception(String.Format("operation '{0}' not found", operationName));

            return op.Execute(text);
        }

        private static readonly object _statesLock = new object();
        private static readonly string statesFile = HostingEnvironment.MapPath("~/App_Data/Plugins.config");
        private static readonly ConcurrentDictionary<string, bool> _states = new ConcurrentDictionary<string, bool>();

        public static void LoadPluginStates()
        {
            bool changed = false;
            lock (_statesLock)
            {
                _states.Clear();
                PluginStates.FromFile(statesFile, _states);
                foreach (var item in _catalog.Parts)
                {
                    string pluginName = item.ToString();
                    if (!_states.ContainsKey(pluginName))
                    {
                        Utils.Log(String.Format("  - Adding plugin [{0}]", pluginName));
                        changed = true;
                        _states[pluginName] = true; // Disabled when added
                    }
                }
            }
            if (changed)
                SavePluginStates();

        }
        public static void SavePluginStates()
        {
            PluginStates.ToFile(_states, statesFile);
        }

        public static bool PluginEnabled(string name)
        {
            bool disabled = _states.ContainsKey(name) ? _states[name] : true; //disabled by default;
            return !disabled;
        }
        public static int ForEachExport<TExportType>(Action<TExportType> action, string contractName = "")
            where TExportType : IPlugin
        {
            int count = 0;
            try
            {
                int skipCount = 0;
                foreach (var item in _container.GetExports<TExportType>(contractName))
                {
                    string itemName = item.Value.ToString();                                        

                    if (PluginEnabled(itemName))
                    {
                        Utils.Log(String.Format("   - action -> {0}", itemName));
                        action(item.Value);
                        count++;
                    }
                    else
                    {
                        skipCount++;
                        Utils.Log(String.Format("   - skipping -> {0}", itemName));
                    }
                }
                Utils.Log(String.Format("{0} actions performed / {1} actions skipped", count, skipCount));
            }
            catch (Exception err)
            {
                throw Utils.LogPluginException(err);
            }
            return count;
        }
    }
}
