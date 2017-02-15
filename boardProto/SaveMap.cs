using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    internal partial class SaveMap : Form
    {
        private MapData mapData;
        XmlWriterSettings xmlSettings;
        string mapName;
        string exeFilePath;
        string mapFolderName;
        string mapFolderPath;

        public SaveMap(MapData _mapData)
        {
            // Changes form so it is not resizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            mapName = "";
            exeFilePath = AppDomain.CurrentDomain.BaseDirectory;
            mapFolderName = "\\Maps\\";
            mapFolderPath = exeFilePath + mapFolderName;
            mapData = _mapData;

            InitializeComponent();
        }

        private void SaveMap_Load(object sender, EventArgs e)
        {
            // Writer settings
            xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };

            // Changes the path text box text
            textBoxMapPath.Text = "~" + mapFolderName;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            mapName = textBoxMapName.Text;

            // Checks if directory exists, creates it if doesn't
            if (!Directory.Exists(mapFolderPath))
                Directory.CreateDirectory(mapFolderPath);

            // If name is blank
            if (mapName == "")
            {
                mapName = "newMap";
            }

            // Save map to file
            using (XmlWriter xmlWriter = XmlWriter.Create(mapFolderPath + mapName + ".map", xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("MapData");

                foreach (var _l1Tile in mapData.L1Tiles)
                {
                    xmlWriter.WriteStartElement("L1Tile");
                    xmlWriter.WriteElementString("Type", _l1Tile.TileType.ToString());
                    xmlWriter.WriteElementString("Position", _l1Tile.TilePosition.ToString());
                    xmlWriter.WriteElementString("Offset", _l1Tile.TileOffset.ToString());
                    xmlWriter.WriteElementString("TextureID", _l1Tile.TextureID.ToString());

                    xmlWriter.WriteEndElement();
                }
                foreach (var _l2Tile in mapData.L2Tiles)
                {
                    xmlWriter.WriteStartElement("L2Tile");
                    xmlWriter.WriteElementString("Type", _l2Tile.TileType.ToString());
                    xmlWriter.WriteElementString("Position", _l2Tile.TilePosition.ToString());
                    xmlWriter.WriteElementString("Offset", _l2Tile.TileOffset.ToString());
                    xmlWriter.WriteElementString("TextureID", _l2Tile.TextureID.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }

            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ChangeNameTextBox(string _mapName)
        {
            textBoxMapName.Text = _mapName;
        }

        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }
    }
}
