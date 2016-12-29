using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace boardProto
{
    class MapSaver
    {
        string mapName;

        public void SaveFile(MapData _data)
        {
            // Writer settings
            XmlWriterSettings xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(mapName + ".map", xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("MapData");

                foreach (var _l1Tile in _data.L1Tiles)
                {
                    xmlWriter.WriteStartElement("L1Tile");

                    xmlWriter.WriteElementString("Position", _l1Tile.TilePosition.ToString());
                    xmlWriter.WriteElementString("Offset", _l1Tile.TileOffset.ToString());
                    xmlWriter.WriteElementString("Texture", _l1Tile.TileTexture.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                Console.WriteLine("Map saved.");
            }
        }
    }
}
