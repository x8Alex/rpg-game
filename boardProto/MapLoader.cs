using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace boardProto
{
    class MapLoader
    {
        string mapName = "testMap";

        public void LoadMap()
        {
            Console.WriteLine("LoadMap called.");
            using (XmlReader xmlReader = XmlReader.Create(mapName + ".map"))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {
                        Console.WriteLine("IsStartElement called. {0}", xmlReader.Name);
                        switch (xmlReader.Name)
                        {
                            case "L1Tile":
                                Console.WriteLine("Start {0} element.", xmlReader.Name);
                                break;
                            case "Position":
                                if (xmlReader.Read())
                                {
                                    Console.WriteLine(xmlReader.Value);
                                }
                                break;
                            case "Tile":
                                Console.WriteLine("Start {0} element.", xmlReader.Name);
                                break;
                        }
                    }
                }
            }
        }
    }
}
