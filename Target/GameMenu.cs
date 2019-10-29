// Decompiled with JetBrains decompiler
// Type: Target.GameMenu
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Target
{
  internal class GameMenu
  {
    private MenuState _menu;
    private Menu _main;
    private Menu _pause;
    private Menu _options;
    private Menu _infoKeys;
    private bool activeGame;

    public GameMenu()
    {
      activeGame = false;
      _menu = MenuState.Main;
      _main = new Menu(MenuState.Main, "Target", 3, new List<string>()
      {
        "Jouer",
        "Options",
        "Quitter"
      });
      _pause = new Menu(MenuState.Pause, "Target", 4, new List<string>()
      {
        "Reprendre",
        "Options",
        "Terminer Partie",
        "Quitter"
      });
      _options = new Menu(MenuState.Options, "Options", 4, new List<string>()
      {
        "Liste Contrôles",
        "Plein Ecran",
        "V-Sync",
        "Retour"
      });
      _infoKeys = new Menu(MenuState.InfoKeys, "Info Contrôles", 3, new List<string>()
      {
        "Clavier / Souris",
        "Gamepad",
        "Retour"
      });
    }

    public void resetGame(bool active)
    {
      _menu = MenuState.Main;
      activeGame = active;
    }

    public bool getActivity()
    {
      return activeGame;
    }

    public void Update(
      GameTime gameTime,
      GraphicsDeviceManager graphics,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (_menu == MenuState.Main)
        _main.Update(ref _menu, ref activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      else if (_menu == MenuState.Pause)
        _pause.Update(ref _menu, ref activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      else if (_menu == MenuState.Options)
      {
        _options.Update(ref _menu, ref activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      }
      else
      {
        if (_menu != MenuState.InfoKeys)
          return;
        _infoKeys.Update(ref _menu, ref activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (_menu == MenuState.Main)
        _main.Draw(spriteBatch);
      else if (_menu == MenuState.Pause)
        _pause.Draw(spriteBatch);
      else if (_menu == MenuState.Options)
      {
        _options.Draw(spriteBatch);
      }
      else
      {
        if (_menu != MenuState.InfoKeys)
          return;
        _infoKeys.Draw(spriteBatch);
      }
    }
  }
}
