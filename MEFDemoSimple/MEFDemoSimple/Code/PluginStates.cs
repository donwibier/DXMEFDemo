using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MEFDemoSimple.Code
{
    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true, TypeName = "PluginConfiguration")]
    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "Plugins")]
    public class PluginStates
    {
        private readonly static XmlSerializer configSerializer = new XmlSerializer(typeof(PluginStates));
        private List<PluginConfigItem> _Plugins;
        public static void FromFile(string file, IDictionary<string, bool> states)
        {            
            if (File.Exists(file))
            {                
                using (FileStream stream = File.OpenRead(file))
                {
                    PluginStates s = configSerializer.Deserialize(stream) as PluginStates;
                    foreach (PluginConfigItem item in s.Plugins)
                    {
                        states[item.PluginTypeString] = item.Disabled;
                    }
                }
            }            
        }

        public static void ToFile(IDictionary<string, bool> states, string file)
        {
            PluginStates s = new PluginStates();
            foreach(KeyValuePair<string, bool> kvp in states)
            {
                s.Plugins.Add(new PluginConfigItem() { PluginTypeString = kvp.Key, Disabled = kvp.Value });
            }
            s.Plugins.Sort();            
            using (FileStream stream = File.Create(file))
                configSerializer.Serialize(stream, s);
        }

        [XmlArrayItem("plugin", typeof(PluginConfigItem))]
        [XmlArray(ElementName = "plugins")]
        public List<PluginConfigItem> Plugins
        {
            get
            {
                if (_Plugins == null)
                    _Plugins = new List<PluginConfigItem>();
                return _Plugins;
            }
            set { _Plugins = value; }
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true, TypeName = "PluginConfigItem")]
    public class PluginConfigItem : IComparable
    {
        [XmlElement("typeName")]
        public string PluginTypeString { get; set; }
        [XmlElement("disabled")]
        public bool Disabled { get; set; }
        public int CompareTo(object obj)
        {            
            if (obj == null) return 1;
            if (obj is PluginConfigItem)
            {
                PluginConfigItem other = obj as PluginConfigItem;
                return PluginTypeString.CompareTo(other.PluginTypeString);
            }
            else
                throw new ArgumentException("Object is not a PluginConfigItem");
        }
    }
}