using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Folium.Main
{
    public class Config
    {
        public static Dictionary<String, float> settings;

        public Config(String configFileURL)
        {
            settings                    = new Dictionary<string,float>();
            XmlTextReader configFile    = new XmlTextReader(configFileURL);
            String elementName          = "";

            while (configFile.Read())
            {
                switch (configFile.NodeType)
                {
                    case XmlNodeType.Element: elementName = configFile.Name; break;
                    case XmlNodeType.Text: settings.Add(elementName, Convert.ToSingle(configFile.Value)); break;
                }
            }

            configFile.Close();
        }
    }
}
