using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace boardProto
{
    class L2TileTool
    {
        List<Texture2D> listTextures;
        Vector2 tileOffset;                     // Tile offset for textures like grass that are not fully flat. Ex: Grass
        List<Texture2D> listButtonTextures;     // Odd indexes are off, even are on
        List<ToolTileButton> listMenuButtons;   // List of buttons. There are as many buttons as there are 1x1 tiles
        List<Rectangle> ignoredAreas;

        Rectangle rectangleMenuOpen;    // Rectangle boundaries for the menu while open
        Rectangle rectangleMenuClosed;  // Rectangle boundaries for the menu while closed, only shows selected texture

        int tileTextureIndex;
        Texture2D textureMenuOpen;
        Texture2D textureMenuClosed;
        Texture2D selectedTileTexture;
        EditorManager.L2TileType selectedTileType;
        bool tileSelectionMenuShow;

        public L2TileTool(Texture2D _textureMenuOpen, Texture2D _textureMenuClosed, List<Texture2D> _listTextures)
        {
            listButtonTextures = new List<Texture2D>();
            ignoredAreas = new List<Rectangle>();           // Initializes an empty list
            listMenuButtons = new List<ToolTileButton>();

            listTextures = _listTextures;
            textureMenuOpen = _textureMenuOpen;
            textureMenuClosed = _textureMenuClosed;
            rectangleMenuOpen = new Rectangle(0, 0, textureMenuOpen.Width, textureMenuOpen.Height);
            rectangleMenuClosed = new Rectangle(0, 0, textureMenuClosed.Width, textureMenuClosed.Height);
            tileSelectionMenuShow = false;
            ignoredAreas.Add(rectangleMenuClosed);

            // Isolates the button textures and assigns them to listTileButtons
            foreach (var _buttonTexture in listTextures)
            {
                if (_buttonTexture.ToString().Substring(14).Contains("ButtonTile"))
                {
                    listButtonTextures.Add(_buttonTexture);
                }
            }

            // Creates buttons and assigns them to a list
            tileTextureIndex = 0;
            foreach (var _texture in listTextures)
            {
                if (_texture.ToString().Contains("Walls"))
                    if(_texture.ToString().Substring(12).Contains("1x1") ||
                       _texture.ToString().Substring(_texture.ToString().Length - 1).Contains("A"))
                {
                    listMenuButtons.Add(new ToolTileButton(tileTextureIndex++, listButtonTextures, _texture));
                    Console.WriteLine(_texture.ToString());
                }
            }

            tileTextureIndex = listTextures.FindIndex(a => a.ToString().Substring(13).Contains("1x1") ||
                                                           a.ToString().Substring(a.ToString().Length - 1).Contains("A"));
            selectedTileTexture = listTextures[tileTextureIndex];

             if (selectedTileTexture.ToString().Contains("Rock"))
            {
                tileOffset = new Vector2(0, 0);
                selectedTileType = EditorManager.L2TileType.RockS;
            }
            else
            {
                selectedTileType = EditorManager.L2TileType.Default;
            }
        }

        // Tile selection for increasing or decreasing the tile size
        public void TileSelection(String _changeSize)
        {
            // Handles enlarging and shrinking of tiles
            switch (_changeSize)
            {
                // Increase tile size
                case "BIGGER":
                    if (!selectedTileTexture.ToString().Contains("4x4") &&
                        !selectedTileTexture.ToString().Substring(selectedTileTexture.ToString().Length - 1).Contains("C"))
                    {
                        try
                        {
                            selectedTileTexture = listTextures[tileTextureIndex + 1];
                            tileTextureIndex++;
                            
                            selectedTileType = (EditorManager.L2TileType)(int)selectedTileType++;
                            tileOffset = new Vector2(0, -selectedTileTexture.Height + 40);
                        }
                        catch { }
                    }
                    break;

                // Decrease tile size
                case "SMALLER":
                    if (!selectedTileTexture.ToString().Contains("1x1") &&
                        !selectedTileTexture.ToString().Substring(selectedTileTexture.ToString().Length - 1).Contains("A"))
                    {
                        Console.WriteLine(tileTextureIndex);
                        try
                        {
                            selectedTileTexture = listTextures[tileTextureIndex - 1];
                            tileTextureIndex--;

                            selectedTileType = selectedTileType--;
                            tileOffset = new Vector2(0, -selectedTileTexture.Height + 40);
                        }
                        catch { }
                    }
                    break;
            }
        }

        // Tile selection from menu
        public void TileSelection(MouseState _mouseState, Vector2 _mousePosition)
        {
            foreach (ToolTileButton _button in listMenuButtons)
            {
                if (_mousePosition.X >= _button.GetButtonBoundaries().Left &&
                    _mousePosition.X <= _button.GetButtonBoundaries().Right &&
                    _mousePosition.Y >= _button.GetButtonBoundaries().Top &&
                    _mousePosition.Y <= _button.GetButtonBoundaries().Bottom &&
                    _mouseState.LeftButton == ButtonState.Pressed &&
                    tileSelectionMenuShow)
                {
                    selectedTileTexture = _button.ButtonTileTexture;
                    tileTextureIndex = listTextures.FindIndex(a => a.ToString().Contains(_button.ButtonTileTexture.ToString()));
                    if (selectedTileTexture.ToString().Contains("RockA") ||
                        selectedTileTexture.ToString().Substring(selectedTileTexture.ToString().Length - 1).Contains("A"))
                        selectedTileType = EditorManager.L2TileType.RockS;

                    if (selectedTileTexture.ToString().Contains("RockA"))
                        tileOffset = new Vector2(0, 0);
                    else
                        tileOffset = new Vector2(0, 0);
                }
            }
        }

        public List<L2Tile> PlaceTile(List<L2Tile> _listL2Tiles, Vector2 _tilePosition)
        {
            // 0.00001f is passed when mouse clicks occur within the boundaries of displayed menus
            if (_tilePosition != new Vector2(0.00001f, 0.00001f))
            {
                // If tile is not occupied
                if (!_listL2Tiles.Exists(a => a.TilePosition == _tilePosition))
                {
                    // Add a tile to Layer1Tiles with arguments "tile type", "tile position", "passable T/F", "texture".
                    _listL2Tiles.Add(new L2Tile(selectedTileType, _tilePosition, true, listTextures.IndexOf(SelectedTileTexture, 0, listTextures.Count)));
                }
                // If tile is occupied check if the tile being placed is the same as the one that already exists
                else if (_listL2Tiles[_listL2Tiles.FindIndex(a => a.TilePosition == _tilePosition)].TextureIndex !=
                                                                                                    listTextures.IndexOf(SelectedTileTexture, 0, listTextures.Count))
                {
                    // Overwrites the tile to Layer1Tiles with arguments "tile type", "tile position", "passable T/F", "texture".
                    _listL2Tiles[_listL2Tiles.FindIndex(a => a.TilePosition == _tilePosition)] =
                                new L2Tile(selectedTileType, _tilePosition, true, listTextures.IndexOf(SelectedTileTexture, 0, listTextures.Count));
                }
            }

            return _listL2Tiles;
        }


        // GETTERS AND SETTERS =====================================
        public List<Rectangle> IgnoredAreas
        {
            get { return ignoredAreas; }
        }
        public List<Texture2D> ListTextures
        {
            get { return listTextures; }
            set { listTextures = value; }
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
        public List<ToolTileButton> ListMenuButtons
        {
            get { return listMenuButtons; }
            set { listMenuButtons = value; }
        }
        public Vector2 TileOffset
        {
            get { return tileOffset; }
            set { tileOffset = value; }
        }

        //public List<Texture2D> listTextures { get; set; }
    }
}
