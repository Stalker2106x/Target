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
using System.Linq;
using Target.Utils;

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
    public static int spawnTimerActionIndex;
    public static Timer spawnTimer;

    private static Random randomSpawn = new Random();
    
    public static void resetGame()
    {
        gameOver = false;
        _player = new Player();
        hud = new HUD();
        _targets = new List<Target>();
        _items = new List<Item>();
        spawnTimer = new Timer();
        spawnTimerActionIndex = spawnTimer.addAction(TimerDirection.Forward, 3000, TimeoutBehaviour.StartOver, () => { spawnTarget(); });
        spawnTimer.Start();
    }

    public static void addTimeout(double timeout)
    {
      var timerTimeout = spawnTimer.actions[spawnTimerActionIndex];
      if (timeout < 0 && timerTimeout.timeout <= 750) return; //Cannot go under 750msec
      timerTimeout.addTimeout(timeout);
      spawnTimer.actions[spawnTimerActionIndex] = timerTimeout;
    }

    public static void spawnTarget()
    {
      addTimeout(-50); //Increase spawn rate
      if (randomSpawn.Next(0, 100) <= 90)
      {
        var target = Resources.targets[randomSpawn.Next(0, Resources.targets.Count)].Copy();
        target.randomizeSpawn();
        _targets.Add(target);
      }
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

    public static void GameOver(Desktop menuUI)
    {
      GameMain.gameOver = true;
      GameEngine.setState(GameState.Menu);
      Menu.GameOverMenu(menuUI, "Score: " + _player.stats.score.ToString() + "\n"
                              + "Shots fired: " + _player.stats.bulletsFired.ToString() + "\n"
                              + "Contrats completed: " + _player.stats.contractsCompleted.ToString() + "\n"
                              + "Accuracy: " + Math.Round((double)_player.getAccuracy(), 2).ToString() + " %\n");
    }

    public static void Update(GameTime gameTime, Desktop menuUI, DeviceState state, DeviceState prevState)
    {
      if (Options.Bindings[GameAction.Menu].IsControlPressed(state, prevState))
      {
        Menu.GameMenu(menuUI);
        GameEngine.setState(GameState.Menu);
      }
      if (!gameOver)
      {
        destroyTargets();
        if (state.keyboard.IsKeyDown(Keys.I) && prevState.keyboard.IsKeyUp(Keys.I)) _items.Add(new Item());
        for (int index = 0; index < _targets.Count; ++index)
          _targets[index].Update(gameTime);
        for (int index = 0; index < _items.Count; ++index)
          _items[index].Update(gameTime);
        _player.Update(gameTime, menuUI, state, prevState);
        hud.Update(gameTime, ref _player, state, prevState);
        spawnTimer.Update(gameTime);
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
