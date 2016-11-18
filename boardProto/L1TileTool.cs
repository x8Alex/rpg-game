using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class L1TileTool
    {
        List<Texture2D> listTileTextures;
        List<Rectangle> ignoredAreas;

        Rectangle rectangleMenuOpen;    // Rectangle boundaries for the menu while open
        Rectangle rectangleMenuClosed;  // Rectangle boundaries for the menu while closed, only shows selected texture

        int tileTextureIndex = 0;
        Texture2D textureMenuOpen;
        Texture2D textureMenuClosed;
        Texture2D selectedTileTexture;
        bool tileSelectionMenuShow;

        public L1TileTool(Texture2D _textureMenuOpen, Texture2D _textureMenuClosed, List <Texture2D> _listTileTextures)
        {
            listTileTextures = _listTileTextures;
            textureMenuOpen = _textureMenuOpen;
            textureMenuClosed = _textureMenuClosed;
            selectedTileTexture = listTileTextures[tileTextureIndex];
            tileSelectionMenuShow = true;
            rectangleMenuOpen = new Rectangle(0, 0, textureMenuOpen.Width, textureMenuOpen.Height);
            rectangleMenuClosed = new Rectangle(0, 0, textureMenuClosed.Width, textureMenuClosed.Height);
            ignoredAreas = new List<Rectangle>();   // Initializes an empty list
            ignoredAreas.Add(rectangleMenuOpen);    
        }

        public void TileSelection(String _changeSize)
        {
            if (_changeSize == "BIGGER" && !selectedTileTexture.ToString().Contains("4x4"))
                // Increase tile size
                selectedTileTexture = listTileTextures[++tileTextureIndex];
            else if (_changeSize == "SMALLER" && !selectedTileTexture.ToString().Contains("1x1"))
                // Decrease tile size
                selectedTileTexture = listTileTextures[--tileTextureIndex];
        }

        public List<L1Tile> PlaceTile(List <L1Tile> _listL1Tiles, Vector2 _tilePosition)
        {
            // 0.00001f is passed when mouse clicks occur within the boundaries of displayed menus
            if (_tilePosition != new Vector2(0.00001f, 0.00001f))
            {
                // If tile is not occupied
                if (!_listL1Tiles.Exists(a => a.TilePosition == _tilePosition))
                {
                    // Add a tile to Layer1Tiles with arguments "tile type", "tile position", "passable T/F", "texture".
                    _listL1Tiles.Add(new L1Tile(EditorManager.L1TileType.Dirt1x1, _tilePosition, true, SelectedTileTexture));
                }
                // If tile is occupied check if the tile being placed is the same as the one that already exists
                else if (_listL1Tiles[_listL1Tiles.FindIndex(a => a.TilePosition == _tilePosition)].TileTexture !=
                                                                                                    SelectedTileTexture)
                {
                    // Overwrites the tile to Layer1Tiles with arguments "tile type", "tile position", "passable T/F", "texture".
                    _listL1Tiles[_listL1Tiles.FindIndex(a => a.TilePosition == _tilePosition)] =
                                new L1Tile(EditorManager.L1TileType.Dirt1x1, _tilePosition, true, SelectedTileTexture);
                }
            }

            return _listL1Tiles;
        }

        
        // GETTERS AND SETTERS =====================================
        public List<Rectangle> IgnoredAreas
        {
            get { return ignoredAreas; }
        }
        public List<Texture2D> ListTileTextures
        {
            get { return listTileTextures; }
            set { listTileTextures = value; }
        }
        public bool TileSelectionMenuShow
        {
            get { return tileSelectionMenuShow; }
            set { tileSelectionMenuShow = value; }
        }
        public Texture2D SelectedTileTexture
        {
            get { return selectedTileTexture; }
            set { selectedTileTexture = value; }
        }
        public Texture2D TextureMenuOpen
        {
            get { return textureMenuOpen; }
            set { textureMenuOpen = value; }
        }
        public Texture2D TextureMenuClosed
        {
            get { return textureMenuClosed; }
            set { textureMenuClosed = value; }
        }
        public Rectangle RectangleMenuOpen
        {
            get { return rectangleMenuOpen; }
            set { rectangleMenuOpen = value; }
        }
        public Rectangle RectangleMenuClosed
        {
            get { return rectangleMenuClosed; }
            set { rectangleMenuClosed = value; }
        }
    }
}
