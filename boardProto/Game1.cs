using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace boardProto
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        bool startedAsFullScreen;
        GraphicsDeviceManager graphics;
        Vector2 virtualResolution;
        RenderTarget2D scene;
        Texture2D renderTargetTexture;
        SpriteBatch spriteBatch;
        DirectoryInfo tileDirectory;
        string[] tileSubfolder;
        Vector2 borderSize;
        SpriteFont debugFont;
        Vector2 mouseWorldPosition;
        List<Texture2D> TEXTURE_LIST;
        Texture2D EMPTY_SPACE;          // Black texture for the background of the map editor

        bool showMainMenu;              // Show menu T/F
        SaveMap saveMapQuery;           // Query form to save the map
        private MapData generatedMapData;
        MainMenuManager mainMenuManager;
        Player player;
        Vector2 playerPosition;
        EditorManager editorManager;
        MouseManager mouseManager;
        KeyboardState kbState;
        KeyboardState kbStateOld;

        public Game1(Vector2 resolution, bool _screenMode, Vector2 _borderSize)
        {
            showMainMenu = true;

            borderSize = _borderSize;
            virtualResolution = resolution;
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = _screenMode;
            startedAsFullScreen = _screenMode;
            IntPtr hWnd = this.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            var form = control.FindForm();
            form.Location = new System.Drawing.Point(0, 0);
            
            graphics.PreferredBackBufferWidth = (int)virtualResolution.X;
            graphics.PreferredBackBufferHeight = (int)virtualResolution.Y;

            this.IsMouseVisible = true;
            //graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            tileSubfolder = new string[] { "Ground/", "Walls/"};
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TEXTURE_LIST = new List<Texture2D>();

            // Initializes the managers for the main menu
            mainMenuManager = new MainMenuManager();

            player = new Player();
            editorManager = new EditorManager();
            mouseManager = new MouseManager();
            kbState = Keyboard.GetState();
            kbStateOld = kbState;

            // Render to target
            scene = new RenderTarget2D(graphics.GraphicsDevice, (int)(virtualResolution.X), (int)(virtualResolution.Y),
                                       false, SurfaceFormat.Color, DepthFormat.None,
                                       graphics.GraphicsDevice.PresentationParameters.MultiSampleCount,
                                       RenderTargetUsage.DiscardContents);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Creates the fonts
            debugFont = Content.Load<SpriteFont>("DebugFont");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            // Loads textures
            tileDirectory = new DirectoryInfo(Content.RootDirectory + "/Tiles/" + tileSubfolder[0]);
            FileInfo[] contentFilesGround = tileDirectory.GetFiles("*.*");

            EMPTY_SPACE = new Texture2D (graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            EMPTY_SPACE.SetData<Color>(new Color[] { Color.Black });

            TEXTURE_LIST.Add(EMPTY_SPACE);
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Tiles/GridTile"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Panels/ToolMenuOpen"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Panels/ToolMenuClosed"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Panels/Buttons/ButtonTileOFF"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Panels/Buttons/ButtonTileON"));

            // Adds textures from Tiles/Ground to list
            foreach (FileInfo _contentFile in contentFilesGround)
            {
                TEXTURE_LIST.Add(Content.Load<Texture2D>("Tiles/Ground/" + _contentFile.Name.Split('.')[0]));
            }

            // Adds textures from Tiles/Walls to list
            tileDirectory = new DirectoryInfo(Content.RootDirectory + "/Tiles/" + tileSubfolder[1]);
            FileInfo[] contentFilesWalls = tileDirectory.GetFiles("*.*");

            foreach (FileInfo _contentFile in contentFilesWalls)
            {
                TEXTURE_LIST.Add(Content.Load<Texture2D>("Tiles/Walls/" + _contentFile.Name.Split('.')[0]));
            }

            // Initialize mainMenuManager
            if (EMPTY_SPACE == null)
                Console.WriteLine("button texture is null");
            mainMenuManager.Initialize(graphics.GraphicsDevice, EMPTY_SPACE, debugFont, TEXTURE_LIST);

            // Load player resources
            playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\PlayerXS"), playerPosition);

            if(graphics.IsFullScreen)
                mouseManager.Initialize(virtualResolution, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                                                                       GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            else
                mouseManager.Initialize(virtualResolution, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - borderSize.X * 2,
                                                                       GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - borderSize.Y - borderSize.X));

            editorManager.Initialize(TEXTURE_LIST);  // Will later pass a list of textures to the manager
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F12) ||
                mainMenuManager.ExitGame)
                Exit();
            // TODO: Add your update logic here

            // Gets any keys that are pressed
            kbState = Keyboard.GetState();

            // Toggle main menu
            if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                showMainMenu = !showMainMenu;

            if (!showMainMenu)
            {
                editorManager.ScrollWorld(mouseManager.GetMouseState(), mouseManager.GetMousePosition());

                // Select Layer 1 Tile Tool
                if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F1) &&
                    kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1) &&
                    editorManager.ActiveTool != EditorManager.EditorTools.L1TilePlacer)
                {
                    editorManager.LastActiveTool = editorManager.ActiveTool;
                    editorManager.ActiveTool = EditorManager.EditorTools.L1TilePlacer;
                }
                // Select Layer 2 Tile Tool
                if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F2) &&
                    kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2) &&
                    editorManager.ActiveTool != EditorManager.EditorTools.L2TilePlacer)
                {
                    editorManager.LastActiveTool = editorManager.ActiveTool;
                    editorManager.ActiveTool = EditorManager.EditorTools.L2TilePlacer;
                }

                // Toggle whether to display editor menus
                if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Tab) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab))
                    editorManager.ActiveToolMenuShow = !editorManager.ActiveToolMenuShow;

                // Toggle whether to display the grid
                if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.G) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G))
                    editorManager.GridShow = !editorManager.GridShow;

                // Increase or decrease tile size
                TileSizeControl(kbStateOld, kbState);

                // Tile selection
                TileSelection(mouseManager.GetMouseState(), mouseManager.GetMousePosition());

                // Toggle tile delete tool
                if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Back) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                    if (editorManager.ActiveTool == EditorManager.EditorTools.TileDelete)
                        editorManager.ActiveTool = editorManager.LastActiveTool;
                    else
                    {
                        editorManager.LastActiveTool = editorManager.ActiveTool;
                        editorManager.ActiveTool = EditorManager.EditorTools.TileDelete;
                    }

                // Toggle tile mover tool
                if (kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Insert) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Insert))
                    if (editorManager.ActiveTool == EditorManager.EditorTools.TileMover)
                        editorManager.ActiveTool = editorManager.LastActiveTool;
                    else
                    {
                        editorManager.LastActiveTool = editorManager.ActiveTool;
                        editorManager.ActiveTool = EditorManager.EditorTools.TileMover;
                    }

                /*
                // Organizes the tile lists
                editorManager.ListL1Tiles = new List<L1Tile>(editorManager.ListL1Tiles.OrderBy(a => a.TilePosition.Y));
                editorManager.ListL2Tiles = new List<L2Tile>(
                                            editorManager.ListL2Tiles
                                            .OrderBy(a => a.TilePosition.Y)
                                            .ThenBy(a => a.TilePosition.X)
                                            );*/

                // Places tiles when LMB is pressed
                if (editorManager.ActiveTool != EditorManager.EditorTools.None)
                    editorManager.EditTiles(kbState, mouseManager.GetMouseState(), mouseManager.OldMouseState,
                                            editorManager.DetectClosestTilePosition(mouseWorldPosition,
                                                                                    editorManager.IgnoredMenuAreas));
            }
            else        // Main menu logic ============================================
            {
                mainMenuManager.MenuClickDetector(mouseManager.GetMouseState());
                if (mainMenuManager.NewMap)
                    NewMap();                               // If newMap == true, starts a new map, otherwise does nothing

                // Loads map
                if (mainMenuManager.LoadMapData)
                {
                    mainMenuManager.LoadCalled = true;
                    mainMenuManager.MapLoaderInit();
                    mainMenuManager.MapLoader.Show();
                    mainMenuManager.LoadMapData = false;
                }
                // Passes map data when window is closed
                if (!mainMenuManager.MapLoader.Visible && mainMenuManager.LoadCalled)
                {
                    editorManager.PassMapData(mainMenuManager.MapLoader.MapData);
                    mainMenuManager.LoadCalled = false;
                }

                // Saves map
                if (mainMenuManager.RetrieveMapData)
                {
                    // Calles SaveFile method and passes it the generated map data.
                    generatedMapData = editorManager.GenerateMapData();
                    saveMapQuery = new SaveMap(generatedMapData);
                    saveMapQuery.ChangeNameTextBox(editorManager.MapName);
                    graphics.IsFullScreen = false;
                    saveMapQuery.Show();
                    editorManager.MapName = saveMapQuery.MapName;
                    mainMenuManager.RetrieveMapData = false;
                }

                if (saveMapQuery != null)
                {
                    try
                    {
                        if (editorManager.MapName != saveMapQuery.MapName)
                            editorManager.MapName = saveMapQuery.MapName;
                    }
                    catch
                    {
                        Console.WriteLine("Shits acting up");
                    }
                }
            }

            mouseManager.OldMouseState = mouseManager.MouseState;
            kbStateOld = kbState;

            // Updates mouse world position
            mouseWorldPosition.X = mouseManager.GetMousePosition().X - editorManager.GetWorldOffset().X;
            mouseWorldPosition.Y = mouseManager.GetMousePosition().Y - editorManager.GetWorldOffset().Y;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            graphics.GraphicsDevice.SetRenderTarget(scene);
            graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                                BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                DepthStencilState.None,
                                RasterizerState.CullNone);
            
            // TODO: Add your drawing code here
            if (!showMainMenu)
            {
                editorManager.DrawEmptySpace(spriteBatch, virtualResolution);   // Draws empty black background
                editorManager.DrawTiles(spriteBatch, editorManager.DetectClosestTilePosition(mouseWorldPosition, editorManager.IgnoredMenuAreas),
                                        editorManager.SelectedTileTexture);

                // Draws the grid if GridShow is true
                if (editorManager.GridShow)
                    editorManager.DrawGrid(spriteBatch, virtualResolution);

                player.Draw(spriteBatch, editorManager.GetWorldOffset());

                // Draws the active tool menus and buttons
                editorManager.DrawActivePanel(spriteBatch, editorManager.ActiveTool);

                spriteBatch.DrawString(debugFont, "World coords:", new Vector2(5, 1), Color.Yellow);
                spriteBatch.DrawString(debugFont, mouseWorldPosition.ToString(), new Vector2(80, 1), Color.Yellow);

                // Tile position on the map
                spriteBatch.DrawString(debugFont, editorManager.DetectClosestTilePosition(mouseWorldPosition, editorManager.IgnoredMenuAreas).ToString(),
                                       new Vector2(5, 11), Color.Yellow);

                spriteBatch.DrawString(debugFont, "Active tool: " + editorManager.ActiveTool.ToString(), new Vector2(180, 1), Color.LimeGreen);
                try
                {
                    spriteBatch.DrawString(debugFont, "Selected tile: " + editorManager.SelectedTileTexture.ToString(), new Vector2(310, 1), Color.Magenta);
                }
                catch 
                {
                    Console.WriteLine("SelectedTileTexture is null.");
                }
            }
            else        // Main menu drawing
            {
                // Main menu buttons
                mainMenuManager.Draw(spriteBatch, debugFont);
            }

            spriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);

            renderTargetTexture = (Texture2D)scene;
            spriteBatch.Begin();
            spriteBatch.Draw(renderTargetTexture, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0),
                             1f, SpriteEffects.None, 1);
            spriteBatch.End();
            
            //spriteBatch.Draw(scene, Vector2.Zero);
            
            base.Draw(gameTime);
        }

        public Matrix GetScaleMatrix()
        {
            var scaleX = virtualResolution.X / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var scaleY = virtualResolution.Y / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        public void TileSizeControl(KeyboardState _kbStateOld, KeyboardState _kbState)
        {
            // Works if a tile tool is active
            // Works if the texture has a "#x#" at the end of it
            if (_kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.OemPlus) && 
                _kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemPlus) &&
                editorManager.ActiveTool != EditorManager.EditorTools.None &&
                editorManager.ActiveTool != EditorManager.EditorTools.TileDelete)
            {
                    switch (editorManager.ActiveTool) 
                    { 
                        case EditorManager.EditorTools.L1TilePlacer:
                            editorManager.L1TileTool.TileSelection("BIGGER");   // Increase tile size
                            break;
                        case EditorManager.EditorTools.L2TilePlacer:
                            editorManager.L2TileTool.TileSelection("BIGGER");   // Increase tile size
                            break;
                    }
            }
            else if (_kbStateOld.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.OemMinus) &&
                     _kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemMinus) &&
                     editorManager.ActiveTool != EditorManager.EditorTools.None &&
                     editorManager.ActiveTool != EditorManager.EditorTools.TileDelete)
            {
                    switch (editorManager.ActiveTool)
                    {
                        case EditorManager.EditorTools.L1TilePlacer:
                            editorManager.L1TileTool.TileSelection("SMALLER");   // Decrease tile size
                            break;
                        case EditorManager.EditorTools.L2TilePlacer:
                            editorManager.L2TileTool.TileSelection("SMALLER");   // Decrease tile size
                            break;
                    }
            }
        }

        public void TileSelection(MouseState _mouseState, Vector2 _mousePosition)
        {
            if (editorManager.ActiveTool == EditorManager.EditorTools.L1TilePlacer)
            {
                editorManager.L1TileTool.TileSelection(_mouseState, _mousePosition);
            }
            else if (editorManager.ActiveTool == EditorManager.EditorTools.L2TilePlacer)
            {
                editorManager.L2TileTool.TileSelection(_mouseState, _mousePosition);
            }
        }

        public void NewMap()
        {
            Console.WriteLine("NewMap() called.");
            player = new Player();
            editorManager = new EditorManager();
            mouseManager = new MouseManager();

            playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\PlayerXS"), playerPosition);

            if (graphics.IsFullScreen)
                mouseManager.Initialize(virtualResolution, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                                                                       GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            else
                mouseManager.Initialize(virtualResolution, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - borderSize.X * 2,
                                                                       GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - borderSize.Y - borderSize.X));
            editorManager.Initialize(TEXTURE_LIST);

            showMainMenu = false;
            mainMenuManager.NewMap = false;
        }

        internal MapData GeneratedMapData
        {
            get { return generatedMapData; }
            set { generatedMapData = value; }
        }
    }
}
