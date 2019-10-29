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
    private MenuState _menuState;
    private int _selectedOption;
    private int _oldOption;
    private string _title;
    private int _nbOptions;
    private List<string> _options;
    private List<Color> _optionsColors;
    private bool drawGamePadKeys;
    private bool drawKeyboardKeys;

    public Menu(MenuState menuState, string title, int nbOptions, List<string> options)
    {
      drawGamePadKeys = false;
      drawKeyboardKeys = false;
      _menuState = menuState;
      _nbOptions = nbOptions;
      _selectedOption = 0;
      _oldOption = nbOptions - 1;
      _title = title;
      _options = options;
      _optionsColors = new List<Color>();
      for (int index = 1; index <= nbOptions; ++index)
        _optionsColors.Add(Color.DarkGray);
    }

    public void setOption(int index, string option)
    {
      _options[index] = option;
    }

    public void select(
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (_selectedOption < 0)
        _selectedOption = _nbOptions - 1;
      else if (_selectedOption > _nbOptions - 1)
        _selectedOption = 0;
      else if (keyboard.IsKeyDown(Keys.Up) && oldKeyboard.IsKeyUp(Keys.Up) || (double) gamePad.ThumbSticks.Left.Y > 0.0 && (double) oldGamePad.ThumbSticks.Left.Y == 0.0)
      {
        Resources.menuClick.Play();
        --_selectedOption;
      }
      else
      {
        if ((!keyboard.IsKeyDown(Keys.Down) || !oldKeyboard.IsKeyUp(Keys.Down)) && ((double) gamePad.ThumbSticks.Left.Y >= 0.0 || (double) oldGamePad.ThumbSticks.Left.Y != 0.0))
          return;
        Resources.menuClick.Play();
        ++_selectedOption;
      }
    }

    public void updateColors()
    {
      if (_selectedOption < 0 || _selectedOption > _nbOptions - 1)
        return;
      if (_oldOption >= 0 && _oldOption <= _nbOptions - 1)
        _optionsColors[_oldOption] = Color.Gray;
      else if (_oldOption < 0)
        _optionsColors[0] = Color.Gray;
      else
        _optionsColors[_nbOptions - 1] = Color.Gray;
      _optionsColors[_selectedOption] = Color.AntiqueWhite;
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
      if (_menuState == MenuState.Main)
      {
        switch (_selectedOption)
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
      else if (_menuState == MenuState.Pause)
      {
        switch (_selectedOption)
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
      else if (_menuState == MenuState.Options)
      {
        switch (_selectedOption)
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
        if (_menuState != MenuState.InfoKeys)
          return;
        switch (_selectedOption)
        {
          case 0:
            if (!drawKeyboardKeys)
            {
              drawGamePadKeys = false;
              drawKeyboardKeys = true;
              break;
            }
            drawKeyboardKeys = false;
            break;
          case 1:
            if (!drawGamePadKeys)
            {
              drawKeyboardKeys = false;
              drawGamePadKeys = true;
              break;
            }
            drawGamePadKeys = false;
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
      select(keyboard, oldKeyboard, gamePad, oldGamePad);
      updateColors();
      checkSelect(ref currentMenu, ref activeGame, graphics, keyboard, oldKeyboard, gamePad, oldGamePad);
      _oldOption = _selectedOption;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Resources.menuBackground, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White * 0.5f);
      spriteBatch.DrawString(Resources.title, "Target", new Vector2((float) Game1.centerX(Resources.title, "Target"), 20f), Color.Gold);
      int index = 0;
      foreach (string option in _options)
      {
        spriteBatch.DrawString(Resources.normal, option, new Vector2((float) Game1.centerX(Resources.normal, option), (float) (150 + 25 * index)), _optionsColors[index]);
        ++index;
      }
      if (drawGamePadKeys)
      {
        spriteBatch.Draw(Resources.gamepadKeys, new Rectangle(Game1.screenWidth / 2 - Resources.gamepadKeys.Width / 2, Game1.screenHeight / 2 - Resources.gamepadKeys.Height / 2, Resources.gamepadKeys.Width, Resources.gamepadKeys.Height), Color.White);
      }
      else
      {
        if (!drawKeyboardKeys)
          return;
        spriteBatch.Draw(Resources.keyboardKeys, new Rectangle(Game1.screenWidth / 2 - Resources.keyboardKeys.Width / 2, Game1.screenHeight / 2 - Resources.keyboardKeys.Height / 2, Resources.keyboardKeys.Width, Resources.keyboardKeys.Height), Color.White);
      }
    }
  }
}
