using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System;

namespace Target
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardState oldKeyboard;
        private MouseState oldMouse;
        private GamePadState oldGamePad;
        public static bool quit;
        public static GameState gameState;
        public static Options options;
        private Desktop _desktop;

        public Game1()
        {
            quit = false;
            gameState = GameState.Menu;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 90.0); //Set FPS
            _desktop = new Desktop();
            options = new Options(graphics, GraphicsAdapter.DefaultAdapter);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            MyraEnvironment.Game = this;
            gameState = GameState.Menu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Resources.LoadContent(Content);
            Menu.LoadContent(Content);
            Menu.MainMenu(_desktop);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }


        public static Texture2D createTexture2D(GraphicsDeviceManager graphics)
        {
            Texture2D texture2D = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture2D.SetData<Color>(new Color[1]
            {
                Color.White
            });
            return texture2D;
        }

        public static int centerX(SpriteFont spriteFont, string text)
        {
            Vector2 vector2 = spriteFont.MeasureString(text);
            return Options.Config.Width / 2 - (int)vector2.X / 2;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Game1.quit)
                Exit();
            switch (Game1.gameState)
            {
                case GameState.Menu:
                    IsMouseVisible = true;
                    break;
                case GameState.Playing:
                    IsMouseVisible = false;
                    GameMain.Update(gameTime, _desktop, Keyboard.GetState(), oldKeyboard, Mouse.GetState(), oldMouse, GamePad.GetState(PlayerIndex.One), oldGamePad);
                    break;
            }
            oldKeyboard = Keyboard.GetState();
            oldMouse = Mouse.GetState();
            oldGamePad = GamePad.GetState(PlayerIndex.One);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            spriteBatch.Begin();
            switch (Game1.gameState)
            {
                case GameState.Menu:
                    _desktop.Render();
                    break;
                case GameState.Playing:
                    GameMain.Draw(graphics, spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
