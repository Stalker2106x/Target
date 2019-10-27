// Decompiled with JetBrains decompiler
// Type: Target.Game1
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Target
{
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
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
      this.graphics.IsFullScreen = false;
      this.graphics.PreferredBackBufferWidth = 1024;
      this.graphics.PreferredBackBufferHeight = 768;
      this.graphics.ApplyChanges();
      this.graphics.SynchronizeWithVerticalRetrace = false;
      this.IsFixedTimeStep = true;
      this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 90.0);
    }

    protected override void Initialize()
    {
      Game1.screenWidth = this.Window.ClientBounds.Width;
      Game1.screenHeight = this.Window.ClientBounds.Height;
      this.Window.SetPosition(new Point(50, 50));
      this.gameMain = new GameMain(ref this.graphics);
      this.gameMenu = new GameMenu();
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      Resources.LoadContent(this.Content);
    }

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
      return Game1.screenWidth / 2 - (int) vector2.X / 2;
    }

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
