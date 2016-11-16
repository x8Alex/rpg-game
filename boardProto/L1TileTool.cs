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

        Rectangle openMenuRectangle = new Rectangle(0, 0, 600, 110);    // Rectangle boundaries for the menu while open
        Rectangle closedMenuRectangle = new Rectangle(0, 0, 100, 100);  // Rectangle boundaries for the menu while closed, only shows selected texture

        Texture2D selectedTileTexture;
        bool tileSelectionMenuShow;

        public L1TileTool(List <Texture2D> _listTileTextures)
        {
            listTileTextures = _listTileTextures;
            selectedTileTexture = listTileTextures[1];
            tileSelectionMenuShow = true;
            ignoredAreas = new List<Rectangle>();   // Initializes an empty list
            ignoredAreas.Add(openMenuRectangle);    
        }

        public void TileSelection()
        {

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
        public Rectangle ClosedMenuRectangle
        {
            get { return closedMenuRectangle; }
            set { closedMenuRectangle = value; }
        }
    }
}
