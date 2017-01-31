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

namespace boardProto
{
    internal partial class SaveMap : Form
    {
        private MapData mapData;
        string mapName;
        XmlWriterSettings xmlSettings;
        string exeFilePath;
        string mapFolderName;
        string mapFolderPath;

        public SaveMap(MapData _mapData)
        {
            mapName = "";
            exeFilePath = AppDomain.CurrentDomain.BaseDirectory;
            mapFolderName = "\\Maps\\";
            mapFolderPath = exeFilePath + mapFolderName;
            mapData = _mapData;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitializeComponent();
        }

        public void ChangeNameTextBox(string _mapName)
        {
            textBoxMapName.Text = _mapName;
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
            Console.WriteLine("Assigned name: " + mapName);

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

                    xmlWriter.WriteElementString("Position", _l1Tile.TilePosition.ToString());
                    xmlWriter.WriteElementString("Offset", _l1Tile.TileOffset.ToString());
                    xmlWriter.WriteElementString("Texture", _l1Tile.TileTexture.ToString());

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

        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }
    }
}
