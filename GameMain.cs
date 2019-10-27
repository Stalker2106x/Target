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
  internal class GameMain
  {
    private GraphicsDeviceManager m_graphics;
    public static Player m_player;
    public static HUD m_hud;
    public static List<Target.Target> m_targets;
    public static List<Item> m_items;
    public static bool gameOver;
    public static bool gameQuit;
    private float spawnTimer;
    private float spawnDelay;
    private Random randomSpawn;

    public GameMain(ref GraphicsDeviceManager graphics)
    {
      this.m_graphics = graphics;
      GameMain.gameOver = false;
      GameMain.gameQuit = false;
      this.randomSpawn = new Random();
      this.spawnTimer = 0.0f;
      this.spawnDelay = 3f;
      GameMain.m_targets = new List<Target.Target>();
      GameMain.m_items = new List<Item>();
      GameMain.m_player = new Player();
      GameMain.m_hud = new HUD(ref graphics);
    }

    public void spawnTarget()
    {
      if ((double) this.spawnTimer < (double) this.spawnDelay)
        return;
      this.spawnTimer = 0.0f;
      if ((double) this.spawnDelay > 0.75)
        this.spawnDelay -= 0.1f;
      if (this.randomSpawn.Next(1, 100) <= 90)
        GameMain.m_targets.Add(new Target.Target(1));
      else
        GameMain.m_items.Add(new Item());
    }

    public void destroyTargets()
    {
      for (int index = 0; index < GameMain.m_targets.Count; ++index)
      {
        if (!GameMain.m_targets[index].getActivity())
        {
          GameMain.m_targets.RemoveAt(index);
          GameMain.m_hud.setHitmarker();
        }
      }
      for (int index = 0; index < GameMain.m_items.Count; ++index)
      {
        if (!GameMain.m_items[index].getActivity())
        {
          GameMain.m_items.RemoveAt(index);
          GameMain.m_hud.setHitmarker();
        }
      }
    }

    public void Update(
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
        if (!GameMain.gameOver)
          Game1.gameState = GameState.Menu;
      }
      else if (GameMain.m_player.getHealth() <= 0)
        GameMain.gameOver = true;
      if (!GameMain.gameOver)
      {
        this.spawnTarget();
        this.destroyTargets();
        if (keyboard.IsKeyDown(Keys.I) && oldKeyboard.IsKeyUp(Keys.I))
          GameMain.m_items.Add(new Item());
        for (int index = 0; index < GameMain.m_targets.Count; ++index)
          GameMain.m_targets[index].Update(ref GameMain.m_player, gameTime, mouse);
        for (int index = 0; index < GameMain.m_items.Count; ++index)
          GameMain.m_items[index].Update(gameTime);
        GameMain.m_player.Update(gameTime, keyboard, oldKeyboard, mouse, oldMouse, gamePad, oldGamePad);
        GameMain.m_hud.Update(ref GameMain.m_player, gameTime, keyboard, oldKeyboard, mouse, oldMouse, gamePad, oldGamePad);
        this.spawnTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      }
      else
      {
        if ((!keyboard.IsKeyDown(Keys.Escape) || !oldKeyboard.IsKeyUp(Keys.Escape)) && (!keyboard.IsKeyDown(Keys.Enter) || !oldKeyboard.IsKeyUp(Keys.Enter)) && (!gamePad.IsButtonDown(Buttons.A) || !oldGamePad.IsButtonUp(Buttons.A)))
          return;
        GameMain.gameQuit = true;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (GameMain.gameOver)
      {
        if (GameMain.gameQuit)
          return;
        spriteBatch.Draw(Resources.menuBackground, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.DimGray);
        spriteBatch.DrawString(Resources.title, "Game Over", new Vector2((float) Game1.centerX(Resources.title, "Game Over"), (float) (Game1.screenHeight / 2 - 60)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Appuyez sur A / Entree", new Vector2((float) Game1.centerX(Resources.normal, "Appuyez sur A / Entree"), (float) (Game1.screenHeight / 2 - 15)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Score: " + GameMain.m_player.getScore().ToString(), new Vector2((float) Game1.centerX(Resources.normal, "Score: " + GameMain.m_player.getScore().ToString()), (float) (Game1.screenHeight / 2 + 60)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Tirs: " + GameMain.m_player.getBulletsFired().ToString(), new Vector2((float) Game1.centerX(Resources.normal, "Tirs: " + GameMain.m_player.getBulletsFired().ToString()), (float) (Game1.screenHeight / 2 + 90)), Color.White);
        spriteBatch.DrawString(Resources.normal, "Precision: " + Math.Round((double) GameMain.m_player.getAccuracy(), 2).ToString() + " %", new Vector2((float) Game1.centerX(Resources.normal, "Precision: " + Math.Round((double) GameMain.m_player.getAccuracy(), 2).ToString() + " %"), (float) (Game1.screenHeight / 2 + 120)), Color.White);
      }
      else
      {
        spriteBatch.Draw(Resources.mapWoods, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);
        foreach (Target.Target target in GameMain.m_targets)
          target.Draw(spriteBatch);
        foreach (Item obj in GameMain.m_items)
          obj.Draw(spriteBatch);
        GameMain.m_player.Draw(spriteBatch);
        GameMain.m_hud.Draw(spriteBatch);
      }
    }
  }
}
