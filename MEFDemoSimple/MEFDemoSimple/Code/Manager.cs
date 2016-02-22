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
        }

        public static IContent ContentProvider { get { return _manager.ContentGenerator; } }
        public static IEnumerable<IOperation> Operations { get { return _manager.Operations; } }

        public static string Execute(string operationName, string text)
        {
            var op = (from n in Operations
                      where n.Name == operationName
                      select n).FirstOrDefault();
            if (op == null)
                throw new Exception(String.Format("operation '{0}' not found", operationName));

            return op.Execute(text);
        }
    }
}
