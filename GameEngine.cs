using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Myra;
using Myra.Graphics2D.UI;
using System;
using TargetGame.Settings;

namespace TargetGame
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class GameEngine : Game
  {
    private static DeviceState deviceState;
    private static DeviceState prevDeviceState;
    private static GameState gameState;
    public static bool quit;
    public static Options options;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public GameEngine()
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
      MyraEnvironment.Game = this;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      IsMouseVisible = true;
      gameState = GameState.Splashscreen;
      base.Initialize();
      SplashScreen.Init();
      setState(GameState.Splashscreen);
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      Resources.LoadContent(Content);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      Content.Unload();
    }

    public static GameState getState()
    {
      return (gameState);
    }

    public static void setState(GameState state)
    {
      gameState = state;
      switch(state)
      {
        case GameState.Playing:
        case GameState.Tutorial:
          MediaPlayer.Stop();
          GameMain.hud.activate();
          break;
        case GameState.Menu:
          MediaPlayer.Play(Resources.menuTheme);
        break;
        case GameState.Splashscreen:
          SplashScreen.Trigger();
          break;
      }
    }

    public static DeviceState getDeviceState()
    {
      return (deviceState);
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (quit) Exit();
      deviceState = new DeviceState(Mouse.GetState(), Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));
      switch (gameState)
      {
        case GameState.Splashscreen:
          IsMouseVisible = true;
          SplashScreen.Update(gameTime, deviceState);
          break;
        case GameState.Menu:
          IsMouseVisible = true;
          break;
        case GameState.Playing:
        case GameState.Tutorial:
          IsMouseVisible = false;
          GameMain.Update(gameTime, deviceState, prevDeviceState);
          break;
      }
      prevDeviceState = deviceState;
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);
      spriteBatch.Begin();
      switch (gameState)
      {
        case GameState.Playing:
        case GameState.Tutorial:
          GameMain.Draw(graphics, spriteBatch);
          break;
      }
      spriteBatch.End();
      Desktop.Render();
      base.Draw(gameTime);
    }
  }
}
