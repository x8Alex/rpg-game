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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
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
            TILE_DIRT1 = Content.Load<Texture2D>("Tiles/Ground/TileDirt1");

            EMPTY_SPACE.SetData<Color>(new Color[] { Color.Black });
            GRID_TEXTURE.SetData<Color>(new Color[] { Color.Gray });

            TEXTURE_LIST.Add(EMPTY_SPACE);
            TEXTURE_LIST.Add(GRID_TEXTURE);
            TEXTURE_LIST.Add(TILE_DIRT1);

            // Adds textures to a list that is passed to EditorManager


            // TODO: ================================================================
            // Load EditorManager


            // Load player resources
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\PlayerXS"), playerPosition);
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
            editorManager.ScrollWorld(mouseManager.GetMouseState());
            mouseWorldPosition.X = mouseManager.GetMouseState().X - editorManager.GetWorldOffset().X;
            mouseWorldPosition.Y = mouseManager.GetMouseState().Y - editorManager.GetWorldOffset().Y;
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
            spriteBatch.Begin();

            editorManager.DrawEmptySpace(spriteBatch, graphics.GraphicsDevice.Viewport);
            editorManager.DrawTiles(spriteBatch, editorManager.DetectClosestTilePosition(mouseWorldPosition));
            editorManager.DrawGrid(spriteBatch, graphics.GraphicsDevice.Viewport);

            player.Draw(spriteBatch,editorManager.GetWorldOffset());

            spriteBatch.DrawString(debugFont, "World coords:", new Vector2(5, 5), Color.Yellow);
            spriteBatch.DrawString(debugFont, mouseWorldPosition.ToString(), new Vector2(80, 5), Color.Yellow);
            spriteBatch.DrawString(debugFont, 
                                   editorManager.DetectClosestTilePosition(mouseWorldPosition).ToString(),
                                   new Vector2(5, 15), Color.Yellow);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
