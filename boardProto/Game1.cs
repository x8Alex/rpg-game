﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
//using Shooter;

namespace boardProto
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Vector2 virtualResolution = new Vector2(1366, 768);
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        Vector2 mouseWorldPosition;

        List<Texture2D> TEXTURE_LIST;
        Texture2D EMPTY_SPACE;      // Black texture for the background of the map editor
        Texture2D GRID_TEXTURE;     // Gray texture for the grid lines of the map editor
        Texture2D TILE_DIRT1;

        Player player;
        EditorManager editorManager;
        MouseManager mouseManager;
        KeyboardState kbState;
        KeyboardState kbStateOld;

        public Game1(Vector2 resolution)
        {
            virtualResolution = resolution;
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
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
            player = new Player();
            editorManager = new EditorManager();
            mouseManager = new MouseManager();
            kbState = Keyboard.GetState();
            kbStateOld = kbState;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures.
            EMPTY_SPACE = new Texture2D (graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            GRID_TEXTURE = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            EMPTY_SPACE.SetData<Color>(new Color[] { Color.Black });
            GRID_TEXTURE.SetData<Color>(new Color[] { Color.Gray });

            TEXTURE_LIST.Add(EMPTY_SPACE);
            TEXTURE_LIST.Add(GRID_TEXTURE);
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Panels/ToolMenuOpen"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Panels/ToolMenuClosed"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Tiles/Ground/TileDirt1x1"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Tiles/Ground/TileDirt2x2"));
            TEXTURE_LIST.Add(Content.Load<Texture2D>("Tiles/Ground/TileDirt4x4"));

            // Adds textures to a list that is passed to EditorManager


            // TODO: ================================================================
            // Load EditorManager


            // Load player resources
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\PlayerXS"), playerPosition);
            mouseManager.Initialize(virtualResolution);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            editorManager.ScrollWorld(mouseManager.GetMouseState(), mouseManager.GetMousePosition());

            kbState = Keyboard.GetState();
            // Toggle whether to display editor menus
            if (kbStateOld.IsKeyUp(Keys.Tab) && kbState.IsKeyDown(Keys.Tab))
                editorManager.ActiveToolMenuShow = !editorManager.ActiveToolMenuShow;
            // Toggle whether to display the grid
            if (kbStateOld.IsKeyUp(Keys.G) && kbState.IsKeyDown(Keys.G))
                editorManager.GridShow = !editorManager.GridShow;
            
            // Increase or decrease tile size
            // Works if a tile tool is active
            // Works if the texture has a "#x#" at the end of it
            if (kbStateOld.IsKeyUp(Keys.OemPlus) && kbState.IsKeyDown(Keys.OemPlus) &&
                editorManager.ActiveTool == EditorManager.EditorTools.L1TilePlacer)
            {
                if (editorManager.SelectedTileTexture.ToString().Contains("1x1") ||
                    editorManager.SelectedTileTexture.ToString().Contains("2x2") ||
                    editorManager.SelectedTileTexture.ToString().Contains("4x4"))
                {
                    editorManager.L1TileTool.TileSelection("BIGGER");   // Increase tile size
                }
            }
            else if (kbStateOld.IsKeyUp(Keys.OemMinus) && kbState.IsKeyDown(Keys.OemMinus))
            {
                if (editorManager.SelectedTileTexture.ToString().Contains("1x1") ||
                    editorManager.SelectedTileTexture.ToString().Contains("2x2") ||
                    editorManager.SelectedTileTexture.ToString().Contains("4x4"))
                {
                    editorManager.L1TileTool.TileSelection("SMALLER");  // Decrease tile size
                }
            }
            kbStateOld = kbState;
            
            // Places tiles when LMB is pressed
            if (editorManager.ActiveTool != EditorManager.EditorTools.None)
                editorManager.PlaceTiles(mouseManager.GetMouseState(), 
                                         editorManager.DetectClosestTilePosition(mouseWorldPosition, 
                                         editorManager.IgnoredMenuAreas));

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(transformMatrix: GetScaleMatrix());

            editorManager.DrawEmptySpace(spriteBatch, virtualResolution);   // Draws empty black background
            editorManager.DrawTiles(spriteBatch, editorManager.DetectClosestTilePosition(mouseWorldPosition), 
                                    editorManager.SelectedTileTexture);
            if (editorManager.GridShow)     // Draws the grid if GridShow is true
                editorManager.DrawGrid(spriteBatch, virtualResolution);

            player.Draw(spriteBatch, editorManager.GetWorldOffset());

            // Draws the active tool menus
            editorManager.DrawActivePanel(spriteBatch, editorManager.ActiveTool);

            spriteBatch.DrawString(debugFont, "World coords:", new Vector2(5, 1), Color.Yellow);
            spriteBatch.DrawString(debugFont, mouseWorldPosition.ToString(), new Vector2(80, 1), Color.Yellow);
            spriteBatch.DrawString(debugFont, 
                                   new Vector2 ((float)Math.Round(editorManager.DetectClosestTilePosition(mouseWorldPosition).X, 0),
                                                (float)Math.Round(editorManager.DetectClosestTilePosition(mouseWorldPosition).Y, 0)).ToString(),
                                   new Vector2(5, 11), Color.Yellow);
            spriteBatch.DrawString(debugFont, "Active tool: " + editorManager.ActiveTool.ToString(), new Vector2(180, 1), Color.LimeGreen);
            spriteBatch.DrawString(debugFont, "Selected tile: " + editorManager.SelectedTileTexture.ToString(), new Vector2(310, 1), Color.Magenta);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Matrix GetScaleMatrix()
        {
            var scaleX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / virtualResolution.X;
            var scaleY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / virtualResolution.Y;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}
