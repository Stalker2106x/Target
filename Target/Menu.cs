// Decompiled with JetBrains decompiler
// Type: Target.Menu
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Target
{
  internal class Menu
  {
    private MenuState m_menuState;
    private int m_selectedOption;
    private int m_oldOption;
    private string m_title;
    private int m_nbOptions;
    private List<string> m_options;
    private List<Color> m_optionsColors;
    private bool drawGamePadKeys;
    private bool drawKeyboardKeys;

    public Menu(MenuState menuState, string title, int nbOptions, List<string> options)
    {
      this.drawGamePadKeys = false;
      this.drawKeyboardKeys = false;
      this.m_menuState = menuState;
      this.m_nbOptions = nbOptions;
      this.m_selectedOption = 0;
      this.m_oldOption = nbOptions - 1;
      this.m_title = title;
      this.m_options = options;
      this.m_optionsColors = new List<Color>();
      for (int index = 1; index <= nbOptions; ++index)
        this.m_optionsColors.Add(Color.DarkGray);
    }

    public void setOption(int index, string option)
    {
      this.m_options[index] = option;
    }

    public void select(
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (this.m_selectedOption < 0)
        this.m_selectedOption = this.m_nbOptions - 1;
      else if (this.m_selectedOption > this.m_nbOptions - 1)
        this.m_selectedOption = 0;
      else if (keyboard.IsKeyDown(Keys.Up) && oldKeyboard.IsKeyUp(Keys.Up) || (double) gamePad.ThumbSticks.Left.Y > 0.0 && (double) oldGamePad.ThumbSticks.Left.Y == 0.0)
      {
        Resources.menuClick.Play();
        --this.m_selectedOption;
      }
      else
      {
        if ((!keyboard.IsKeyDown(Keys.Down) || !oldKeyboard.IsKeyUp(Keys.Down)) && ((double) gamePad.ThumbSticks.Left.Y >= 0.0 || (double) oldGamePad.ThumbSticks.Left.Y != 0.0))
          return;
        Resources.menuClick.Play();
        ++this.m_selectedOption;
      }
    }

    public void updateColors()
    {
      if (this.m_selectedOption < 0 || this.m_selectedOption > this.m_nbOptions - 1)
        return;
      if (this.m_oldOption >= 0 && this.m_oldOption <= this.m_nbOptions - 1)
        this.m_optionsColors[this.m_oldOption] = Color.Gray;
      else if (this.m_oldOption < 0)
        this.m_optionsColors[0] = Color.Gray;
      else
        this.m_optionsColors[this.m_nbOptions - 1] = Color.Gray;
      this.m_optionsColors[this.m_selectedOption] = Color.AntiqueWhite;
    }

    public void checkSelect(
      ref MenuState currentMenu,
      ref bool activeGame,
      GraphicsDeviceManager graphics,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if ((!keyboard.IsKeyDown(Keys.Enter) || !oldKeyboard.IsKeyUp(Keys.Enter)) && (!gamePad.IsButtonDown(Buttons.A) || !oldGamePad.IsButtonUp(Buttons.A)))
        return;
      if (this.m_menuState == MenuState.Main)
      {
        switch (this.m_selectedOption)
        {
          case 0:
            activeGame = true;
            currentMenu = MenuState.Pause;
            Game1.gameState = GameState.Playing;
            break;
          case 1:
            currentMenu = MenuState.Options;
            break;
          case 2:
            Game1.quit = true;
            break;
        }
      }
      else if (this.m_menuState == MenuState.Pause)
      {
        switch (this.m_selectedOption)
        {
          case 0:
            Game1.gameState = GameState.Playing;
            break;
          case 1:
            currentMenu = MenuState.Options;
            break;
          case 2:
            activeGame = false;
            currentMenu = MenuState.Main;
            GameMain.gameQuit = true;
            break;
          case 3:
            Game1.quit = true;
            break;
        }
      }
      else if (this.m_menuState == MenuState.Options)
      {
        switch (this.m_selectedOption)
        {
          case 0:
            currentMenu = MenuState.InfoKeys;
            break;
          case 1:
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
            break;
          case 3:
            if (activeGame)
            {
              currentMenu = MenuState.Pause;
              break;
            }
            currentMenu = MenuState.Main;
            break;
        }
      }
      else
      {
        if (this.m_menuState != MenuState.InfoKeys)
          return;
        switch (this.m_selectedOption)
        {
          case 0:
            if (!this.drawKeyboardKeys)
            {
              this.drawGamePadKeys = false;
              this.drawKeyboardKeys = true;
              break;
            }
            this.drawKeyboardKeys = false;
            break;
          case 1:
            if (!this.drawGamePadKeys)
            {
              this.drawKeyboardKeys = false;
              this.drawGamePadKeys = true;
              break;
            }
            this.drawGamePadKeys = false;
            break;
          case 2:
            currentMenu = MenuState.Options;
            break;
        }
      }
    }

    public void Update(
      ref MenuState currentMenu,
      ref bool activeGame,
      GameTime gameTime,
      GraphicsDeviceManager graphics,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (keyboard.IsKeyDown(Keys.Escape) && oldKeyboard.IsKeyUp(Keys.Escape) && activeGame)
        Game1.gameState = GameState.Playing;
      this.select(keyboard, oldKeyboard, gamePad, oldGamePad);
      this.updateColors();
      this.checkSelect(ref currentMenu, ref activeGame, graphics, keyboard, oldKeyboard, gamePad, oldGamePad);
      this.m_oldOption = this.m_selectedOption;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Resources.menuBackground, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White * 0.5f);
      spriteBatch.DrawString(Resources.title, "Target", new Vector2((float) Game1.centerX(Resources.title, "Target"), 20f), Color.Gold);
      int index = 0;
      foreach (string option in this.m_options)
      {
        spriteBatch.DrawString(Resources.normal, option, new Vector2((float) Game1.centerX(Resources.normal, option), (float) (150 + 25 * index)), this.m_optionsColors[index]);
        ++index;
      }
      if (this.drawGamePadKeys)
      {
        spriteBatch.Draw(Resources.gamepadKeys, new Rectangle(Game1.screenWidth / 2 - Resources.gamepadKeys.Width / 2, Game1.screenHeight / 2 - Resources.gamepadKeys.Height / 2, Resources.gamepadKeys.Width, Resources.gamepadKeys.Height), Color.White);
      }
      else
      {
        if (!this.drawKeyboardKeys)
          return;
        spriteBatch.Draw(Resources.keyboardKeys, new Rectangle(Game1.screenWidth / 2 - Resources.keyboardKeys.Width / 2, Game1.screenHeight / 2 - Resources.keyboardKeys.Height / 2, Resources.keyboardKeys.Width, Resources.keyboardKeys.Height), Color.White);
      }
    }
  }
}
