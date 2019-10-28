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
    private MenuState m_menu;
    private Menu m_main;
    private Menu m_pause;
    private Menu m_options;
    private Menu m_infoKeys;
    private bool activeGame;

    public GameMenu()
    {
      this.activeGame = false;
      this.m_menu = MenuState.Main;
      this.m_main = new Menu(MenuState.Main, "Target", 3, new List<string>()
      {
        "Jouer",
        "Options",
        "Quitter"
      });
      this.m_pause = new Menu(MenuState.Pause, "Target", 4, new List<string>()
      {
        "Reprendre",
        "Options",
        "Terminer Partie",
        "Quitter"
      });
      this.m_options = new Menu(MenuState.Options, "Options", 4, new List<string>()
      {
        "Liste Contrôles",
        "Plein Ecran",
        "V-Sync",
        "Retour"
      });
      this.m_infoKeys = new Menu(MenuState.InfoKeys, "Info Contrôles", 3, new List<string>()
      {
        "Clavier / Souris",
        "Gamepad",
        "Retour"
      });
    }

    public void resetGame(bool active)
    {
      this.m_menu = MenuState.Main;
      this.activeGame = active;
    }

    public bool getActivity()
    {
      return this.activeGame;
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
      if (this.m_menu == MenuState.Main)
        this.m_main.Update(ref this.m_menu, ref this.activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      else if (this.m_menu == MenuState.Pause)
        this.m_pause.Update(ref this.m_menu, ref this.activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      else if (this.m_menu == MenuState.Options)
      {
        this.m_options.Update(ref this.m_menu, ref this.activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      }
      else
      {
        if (this.m_menu != MenuState.InfoKeys)
          return;
        this.m_infoKeys.Update(ref this.m_menu, ref this.activeGame, gameTime, graphics, keyboard, oldKeyboard, mouse, gamePad, oldGamePad);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.m_menu == MenuState.Main)
        this.m_main.Draw(spriteBatch);
      else if (this.m_menu == MenuState.Pause)
        this.m_pause.Draw(spriteBatch);
      else if (this.m_menu == MenuState.Options)
      {
        this.m_options.Draw(spriteBatch);
      }
      else
      {
        if (this.m_menu != MenuState.InfoKeys)
          return;
        this.m_infoKeys.Draw(spriteBatch);
      }
    }
  }
}
