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
    class EditorManager
    {
        Vector2 worldOffset = new Vector2(0, 0);
        MouseState mouseState;
        Vector2 lastMousePosition;
        Vector2 MOUSE_POSITION;

        List<Texture2D> listAllTextures;
        List<Texture2D> listL1Textures;
        Texture2D textureEmptySpace;
        Texture2D textureGridGray;
        Texture2D DIRT_TILE1;

        L1TileTool l1TileTool;
        EditorTools activeTool;
        bool activeToolMenuShow = false;

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

        // Editor tools
        public enum EditorTools
        {
            None,
            L1TilePlacer
        };

        // Layers of the map
        List<L1Tile> listL1Tiles;   // X, Y, PASSABLE
        List<Vector3> layer2;
        List<Vector3> layer3;

        // Layer 1 tiles
        public enum L1TileType {
            Dirt1x1,
            Dirt2x2,
            Grass1x1,
            Grass2x2
        };

        public void Initialize(List<Texture2D> texture_list)
        {
            listAllTextures = texture_list;
            listL1Textures = new List<Texture2D>();
            listL1Tiles = new List<L1Tile>();
            textureEmptySpace = listAllTextures[0];
            textureGridGray = listAllTextures[1];
            listL1Textures.Add(listAllTextures[2]);   // Adds TileDirt1x1
            listL1Textures.Add(listAllTextures[3]);   // Adds TileDirt2x2

            activeTool = EditorTools.L1TilePlacer;      // Makes all panels inactive by default
            l1TileTool = new L1TileTool(listL1Textures);
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

        // Places tiles in the world
        public void PlaceTiles(MouseState _mouseState, Vector2 _tilePosition)
        {
            // Check which panel is active
            // ===========================================================================
            if (activeTool == EditorTools.L1TilePlacer)
            {
                // Open tile selection menu
                l1TileTool.TileSelectionMenuShow = true;
                activeToolMenuShow = l1TileTool.TileSelectionMenuShow;

                // Tile selection
                l1TileTool.TileSelection();

                Console.WriteLine("sdfs");

                // Tile placement
                if (_mouseState.LeftButton == ButtonState.Pressed)
                {
                    
                    // If tile is not occupied
                    if (!listL1Tiles.Exists(a => a.TilePosition == _tilePosition))
                    {
                        // Add a tile to Layer1Tiles with arguments "tile type", "tile position", "passable T/F", "texture".
                        listL1Tiles.Add(new L1Tile(L1TileType.Dirt1x1, _tilePosition, true, l1TileTool.SelectedTileTexture));
                    }
                    // If tile is occupied check if the tile being placed is the same as the one that already exists
                    else if (listL1Tiles[listL1Tiles.FindIndex(a => a.TilePosition == _tilePosition)].TileTexture !=
                                                                                     l1TileTool.SelectedTileTexture)
                    {
                        // Overwrites the tile to Layer1Tiles with arguments "tile type", "tile position", "passable T/F", "texture".
                        listL1Tiles[listL1Tiles.FindIndex(a => a.TilePosition == _tilePosition)] =
                                    new L1Tile(L1TileType.Dirt1x1, _tilePosition, true, l1TileTool.SelectedTileTexture);
                    }
                }
            }
        }

        // Detects which tile the mouse is hovered over and returns the
        // calculated coordinates of the left corner.
        public Vector2 DetectClosestTilePosition(Vector2 _mouseWorldPosition)
        {
            // ===================================================
            // ==   THIS CODE NEEDS TO BE CLEANED UP ONE DAY!   ==
            // ==   IT IS VERY MESSY!!!                         ==
            // ===================================================
            Vector2 MOUSE_POSITION = new Vector2(_mouseWorldPosition.X, _mouseWorldPosition.Y);
            double oddRowBottomBoundary = -0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) + 14.56;
            double oddRowTopBoundary = 0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) - 
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) - 14.56;

            double evenRowBottomBoundary = -0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) + 14.56;
            double evenRowTopBoundary = 0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) - 14.56;

            float calcYValue = (float)(80 * Math.Tan(20 * Math.PI / 180));

            // Checks for tiles at Y = 0
            if (Math.Abs(_mouseWorldPosition.Y) <= 14.56)    
            {
                if (_mouseWorldPosition.Y <= oddRowBottomBoundary
                    && _mouseWorldPosition.Y > oddRowTopBoundary)
                {
                    if (_mouseWorldPosition.X > 0)
                    {   // When X is positive
                        return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80), 0f);
                    }
                    else
                    {   // When X is negative
                        return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80), 0f);
                    }
                }
                 
                // EVEN ROWS NEGATIVE Y values:
                else if (_mouseWorldPosition.Y < 0 && Math.Abs(_mouseWorldPosition.Y) < 14.56)
                {
                    // If the mouse is in the RIGHT half of the tile
                    if (Math.Abs(_mouseWorldPosition.Y) > oddRowBottomBoundary && 
                        Math.Abs(_mouseWorldPosition.Y) - calcYValue <= oddRowTopBoundary)
                    {
                        // If the mouse is in the LEFT half of the tile AND X value is POSITIVE
                        if (_mouseWorldPosition.X >= 0 && (_mouseWorldPosition.X - ((int)(_mouseWorldPosition.X / 80) * 80) - 40) % 80 <= 0.5)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80) - 40, calcYValue / -2);
                        }
                        else if (_mouseWorldPosition.X > 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80) + 40, calcYValue / -2);
                        }
                        // If the mouse is in the LEFT half the tile AND X value is NEGATIVE
                        else if (_mouseWorldPosition.X < 0 && (Math.Abs(_mouseWorldPosition.X) - 
                                 ((int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) % 80 <= 0.5)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80) + 40, calcYValue / -2);
                        }
                        else if (_mouseWorldPosition.X < 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80) + 40, calcYValue / -2);
                        }
                    }
                    
                }

                // EVEN ROWS:
                else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                      && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                {
                    // If the mouse is in the LEFT half of the tile
                    if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                        Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                    {
                        // When X is positive
                        if (_mouseWorldPosition.X > 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                        }
                        // When X is negative
                        else
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                        }
                    }
                    // If the mouse is in the RIGHT half of the tile
                    else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                    {
                        return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                           (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                    }
                }
            }
            else
            {
                // When mouse Y value is POSITIVE
                if (_mouseWorldPosition.Y > 0)
                {
                    // ODD ROWS: When mouse Y value is POSITIVE and GREATER than the closest multiple of 29
                    if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        if (_mouseWorldPosition.X > 0)
                        {   // When X is positive
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                           (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                           (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                    } else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        // If the mouse is in the LEFT half of the tile
                        if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                            Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                        {
                            // When X is positive
                            if (_mouseWorldPosition.X > 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                            // When X is negative
                            else
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                        }
                        // If the mouse is in the RIGHT half of the tile
                        else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                        }
                    }

                    // ODD ROWS: When mouse Y value is POSITIVE and LESS than the closest multiple of 29
                    else if (((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) - _mouseWorldPosition.Y <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        if (_mouseWorldPosition.X > 0)
                        {   // When X is positive
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue);
                        }
                    } 
                    
                    // EVEN ROWS: Uses the same calculations as odd rows but reverses the comparison signs
                    else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        // If the mouse is in the LEFT half of the tile
                        if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                            Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                        {
                            // When X is positive
                            if (_mouseWorldPosition.X > 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                            // When X is negative
                            else
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                        }
                        // If the mouse is in the RIGHT half of the tile
                        else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                        }
                    }
                }

                // When mouse Y value is NEGATIVE ==========================================================
                else
                {
                    // ODD ROWS: When mouse Y value is NEGATIVE and GREATER than the closest multiple of 29
                    if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        if (_mouseWorldPosition.X > 0)
                        {   // When X is positive
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                           (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                           (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                    }
                    // ODD ROWS: When mouse Y value is NEGATIVE and LESS than the closest multiple of 29
                    else if (((int)(Math.Abs(_mouseWorldPosition.Y) / calcYValue + 1) * calcYValue) + _mouseWorldPosition.Y <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && ((int)(Math.Abs(_mouseWorldPosition.Y) / calcYValue + 1) * calcYValue) - _mouseWorldPosition.Y > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        if (_mouseWorldPosition.X > 0)
                        {   // When X is positive
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue - 1) * calcYValue);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue - 1) * calcYValue);
                        }
                    }

                    // EVEN ROWS: Uses the same calculations as odd rows but reverses the comparison signs
                    else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) < oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                        && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        Console.WriteLine(_mouseWorldPosition.ToString());
                        // If the mouse is in the LEFT half of the tile
                        if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                            Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                        {
                            // When X is positive
                            if (_mouseWorldPosition.X > 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue - calcYValue / 2);
                            }
                            // When X is negative
                            else
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue - calcYValue / 2);
                            }
                        }
                        // If the mouse is in the RIGHT half of the tile
                        else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue - calcYValue / 2);
                        }
                    }
                }
            }

            // Slope is 0.36375
            // Vertical gap between tiles is 29.1176
            return new Vector2(_mouseWorldPosition.X, _mouseWorldPosition.Y);
        }
        public Vector2 DetectClosestTilePosition(Vector2 _mouseWorldPosition, Rectangle ignoredArea)
        {
            // ===================================================
            // ==   THIS CODE NEEDS TO BE CLEANED UP ONE DAY!   ==
            // ==   IT IS VERY MESSY!!!                         ==
            // ===================================================
            Vector2 MOUSE_POSITION = new Vector2(_mouseWorldPosition.X, _mouseWorldPosition.Y);
            double oddRowBottomBoundary = -0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) + 14.56;
            double oddRowTopBoundary = 0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) - 14.56;

            double evenRowBottomBoundary = -0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) + 14.56;
            double evenRowTopBoundary = 0.36375 * Math.Abs((Math.Abs(_mouseWorldPosition.X) -
                                    (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) - 14.56;

            float calcYValue = (float)(80 * Math.Tan(20 * Math.PI / 180));

            // Checks if mouse is within the area to be ignored
            if (Mouse.GetState().X > ignoredArea.X && Mouse.GetState().X < ignoredArea.X + ignoredArea.Width &&
                Mouse.GetState().Y > ignoredArea.Y && Mouse.GetState().Y < ignoredArea.Y + ignoredArea.Height)
            {
                return new Vector2();
            }
            else
            {
                // Checks for tiles at Y = 0
                if (Math.Abs(_mouseWorldPosition.Y) <= 14.56)
                {
                    if (_mouseWorldPosition.Y <= oddRowBottomBoundary
                        && _mouseWorldPosition.Y > oddRowTopBoundary)
                    {
                        if (_mouseWorldPosition.X > 0)
                        {   // When X is positive
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80), 0f);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80), 0f);
                        }
                    }

                    // EVEN ROWS NEGATIVE Y values:
                    else if (_mouseWorldPosition.Y < 0 && Math.Abs(_mouseWorldPosition.Y) < 14.56)
                    {
                        // If the mouse is in the RIGHT half of the tile
                        if (Math.Abs(_mouseWorldPosition.Y) > oddRowBottomBoundary &&
                            Math.Abs(_mouseWorldPosition.Y) - calcYValue <= oddRowTopBoundary)
                        {
                            // If the mouse is in the LEFT half of the tile AND X value is POSITIVE
                            if (_mouseWorldPosition.X >= 0 && (_mouseWorldPosition.X - ((int)(_mouseWorldPosition.X / 80) * 80) - 40) % 80 <= 0.5)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80) - 40, calcYValue / -2);
                            }
                            else if (_mouseWorldPosition.X > 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80) + 40, calcYValue / -2);
                            }
                            // If the mouse is in the LEFT half the tile AND X value is NEGATIVE
                            else if (_mouseWorldPosition.X < 0 && (Math.Abs(_mouseWorldPosition.X) -
                                     ((int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80) - 40) % 80 <= 0.5)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80) + 40, calcYValue / -2);
                            }
                            else if (_mouseWorldPosition.X < 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80) + 40, calcYValue / -2);
                            }
                        }

                    }

                    // EVEN ROWS:
                    else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                          && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                    {
                        // If the mouse is in the LEFT half of the tile
                        if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                            Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                        {
                            // When X is positive
                            if (_mouseWorldPosition.X > 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                            // When X is negative
                            else
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                        }
                        // If the mouse is in the RIGHT half of the tile
                        else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                        {
                            return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                        }
                    }
                }
                else
                {
                    // When mouse Y value is POSITIVE
                    if (_mouseWorldPosition.Y > 0)
                    {
                        // ODD ROWS: When mouse Y value is POSITIVE and GREATER than the closest multiple of 29
                        if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                            && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            if (_mouseWorldPosition.X > 0)
                            {   // When X is positive
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                            }
                            else
                            {   // When X is negative
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                            }
                        }
                        else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                          && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            // If the mouse is in the LEFT half of the tile
                            if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                                Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                            {
                                // When X is positive
                                if (_mouseWorldPosition.X > 0)
                                {
                                    return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                       (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                                }
                                // When X is negative
                                else
                                {
                                    return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                       (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                                }
                            }
                            // If the mouse is in the RIGHT half of the tile
                            else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                        }

                      // ODD ROWS: When mouse Y value is POSITIVE and LESS than the closest multiple of 29
                        else if (((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) - _mouseWorldPosition.Y <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                            && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            if (_mouseWorldPosition.X > 0)
                            {   // When X is positive
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                                   (int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue);
                            }
                            else
                            {   // When X is negative
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                                   (int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue);
                            }
                        }

                        // EVEN ROWS: Uses the same calculations as odd rows but reverses the comparison signs
                        else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                            && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            // If the mouse is in the LEFT half of the tile
                            if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                                Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                            {
                                // When X is positive
                                if (_mouseWorldPosition.X > 0)
                                {
                                    return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                       (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                                }
                                // When X is negative
                                else
                                {
                                    return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                       (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                                }
                            }
                            // If the mouse is in the RIGHT half of the tile
                            else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue + calcYValue / 2);
                            }
                        }
                    }

                    // When mouse Y value is NEGATIVE ==========================================================
                    else
                    {
                        // ODD ROWS: When mouse Y value is NEGATIVE and GREATER than the closest multiple of 29
                        if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                            && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            if (_mouseWorldPosition.X > 0)
                            {   // When X is positive
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                            }
                            else
                            {   // When X is negative
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                               (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue);
                            }
                        }
                        // ODD ROWS: When mouse Y value is NEGATIVE and LESS than the closest multiple of 29
                        else if (((int)(Math.Abs(_mouseWorldPosition.Y) / calcYValue + 1) * calcYValue) + _mouseWorldPosition.Y <= oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                            && ((int)(Math.Abs(_mouseWorldPosition.Y) / calcYValue + 1) * calcYValue) - _mouseWorldPosition.Y > oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            if (_mouseWorldPosition.X > 0)
                            {   // When X is positive
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80),
                                                   (int)(_mouseWorldPosition.Y / calcYValue - 1) * calcYValue);
                            }
                            else
                            {   // When X is negative
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 1) * 80),
                                                   (int)(_mouseWorldPosition.Y / calcYValue - 1) * calcYValue);
                            }
                        }

                        // EVEN ROWS: Uses the same calculations as odd rows but reverses the comparison signs
                        else if (_mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue) * calcYValue) < oddRowBottomBoundary    // Bottom boundary y value should be greater than mouse y value
                            && _mouseWorldPosition.Y - ((int)(_mouseWorldPosition.Y / calcYValue + 1) * calcYValue) <= oddRowTopBoundary)    // Top boundary y value should be smaller than mouse y value
                        {
                            Console.WriteLine(_mouseWorldPosition.ToString());
                            // If the mouse is in the LEFT half of the tile
                            if (Math.Abs(_mouseWorldPosition.X) >= (int)(Math.Abs(_mouseWorldPosition.X) / 80) * 80 + 40 &&
                                Math.Abs(_mouseWorldPosition.X) < (int)(Math.Abs(_mouseWorldPosition.X) / 80 + 1) * 80 + 40)
                            {
                                // When X is positive
                                if (_mouseWorldPosition.X > 0)
                                {
                                    return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 + 40),
                                                       (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue - calcYValue / 2);
                                }
                                // When X is negative
                                else
                                {
                                    return new Vector2((float)((int)(_mouseWorldPosition.X / 80 - 2) * 80 + 40),
                                                       (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue - calcYValue / 2);
                                }
                            }
                            // If the mouse is in the RIGHT half of the tile
                            else if (Math.Abs(_mouseWorldPosition.X) % 80 >= 0)
                            {
                                return new Vector2((float)((int)(_mouseWorldPosition.X / 80) * 80 - 40),
                                                   (int)(_mouseWorldPosition.Y / calcYValue) * calcYValue - calcYValue / 2);
                            }
                        }
                    }
                }

                // Slope is 0.36375
                // Vertical gap between tiles is 29.1176
                return new Vector2(_mouseWorldPosition.X, _mouseWorldPosition.Y);
            }
        }

        // Draws editor panels
        public void DrawActivePanel(SpriteBatch spriteBatch, Rectangle frame)
        {
            spriteBatch.Draw(textureEmptySpace, frame, Color.White);

            for (int i = 0; i < l1TileTool.ListTileTextures.Count(); i++ )
                spriteBatch.Draw(listL1Textures[i], new Vector2(10 + 90 * i, 50 - listL1Textures[i].Height / 2),
                                 null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        // Draws a black background
        public void DrawEmptySpace(SpriteBatch spriteBatch, Vector2 windowSize){
            spriteBatch.Draw(textureEmptySpace, new Rectangle(0, 0, (int)windowSize.X, (int)windowSize.Y), Color.White);

        }

        // Draws the tiles
        public void DrawTiles(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(listL1Textures[0], new Vector2(pos.X, pos.Y - listL1Textures[0].Height / 2) + worldOffset, 
                             null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            foreach (var _tile in listL1Tiles)
            {
                spriteBatch.Draw(_tile.TileTexture, new Vector2(_tile.TilePosition.X,
                                                                _tile.TilePosition.Y - _tile.TileTexture.Height / 2) + worldOffset,
                             null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }


        // Draws the grid for the editor
        public void DrawGrid(SpriteBatch spriteBatch, Vector2 windowSize)
        {
            for (int x = (int)(-0.5 * (int)windowSize.X); x <= (int)windowSize.X / 80; x++)
                DrawLine(spriteBatch, new Vector2(x * 80 + worldOffset.X % 80, -29.1176f + worldOffset.Y % 29.1176f),
                                                  windowSize, 20f);

            for (int x = 0; x <= (int)windowSize.X * 3 / 80; x++)
                DrawLine(spriteBatch, new Vector2(x * 80 + worldOffset.X % 80, -29.1176f + worldOffset.Y % 29.1176f),
                                                  new Vector2(0, windowSize.Y), 160f);
        }

        // Method used to draw lines, takes in SpriteBatch, begining point and end point.
        private void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, float angleDeg)
        {
            Vector2 line = point2 - point1;
            //float angle = (float)Math.Atan2(line.Y, line.X);    // Calculates angle
            float angleRad = (float)(angleDeg * Math.PI / 180f);    // Angle in raidans

            spriteBatch.Draw(textureGridGray,
                             new Rectangle((int)point1.X, (int)point1.Y, (int)line.Length(), 1),
                             null, Color.DarkGray, angleRad, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public Vector2 GetWorldOffset()
        {
            return worldOffset;
        }
    }
}
