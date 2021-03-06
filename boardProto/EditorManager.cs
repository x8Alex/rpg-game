﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace boardProto
{
    class EditorManager
    {
        // Name of the map
        string mapName;
        // Offset from the bottom corner of the map
        Vector2 worldOffset = new Vector2(0, 0);

        static float tileAngleRads = (float)(26.565 * (Math.PI/180));
        Vector2 tileSize = new Vector2(80f, (float)(80 * Math.Tan(tileAngleRads)));

        MouseState mouseState;
        Vector2 lastMousePosition;
        Vector2 mouseMapTilePosition;

        Vector2 gridVisibilityLower;
        Vector2 gridVisibilityUpper;
        Vector2 gridVisibilityLowerTransformed;
        Vector2 gridVisibilityUpperTransformed;
        // A list of all textures to be loaded
        List<Texture2D> listTileTextures;
        Texture2D textureEmptySpace;
        Texture2D textureGridGray;
        // Used to draw the current selected texture
        Texture2D selectedTileTexture;
        //Vector2 tileOffset;
        // Menu frame textures
        Texture2D textureMenuOpen;
        Texture2D textureMenuClosed;
        
        // Currently selected tool - enum value
        EditorTools activeTool;
        EditorTools lastActiveTool;
        L1TileTool l1TileTool;
        L2TileTool l2TileTool;
        List<Rectangle> ignoredMenuAreas;

        // Currently selected tile coordinates
        Vector2 selectedTileCoords;

        // Tool menu isn't displayed by default
        bool activeToolMenuShow = false;

        // Shows the grid by default
        bool gridShow = true;
        bool deleteTool = false;

        // Editor tools
        public enum EditorTools
        {
            None,
            TileDelete,
            TileMover,
            L1TilePlacer,
            L2TilePlacer
        };

        // Layers of the map
        List<L1Tile> listL1Tiles;   // X, Y position
        List<L2Tile> listL2Tiles;
        List<Vector3> listL3Tiles;

        // Layer 1 tiles
        public enum L1TileType 
        {
            Default,
            Sand,
            GrassThick
        };

        // Layer 2 tiles
        public enum L2TileType
        {
            Default,
            RockS,
            RockM,
            RockL
        };

        public void Initialize(List<Texture2D> _texture_list)
        {
            listTileTextures = new List<Texture2D>();
            listL1Tiles = new List<L1Tile>();               // L1Tile objects that will be written and read from the map files
            listL2Tiles = new List<L2Tile>();               // L2Tile...

            // Adds tile textures to listTileTextures. "i" equals to the first tile texture
            for (int i = 4; i < _texture_list.Count; i++)   // Passes textures starting from the button textures
                listTileTextures.Add(_texture_list[i]);
            
            textureEmptySpace = _texture_list[0];
            textureGridGray = _texture_list[1];
            textureMenuOpen = _texture_list[2];
            textureMenuClosed = _texture_list[3];

            activeTool = EditorTools.None;      // Default active tool
            lastActiveTool = activeTool;                // Sets the last active tool. Used when tool needs to be temporarily disabled
            l1TileTool = new L1TileTool(textureMenuOpen, textureMenuClosed, listTileTextures);
            l2TileTool = new L2TileTool(textureMenuOpen, textureMenuClosed, listTileTextures);

            // ignoredMenuAreas will be shared between all tools
            ignoredMenuAreas = l1TileTool.IgnoredAreas; // ASSUMES L1EDITORTOOL AND MENUS ARE ACTIVE **********
        }

        public void PassMapData(MapData _mapData)
        {
            try
            {
                mapName = _mapData.MapName;
                try
                {
                    listL1Tiles = _mapData.L1Tiles;
                }
                catch
                {
                    Console.WriteLine("Could not pass L1 data.");
                }
                try
                {
                    listL2Tiles = _mapData.L2Tiles;
                }
                catch
                {
                    Console.WriteLine("Could not pass L2 data.");
                }
            }
            catch
            {
                Console.WriteLine("Could not pass map data.");
            }
        }

        // Scrolls the world when RMB is held down and mouse moved around
        public void ScrollWorld(MouseState _mouseState, Vector2 mousePosition)
        {
            mouseState = _mouseState;
            if (mouseState.RightButton != ButtonState.Pressed)
            {
                lastMousePosition.X = mousePosition.X;
                lastMousePosition.Y = mousePosition.Y;
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                worldOffset.X += mousePosition.X - lastMousePosition.X;
                worldOffset.Y += mousePosition.Y - lastMousePosition.Y;
                lastMousePosition.X = mousePosition.X;
                lastMousePosition.Y = mousePosition.Y;
            }
        }

        // Places, edits, deletes tiles in the world
        public void EditTiles(KeyboardState _keyboardState, MouseState _mouseState, MouseState _oldMouseState, Vector2 _tilePosition)
        {
            //_tilePosition += L1TileTool.TileOffset;
            // Check which panel is active
            // ===========================================================================
            if (activeTool == EditorTools.L1TilePlacer)         // ======= LAYER 1 =======
            {
                // Toggle tile selection menu
                if (l1TileTool.TileSelectionMenuShow != activeToolMenuShow)
                {
                    l1TileTool.TileSelectionMenuShow = activeToolMenuShow;

                    if (activeToolMenuShow)
                        ignoredMenuAreas = new List<Rectangle>() { l1TileTool.RectangleMenuOpen };
                    else
                        ignoredMenuAreas = new List<Rectangle>() { l1TileTool.RectangleMenuClosed };
                }

                // Tile selection
                selectedTileTexture = l1TileTool.SelectedTileTexture;

                // Tile placement
                if (_mouseState.LeftButton == ButtonState.Pressed)
                {
                    listL1Tiles = l1TileTool.PlaceTile(listL1Tiles, _tilePosition);
                }
            }
            else if (activeTool == EditorTools.L2TilePlacer)         // ======= LAYER 2 =======
            {
                // Toggle tile selection menu
                if (l2TileTool.TileSelectionMenuShow != activeToolMenuShow)
                {
                    l2TileTool.TileSelectionMenuShow = activeToolMenuShow;

                    if (activeToolMenuShow)
                        ignoredMenuAreas = new List<Rectangle>() { l2TileTool.RectangleMenuOpen };
                    else
                        ignoredMenuAreas = new List<Rectangle>() { l2TileTool.RectangleMenuClosed };
                }

                // Tile selection
                selectedTileTexture = l2TileTool.SelectedTileTexture;

                // Tile placement
                if (_mouseState.LeftButton == ButtonState.Pressed)
                {
                    listL2Tiles = l2TileTool.PlaceTile(listL2Tiles, _tilePosition);
                }
            }
            else if (activeTool == EditorTools.TileDelete)
            {
                
                while (_mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                {
                    // Tries to remove a tile if found in each list. Throws error on blank tiles.
                    // Removal is done from the highest layer to the lowest in order to not accidentally remove
                    // any tiles underneath.
                    try
                    {
                        listL2Tiles.RemoveAt(listL2Tiles.FindIndex(a => a.TilePosition == mouseMapTilePosition));
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("No tile to delete.");
                    }
                    try
                    {
                        listL1Tiles.RemoveAt(listL1Tiles.FindIndex(a => a.TilePosition == mouseMapTilePosition));
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("No tile to delete.");
                    }
                    break;
                }
            }
            else if (activeTool == EditorTools.TileMover)
            {
                if (_mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                {
                    try
                    {
                        selectedTileCoords = listL2Tiles[listL2Tiles.FindIndex(a => a.TilePosition == mouseMapTilePosition)].TilePosition;
                        Console.WriteLine("selected tile: " + selectedTileCoords);
                    }
                    catch
                    {
                        Console.WriteLine("No tile selected.");
                        selectedTileCoords = new Vector2(0.0001f, 0);
                    }
                }

                if (_keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    try
                    {
                        L2Tile tile = listL2Tiles[listL2Tiles.FindIndex(a => a.TilePosition == selectedTileCoords)];
                        listL2Tiles.Insert(listL2Tiles.Count, tile);
                        listL2Tiles.Remove(tile);
                    }
                    catch { }
                }
                else if (_keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    try
                    {
                        L2Tile tile = listL2Tiles[listL2Tiles.FindIndex(a => a.TilePosition == selectedTileCoords)];
                        listL2Tiles.Remove(tile);
                        listL2Tiles.Insert(0, tile);
                    }
                    catch { }
                }
            }
        }

        // Detects which tile the mouse is hovered over and returns the matrix position
        public Vector2 DetectClosestTilePosition(Vector2 _mouseWorldPosition, List<Rectangle> _ignoredAreas)
        {
            // Runs the check for each area in the list of ignored areas
            foreach (Rectangle _area in _ignoredAreas)
            {
                // Checks if mouse is within the area to be ignored
                if (Mouse.GetState().X > _area.X && Mouse.GetState().X < _area.X + _area.Width &&
                    Mouse.GetState().Y > _area.Y && Mouse.GetState().Y < _area.Y + _area.Height)
                {
                    return new Vector2(0.00001f, 0.00001f); // Returns this when mouse is within menu boundaries
                }
                // If mouse is not within the ignored areas
                else
                {
                    mouseMapTilePosition.X = (int)((_mouseWorldPosition.X / (tileSize.X / 2) + 
                                                    _mouseWorldPosition.Y / (tileSize.Y / 2)) / 2);
                    mouseMapTilePosition.Y = (int)((_mouseWorldPosition.Y / (tileSize.Y / 2) -
                                                    _mouseWorldPosition.X / (tileSize.X / 2)) / 2);

                    if (mouseMapTilePosition.X >= 0 && mouseMapTilePosition.Y >= 0)
                        return mouseMapTilePosition;
                }
            }
            return new Vector2(0.00001f, 0.00001f);
        }

        // Draws editor panels
        public void DrawActivePanel(SpriteBatch spriteBatch, EditorTools _activeTool)
        {
            // LAYER 1 Tile Tool
            if (_activeTool == EditorTools.L1TilePlacer && activeToolMenuShow)
            {
                // Draws the OPEN menu frame
                spriteBatch.Draw(l1TileTool.TextureMenuOpen, l1TileTool.RectangleMenuOpen, Color.White);
                
                // Draws the tile selection buttons
                foreach (ToolTileButton _button in l1TileTool.ListMenuButtons)
                {
                    _button.Draw(spriteBatch, l1TileTool.SelectedTileTexture);
                }
            }
            // LAYER 2 Tile Tool
            else if (_activeTool == EditorTools.L2TilePlacer && activeToolMenuShow)
            {
                // Draws the OPEN menu frame
                spriteBatch.Draw(l2TileTool.TextureMenuOpen, l1TileTool.RectangleMenuOpen, Color.White);

                // Draws the tile selection buttons
                foreach (ToolTileButton _button in l2TileTool.ListMenuButtons)
                {
                    _button.Draw(spriteBatch, l2TileTool.SelectedTileTexture);
                }
            }
            else if (_activeTool != EditorTools.None &&
                     _activeTool != EditorTools.TileDelete &&
                     _activeTool != EditorTools.TileMover &&
                     !activeToolMenuShow)
            {
                spriteBatch.Draw(l1TileTool.TextureMenuClosed, l1TileTool.RectangleMenuClosed, Color.White);
            }
        }

        // Draws a black background
        public void DrawEmptySpace(SpriteBatch spriteBatch, Vector2 windowSize){
            spriteBatch.Draw(textureEmptySpace, new Rectangle(0, 0, (int)windowSize.X, (int)windowSize.Y), Color.White);
        }

        // Draws the tiles
        public void DrawTiles(SpriteBatch spriteBatch, Vector2 pos, Texture2D _selectedTileTexture)
        {
            // Draws all created tiles
            // Layer 1
            int tempTextureIndex;
            foreach (var _tile in listL1Tiles)
            {
                tempTextureIndex = listTileTextures.FindIndex(a => a.ToString() == _tile.TextureID);
                if (_tile.TileOffset == new Vector2(0,0))
                    spriteBatch.Draw(listTileTextures[tempTextureIndex], 
                                     new Vector2((_tile.TilePosition.X - _tile.TilePosition.Y) *
                                     (tileSize.X / 2) - listTileTextures[tempTextureIndex].Width / 2,
                                     (_tile.TilePosition.X + _tile.TilePosition.Y) * (tileSize.Y / 2))
                                                    + worldOffset,
                                 null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(listTileTextures[tempTextureIndex], new Vector2((_tile.TilePosition.X - _tile.TilePosition.Y) *
                                                                    (tileSize.X / 2) - listTileTextures[tempTextureIndex].Width / 2,
                                                                    (_tile.TilePosition.X + _tile.TilePosition.Y) * (tileSize.Y / 2))
                                                    + worldOffset + _tile.TileOffset,
                                 null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            // Layer 2
            foreach (var _tile in listL2Tiles)
            {
                tempTextureIndex = listTileTextures.FindIndex(a => a.ToString() == _tile.TextureID);
                if (_tile.TileOffset == new Vector2(0, 0))
                    spriteBatch.Draw(listTileTextures[tempTextureIndex], new Vector2((_tile.TilePosition.X - _tile.TilePosition.Y) *
                                                                    (tileSize.X / 2) - listTileTextures[tempTextureIndex].Width / 2,
                                                                    (_tile.TilePosition.X + _tile.TilePosition.Y) * (tileSize.Y / 2))
                                                    + worldOffset,
                                 null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(listTileTextures[tempTextureIndex], new Vector2((_tile.TilePosition.X - _tile.TilePosition.Y) *
                                                                    (tileSize.X / 2) - listTileTextures[tempTextureIndex].Width / 2,
                                                                    (_tile.TilePosition.X + _tile.TilePosition.Y) * (tileSize.Y / 2))
                                                    + worldOffset + _tile.TileOffset,
                                 null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            // Draw the selected tile
            // Layer 1
            if(pos.X != 0.00001f && activeTool == EditorTools.L1TilePlacer)
            spriteBatch.Draw(_selectedTileTexture, new Vector2((pos.X - pos.Y) * (tileSize.X / 2) - _selectedTileTexture.Width / 2,
                                                               (pos.X + pos.Y) * (tileSize.Y / 2)) +
                             worldOffset + L1TileTool.TileOffset,
                             null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            // Layer 2
            else if(pos.X != 0.00001f && activeTool == EditorTools.L2TilePlacer)
            spriteBatch.Draw(_selectedTileTexture, new Vector2((pos.X - pos.Y) * (tileSize.X / 2) - _selectedTileTexture.Width / 2,
                                                               (pos.X + pos.Y) * (tileSize.Y / 2)) +
                             worldOffset + L2TileTool.TileOffset,
                             null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        // Draws the grid for the editor
        public void DrawGrid(SpriteBatch spriteBatch, Vector2 windowSize)
        {
            gridVisibilityLower = new Vector2(windowSize.X / 2 - worldOffset.X,
                                             -worldOffset.Y - windowSize.Y / 2);
            gridVisibilityUpper = new Vector2(windowSize.X / 2 - worldOffset.X,
                                             windowSize.Y * 1.5f - worldOffset.Y);
            
            gridVisibilityLowerTransformed.X = (int)((gridVisibilityLower.X / (tileSize.X / 2) + gridVisibilityLower.Y / (tileSize.Y / 2)) / 2);
            gridVisibilityLowerTransformed.Y = (int)((gridVisibilityLower.Y / (tileSize.Y / 2) - gridVisibilityLower.X / (tileSize.X / 2)) / 2);

            gridVisibilityUpperTransformed.X = (int)((gridVisibilityUpper.X / (tileSize.X / 2) + gridVisibilityUpper.Y / (tileSize.Y / 2)) / 2);
            gridVisibilityUpperTransformed.Y = (int)((gridVisibilityUpper.Y / (tileSize.Y / 2) - gridVisibilityUpper.X / (tileSize.X / 2)) / 2);

            for (int x = (int)gridVisibilityLowerTransformed.X; x <= gridVisibilityUpperTransformed.X; x++)
            {
                for (int y = (int)gridVisibilityLowerTransformed.Y; y <= gridVisibilityUpperTransformed.Y; y++)
                {
                    if(x >= 0 && y >= 0)
                        spriteBatch.Draw(textureGridGray, new Vector2((x - y) * (tileSize.X / 2) + worldOffset.X - tileSize.X / 2, 
                                                                      (x + y) * (tileSize.Y / 2) + worldOffset.Y), 
                                        null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }

        // Method used to draw lines, takes in SpriteBatch, begining point and end point
        private void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, float angleDeg)
        {
            Vector2 line = point2 - point1;
            //float angle = (float)Math.Atan2(line.Y, line.X);    // Calculates angle
            float angleRad = (float)(angleDeg * Math.PI / 180f);    // Angle in raidans

            spriteBatch.Draw(textureGridGray,
                             new Rectangle((int)point1.X, (int)point1.Y, (int)line.Length(), 1),
                             null, Color.DarkGray, angleRad, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        // Generate a MapData object to write to file.
        public MapData GenerateMapData()
        {
            MapData mapData = new MapData(mapName, listL1Tiles, listL2Tiles);

            return mapData;
        }

        public Vector2 GetWorldOffset()
        {
            return worldOffset;
        }

        // GETTERS AND SETTERS =============================
        internal L1TileTool L1TileTool
        {
            get { return l1TileTool; }
            set { l1TileTool = value; }
        }
        internal L2TileTool L2TileTool
        {
            get { return l2TileTool; }
            set { l2TileTool = value; }
        }
        public bool ActiveToolMenuShow
        {
            get { return activeToolMenuShow; }
            set { activeToolMenuShow = value; }
        }
        internal EditorTools ActiveTool
        {
            get { return activeTool; }
            set { activeTool = value; }
        }
        internal EditorTools LastActiveTool
        {
            get { return lastActiveTool; }
            set { lastActiveTool = value; }
        }
        public List<Rectangle> IgnoredMenuAreas
        {
            get { return ignoredMenuAreas; }
            set { ignoredMenuAreas = value; }
        }
        public bool GridShow
        {
            get { return gridShow; }
            set { gridShow = value; }
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
        internal List<L1Tile> ListL1Tiles
        {
            get { return listL1Tiles; }
            set { listL1Tiles = value; }
        }
        internal List<L2Tile> ListL2Tiles
        {
            get { return listL2Tiles; }
            set { listL2Tiles = value; }
        }
        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }
    }
}
