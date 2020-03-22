// Decompiled with JetBrains decompiler
// Type: Target.GameMain
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace Target
{

  public enum GameState
  {
      Menu,
      Playing
  }

  public static class GameMain
  {
    //Handles
    public static Player _player;
    public static HUD hud;
    //Data
    public static List<Target> _targets;
    public static List<Item> _items;
    public static bool gameOver;
    public static float spawnTimer = 0.0f;
    public static float spawnDelay;

    private static Random randomSpawn = new Random();
    
    public static void resetGame()
    {
        gameOver = false;
        _player = new Player();
        hud = new HUD();
        _targets = new List<Target>();
        _items = new List<Item>();
        spawnDelay = 3f;
    }

    public static void spawnTarget()
    {
      if ((double) spawnTimer < (double) spawnDelay)
        return;
      spawnTimer = 0.0f;
      if ((double) spawnDelay > 0.75)
        spawnDelay -= 0.05f;
      if (randomSpawn.Next(1, 100) <= 90)
        _targets.Add(new Target(1));
      else
        _items.Add(new Item());
    }

    public static void destroyTargets()
    {
      for (int index = 0; index < _targets.Count; ++index)
      {
        if (!_targets[index].getActivity())
        {
          _targets.RemoveAt(index);
          hud.setHitmarker();
        }
      }
      for (int index = 0; index < _items.Count; ++index)
      {
        if (!_items[index].getActivity())
        {
          _items.RemoveAt(index);
          hud.setHitmarker();
        }
      }
    }

    public static void Update(
      GameTime gameTime,
      Desktop menuUI,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (keyboard.IsKeyDown(Keys.Escape) && oldKeyboard.IsKeyUp(Keys.Escape) || gamePad.IsButtonDown(Buttons.Start))
      {
        Menu.GameMenu(menuUI);
        Game1.gameState = GameState.Menu;
      }
      if (_player.getHealth() <= 0)
      {
        gameOver = true;
        Game1.gameState = GameState.Menu;
        Menu.GameOverMenu(menuUI, "Score: " + _player.getScore().ToString() + "\n"
                              + "Tirs: " + _player.getBulletsFired().ToString() + "\n"
                              + "Precision: " + Math.Round((double)_player.getAccuracy(), 2).ToString() + " %\n");
      }
      if (!gameOver)
      {
        spawnTarget();
        destroyTargets();
        if (keyboard.IsKeyDown(Keys.I) && oldKeyboard.IsKeyUp(Keys.I))
          _items.Add(new Item());
        for (int index = 0; index < _targets.Count; ++index)
          _targets[index].Update(_player, gameTime, mouse);
        for (int index = 0; index < _items.Count; ++index)
          _items[index].Update(gameTime);
        _player.Update(gameTime, keyboard, oldKeyboard, mouse, oldMouse, gamePad, oldGamePad);
        hud.Update(ref _player, gameTime, keyboard, oldKeyboard, mouse, oldMouse, gamePad, oldGamePad);
        spawnTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      }
    }

    public static void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
      if (!gameOver)
      {
        spriteBatch.Draw(Resources.mapWoods, new Rectangle(0, 0, Options.Config.Width, Options.Config.Height), Color.White);
        foreach (Target target in _targets)
          target.Draw(spriteBatch);
        foreach (Item obj in _items)
          obj.Draw(spriteBatch);
        _player.Draw(spriteBatch);
        hud.Draw(graphics, spriteBatch);
      }
    }

    public static void PostDraw()
    {
      hud.DrawUI();
    }
  }
}
