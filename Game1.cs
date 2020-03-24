using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private static GameState gameState;
        public static Options options;
        private Desktop _menuUI;

        public Game1()
        {
            quit = false;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            options = new Options(graphics, GraphicsAdapter.DefaultAdapter);
            graphics.IsFullScreen = Options.Config.Fullscreen;
            graphics.PreferredBackBufferWidth = Options.Config.Width;
            graphics.PreferredBackBufferHeight = Options.Config.Height;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 90.0); //Set FPS
            _menuUI = new Desktop();
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
            setState(GameState.Menu);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Resources.LoadContent(Content);
            Menu.MainMenu(_menuUI);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        public static void setState(GameState state)
        {
            gameState = state;
            if (state == GameState.Menu) MediaPlayer.Play(Resources.menuTheme);
            else MediaPlayer.Stop();
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
                    GameMain.Update(gameTime, _menuUI, Keyboard.GetState(), oldKeyboard, Mouse.GetState(), oldMouse, GamePad.GetState(PlayerIndex.One), oldGamePad);
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
                    _menuUI.Render();
                    break;
                case GameState.Playing:
                    GameMain.Draw(graphics, spriteBatch);
                    break;
            }
            spriteBatch.End();
            PostDraw();
            base.Draw(gameTime);
        }
        protected void PostDraw()
        {
          switch (Game1.gameState)
          {
            case GameState.Menu:
              _menuUI.Render();
              break;
            case GameState.Playing:
              GameMain.PostDraw();
              break;
          }
        }
  }
}
