using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Folium.Main
{
    public class Config
    {
        public static Dictionary<String, float> settings;

        public Config(String configFileURL)
        {
            settings                    = new Dictionary<string, float>();
            XmlTextReader configFile    = new XmlTextReader(configFileURL);
            bool doingFirstElement      = true;
            List<String> elementList    = new List<string>(4);

            while (configFile.Read())
            {
                switch (configFile.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            if (doingFirstElement)
                            {
                                doingFirstElement = false;
                                break;
                            }

                            elementList.Add(configFile.Name);
                        } break;
                    case XmlNodeType.EndElement:
                        {
                            if(elementList.Count > 0)
                                elementList.RemoveAt(elementList.Count-1);
                        } break;
                    case XmlNodeType.Text:
                        {
                            String elementName = "";
                            foreach (String element in elementList)
                                elementName += element + ".";

                            elementName = elementName.Remove(elementName.Length-1);

                            settings.Add(elementName, Convert.ToSingle(configFile.Value, CultureInfo.InvariantCulture));
                        } break;
                }
            }

            configFile.Close();
        }
    }
}
