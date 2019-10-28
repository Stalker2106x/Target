using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public static int screenWidth;
        public static int screenHeight;
        public static bool quit;
        public static GameState gameState;
        private GameMain gameMain;
        private GameMenu gameMenu;

        public Game1()
        {
            Game1.quit = false;
            Game1.gameState = GameState.Menu;
            this.graphics = new GraphicsDeviceManager((Game)this);
            this.Content.RootDirectory = "Content";
            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
            this.graphics.ApplyChanges();
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 90.0);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Game1.screenWidth = this.Window.ClientBounds.Width;
            Game1.screenHeight = this.Window.ClientBounds.Height;
            this.gameMain = new GameMain(ref this.graphics);
            this.gameMenu = new GameMenu();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            Resources.LoadContent(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.Content.Unload();
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
            return Game1.screenWidth / 2 - (int)vector2.X / 2;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Game1.quit)
                this.Exit();
            if (GameMain.gameQuit)
            {
                Game1.gameState = GameState.Menu;
                this.gameMenu.resetGame(false);
                this.gameMain = new GameMain(ref this.graphics);
            }
            switch (Game1.gameState)
            {
                case GameState.Menu:
                    this.IsMouseVisible = true;
                    this.gameMenu.Update(gameTime, this.graphics, Keyboard.GetState(), this.oldKeyboard, Mouse.GetState(), GamePad.GetState(PlayerIndex.One), this.oldGamePad);
                    break;
                case GameState.Playing:
                    this.IsMouseVisible = false;
                    this.gameMain.Update(gameTime, Keyboard.GetState(), this.oldKeyboard, Mouse.GetState(), this.oldMouse, GamePad.GetState(PlayerIndex.One), this.oldGamePad);
                    break;
            }
            this.oldKeyboard = Keyboard.GetState();
            this.oldMouse = Mouse.GetState();
            this.oldGamePad = GamePad.GetState(PlayerIndex.One);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.DarkSlateGray);
            this.spriteBatch.Begin();
            switch (Game1.gameState)
            {
                case GameState.Menu:
                    if (this.gameMenu.getActivity())
                        this.gameMain.Draw(this.spriteBatch);
                    this.gameMenu.Draw(this.spriteBatch);
                    break;
                case GameState.Playing:
                    this.gameMain.Draw(this.spriteBatch);
                    break;
            }
            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
