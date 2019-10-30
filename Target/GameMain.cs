// Decompiled with JetBrains decompiler
// Type: Target.GameMain
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Target
{
  public static class GameMain
  {
    //Handles
    private static GraphicsDeviceManager Graphics;
    public static Player _player = new Player();
    public static HUD _hud = new HUD(ref graphics);
    //Data
    public static List<Target> _targets = new List<Target>();
    public static List<Item> _items = new List<Item>();
    public static bool gameOver = false;
    public static bool gameQuit = false;
    public static float spawnTimer = 0.0f;
    public static float spawnDelay = 3f;

    private static Random randomSpawn = new Random();

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
          _hud.setHitmarker();
        }
      }
      for (int index = 0; index < _items.Count; ++index)
      {
        if (!_items[index].getActivity())
        {
          _items.RemoveAt(index);
          _hud.setHitmarker();
        }
      }
    }

    public static void Update(
      GameTime gameTime,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (keyboard.IsKeyDown(Keys.Escape) && oldKeyboard.IsKeyUp(Keys.Escape) || gamePad.IsButtonDown(Buttons.Start))
      {
        if (!gameOver)
          Game1.gameState = GameState.Menu;
      }
      else if (_player.getHealth() <= 0)
        gameOver = true;
      if (!gameOver)
      {
        spawnTarget();
        destroyTargets();
        if (keyboard.IsKeyDown(Keys.I) && oldKeyboard.IsKeyUp(Keys.I))
          _items.Add(new Item());
        for (int index = 0; index < _targets.Count; ++index)
          _targets[index].Update(ref _player, gameTime, mouse);
        for (int index = 0; index < _items.Count; ++index)
          _items[index].Update(gameTime);
        _player.Update(gameTime, keyboard, oldKeyboard, mouse, oldMouse, gamePad, oldGamePad);
        _hud.Update(ref _player, gameTime, keyboard, oldKeyboard, mouse, oldMouse, gamePad, oldGamePad);
        spawnTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      }
      else
      {
        if ((!keyboard.IsKeyDown(Keys.Escape) || !oldKeyboard.IsKeyUp(Keys.Escape)) && (!keyboard.IsKeyDown(Keys.Enter) || !oldKeyboard.IsKeyUp(Keys.Enter)) && (!gamePad.IsButtonDown(Buttons.A) || !oldGamePad.IsButtonUp(Buttons.A)))
          return;
        gameQuit = true;
      }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      if (gameOver)
      {
        if (gameQuit)
          return;
        spriteBatch.Draw(Resources.menuBackground, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.DimGray);
        spriteBatch.DrawString(Resources.title, "Game Over", new Vector2((float) Game1.centerX(Resources.title, "Game Over"), (float) (Game1.screenHeight / 2 - 60)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Appuyez sur A / Entree", new Vector2((float) Game1.centerX(Resources.normal, "Appuyez sur A / Entree"), (float) (Game1.screenHeight / 2 - 15)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Score: " + _player.getScore().ToString(), new Vector2((float) Game1.centerX(Resources.normal, "Score: " + _player.getScore().ToString()), (float) (Game1.screenHeight / 2 + 60)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Tirs: " + _player.getBulletsFired().ToString(), new Vector2((float) Game1.centerX(Resources.normal, "Tirs: " + _player.getBulletsFired().ToString()), (float) (Game1.screenHeight / 2 + 90)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Precision: " + Math.Round((double) _player.getAccuracy(), 2).ToString() + " %", new Vector2((float) Game1.centerX(Resources.normal, "Precision: " + Math.Round((double) _player.getAccuracy(), 2).ToString() + " %"), (float) (Game1.screenHeight / 2 + 120)), Color.White);
      }
      else
      {
        spriteBatch.Draw(Resources.mapWoods, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);
        foreach (Target target in _targets)
          target.Draw(spriteBatch);
        foreach (Item obj in _items)
          obj.Draw(spriteBatch);
        _player.Draw(spriteBatch);
        _hud.Draw(spriteBatch);
      }
    }
  }
}
