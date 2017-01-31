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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// TO DO:
// Allow user to edit map folder path

namespace boardProto
{
    public partial class LoadMap : Form
    {
        GraphicsDevice graphicsDevice;
        string mapName;
        string exeFilePath;
        string mapFolderName;
        string mapFolderPath;

        MapData mapData;
        L1Tile tempL1Tile;
        L2Tile tempL2Tile;
        tileType tempTileType;

        enum tileType
        {
            L1Tile,
            L2Tile
        };

        public LoadMap(GraphicsDevice _graphicsDevice)
        {
            graphicsDevice = _graphicsDevice;
            // Changes form so it is not resizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            mapName = "newMap";
            exeFilePath = AppDomain.CurrentDomain.BaseDirectory;
            mapFolderName = "Maps\\";
            mapFolderPath = exeFilePath + mapFolderName;
            Console.WriteLine(exeFilePath.ToString());

            InitializeComponent();
        }

        private void LoadMap_Load(object sender, EventArgs e)
        {
            // Changes textBoxMapName to mapName
            textBoxMapName.Text = mapName;
            // Changes the path text box text
            textBoxMapPath.Text = "~" + mapFolderName;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Console.WriteLine("LoadMap called.");
            tempL1Tile = new L1Tile();
            mapData = new MapData();

            mapData.MapName = mapName;

            using (XmlReader xmlReader = XmlReader.Create(mapFolderPath + mapName + ".map"))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {
                        switch (xmlReader.Name)
                        {
                            case "L1Tile":
                                tempL1Tile = new L1Tile();
                                tempTileType = tileType.L1Tile;
                                break;
                            case "L2Tile":
                                tempL2Tile = new L2Tile();
                                tempTileType = tileType.L2Tile;
                                break;
                            case "Type":
                                if (xmlReader.Read())
                                {
                                    if (tempTileType == tileType.L1Tile)
                                    {
                                        // Loads the tile type
                                        tempL1Tile.TileType = (EditorManager.L1TileType)Enum.Parse(typeof(EditorManager.L1TileType), xmlReader.Value);
                                    }
                                    else if (tempTileType == tileType.L2Tile)
                                    {
                                        // Loads the tile type
                                        tempL2Tile.TileType = (EditorManager.L2TileType)Enum.Parse(typeof(EditorManager.L2TileType), xmlReader.Value);
                                    }
                                }
                                break;
                            case "Position":
                                if (xmlReader.Read())
                                {
                                    Vector2 tempVector;

                                    if (tempTileType == tileType.L1Tile)
                                    {
                                        // Parses the tile position from string
                                        int startInd = xmlReader.Value.IndexOf("X:") + 2;
                                        tempVector.X = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf(" Y") - startInd));
                                        startInd = xmlReader.Value.IndexOf("Y:") + 2;
                                        tempVector.Y = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf("}") - startInd));
                                        tempL1Tile.TilePosition = tempVector;
                                    }
                                    else if (tempTileType == tileType.L2Tile)
                                    {
                                        // Parses the tile position from string
                                        int startInd = xmlReader.Value.IndexOf("X:") + 2;
                                        tempVector.X = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf(" Y") - startInd));
                                        startInd = xmlReader.Value.IndexOf("Y:") + 2;
                                        tempVector.Y = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf("}") - startInd));
                                        tempL2Tile.TilePosition = tempVector;
                                    }
                                }
                                break;
                            case "Offset":
                                if (xmlReader.Read())
                                {
                                    Vector2 tempVector;

                                    if (tempTileType == tileType.L1Tile)
                                    {
                                        // Parses the tile offset from string
                                        int startInd = xmlReader.Value.IndexOf("X:") + 2;
                                        tempVector.X = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf(" Y") - startInd));
                                        startInd = xmlReader.Value.IndexOf("Y:") + 2;
                                        tempVector.Y = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf("}") - startInd));
                                        tempL1Tile.TileOffset = tempVector;
                                    }
                                    else if (tempTileType == tileType.L2Tile)
                                    {
                                        // Parses the tile offset from string
                                        int startInd = xmlReader.Value.IndexOf("X:") + 2;
                                        tempVector.X = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf(" Y") - startInd));
                                        startInd = xmlReader.Value.IndexOf("Y:") + 2;
                                        tempVector.Y = float.Parse(xmlReader.Value.Substring(
                                                                    startInd,
                                                                    xmlReader.Value.IndexOf("}") - startInd));
                                        tempL2Tile.TileOffset = tempVector;
                                    }
                                }
                                break;
                            case "TextureIndex":
                                if (xmlReader.Read())
                                {
                                    if (tempTileType == tileType.L1Tile)
                                    {
                                        // Loads the texture index value
                                        tempL1Tile.TextureIndex = Convert.ToInt32(xmlReader.Value);
                                    }
                                    else if (tempTileType == tileType.L2Tile)
                                    {
                                        // Loads the texture index value
                                        tempL2Tile.TextureIndex = Convert.ToInt32(xmlReader.Value);
                                    }
                                }

                                // Adds the final temporary tile to the tile list in map data
                                if (tempL1Tile != null && tempTileType == tileType.L1Tile)
                                    mapData.L1Tiles.Add(tempL1Tile);
                                else if (tempL2Tile != null && tempTileType == tileType.L2Tile)
                                    mapData.L2Tiles.Add(tempL2Tile);
                                break;
                            case "Tile":
                                break;
                        }
                    }
                }
            }
            Console.WriteLine("Map loaded.");
            this.Close();
        }

        private Texture2D LoadTileTexture(String _texData, GraphicsDevice _graphicsDevice)
        {
            byte[] dataArray = Convert.FromBase64String(_texData);

            using (MemoryStream memStream = new MemoryStream(dataArray))
            using (BinaryReader binReader = new BinaryReader(memStream))
            {
                int width = binReader.ReadInt32();
                int height = binReader.ReadInt32();
                int length = binReader.ReadInt32();
                Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[length];

                for (int i = 0; i < data.Length; i++)
                {
                    byte R = binReader.ReadByte();
                    byte G = binReader.ReadByte();
                    byte B = binReader.ReadByte();
                    byte A = binReader.ReadByte();
                    data[i] = new Microsoft.Xna.Framework.Color(R, G, B, A);
                }

                Texture2D tileTexture = new Texture2D(_graphicsDevice, width, height);
                tileTexture.SetData<Microsoft.Xna.Framework.Color>(data, 0, data.Length);
                return tileTexture;
            }

        }

        internal MapData MapData
        {
            get { return mapData; }
            set { mapData = value; }
        }

        private void textBoxMapName_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
