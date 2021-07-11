using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PasteSimple.Helpers
{
    class ConfigHelper
    {
        string configFile;
        public Dictionary<string, object> dict = new Dictionary<string, object>()
        {
            {"last_uid",""},
            {"auto_start",true}
        };
        //public Dictionary<string, object> dict = new Dictionary<string, object>();
        public ConfigHelper(string path)
        {
            configFile = path;
        }

        public bool ConfigExist()
        {
            return File.Exists(configFile);
        }

        public string GetValue(string key)
        {
            if (!dict.Keys.Contains(key))
                return null;
            if (dict[key] == null)
                return "";
            return dict[key].ToString();
        }

        public void WriteToFile(Dictionary<string, object> configDict = null)
        {
            if (configDict == null)
                configDict = dict;
            XmlDocument xmlDoc = new XmlDocument();

            //var delcaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            //xmlDoc.AppendChild(delcaration);

            XmlElement root = xmlDoc.CreateElement("config");
            foreach (var key in configDict.Keys)
            {
                var e = xmlDoc.CreateElement(key);
                e.InnerText = configDict[key].ToString();
                root.AppendChild(e);
            }
            xmlDoc.AppendChild(root);

            xmlDoc.Save(configFile);
        }

        public Dictionary<string, object> Read()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFile);

            var root = xmlDoc.FirstChild;
            foreach (XmlElement element in root.ChildNodes)
            {
                dict[element.Name] = element.InnerText;
                Console.WriteLine(element);
            }
            return dict;
        }


    }
}
