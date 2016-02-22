using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MEFDemoSimple.Code
{
    public static class Utils
    {
        public static int TryForEachExport<TExportType>(CompositionContainer pluginContainer, Action<TExportType> action, string contractName = "")
            where TExportType : IPlugin
        {
            int count = 0;
            try
            {
                int skipCount = 0;
                foreach (var item in pluginContainer.GetExports<TExportType>(contractName))
                {
                    string itemName = item.Value.ToString();
                    // check if the plugin is enabled
                    bool disabled = false;

                    if (disabled)
                    {
                        skipCount++;
                        Log(String.Format("   - skipping -> {0}", itemName));
                    }
                    else
                    {
                        Log(String.Format("   - action -> {0}", itemName));
                        action(item.Value);
                        count++;
                    }
                }
                Log(String.Format("{0} actions performed / {1} actions skipped", count, skipCount));
            }
            catch (Exception err)
            {
                throw LogPluginException(err);
            }
            return count;
        }

        public static Exception GetException(Exception err)
        {
            return err.InnerException ?? err;
        }
        public static void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
        public static string Log(Exception err)
        {
            string result = GetException(err).Message;
            Log(result);
            return result;
        }
        public static string Log(IEnumerable<Exception> errors)
        {
            string result = String.Join("\n", (from err in errors
                                               select GetException(err).Message).ToArray());
            Log(result);
            return result;
        }
        public static string Log(IEnumerable<CompositionError> errors)
        {
            string result = String.Join("\n", (from err in errors
                                               select GetException(err.Exception).Message).ToArray());
            Log(result);
            return result;
        }

        public static Exception LogPluginException(Exception error)
        {
            Exception result = error;
            if (error is FileNotFoundException)
            {
                Log(error);                
            }
            else if (error is CompositionException)
            {
                result = new Exception(Log((CompositionException)error));
            }
            else if (error is ReflectionTypeLoadException)
            {
                result = new Exception(Log(((ReflectionTypeLoadException)error).LoaderExceptions));
            }
            else
            {
                Log(error);
            }

            return result;
        }
    }
}
