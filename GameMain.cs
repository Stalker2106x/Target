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
using Target.Entities;
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
    public static Player player;
    public static HUD hud;
    //Data
    public static List<Target> _targets;
    public static List<Bomb> _bombs;
    public static List<Item> _items;
    public static bool gameOver;

    private static int _targetSpawnTimerActionIndex;
    private static Timer _targetSpawnTimer;
    private static Timer _secondSpawnTimer;

    private static Random _randomGenerator = new Random();
    public static ProbabilityRange itemsProbability = new ProbabilityRange();
    public static ProbabilityRange targetsProbability = new ProbabilityRange();

    public static PlayerStats globalStats;

    public static void resetGame()
    {
      gameOver = false;
      globalStats = PlayerStats.Load();
      hud = new HUD();
      player = new Player();
      _bombs = new List<Bomb>();
      _targets = new List<Target>();
      _items = new List<Item>();
      _targetSpawnTimer = new Timer();
      _targetSpawnTimerActionIndex = _targetSpawnTimer.addAction(TimerDirection.Forward, 3000, TimeoutBehaviour.StartOver, () => { spawnTarget(); });
      _targetSpawnTimer.Start();
      _secondSpawnTimer = new Timer();
      _secondSpawnTimer.addAction(TimerDirection.Forward, 1000, TimeoutBehaviour.StartOver, () => { spawnItem(); spawnBomb(); });
      _secondSpawnTimer.Start();
    }

    public static void addTimeout(double timeout)
    {
      var timerTimeout = _targetSpawnTimer.actions[_targetSpawnTimerActionIndex];
      if (timeout < 0 && timerTimeout.timeout <= 250) return; //Cannot go under 250msec
      timerTimeout.addTimeout(timeout);
      _targetSpawnTimer.actions[_targetSpawnTimerActionIndex] = timerTimeout;
    }

    public static void spawnBomb()
    {
      if (_randomGenerator.Next(0, 61) > 0) return; //1 chance out of 60 per sec
      Bomb bomb = new Bomb();
      bomb.randomizeSpawn();
      _bombs.Add(bomb);
    }

    public static void spawnItem()
    {
      if (_randomGenerator.Next(0, 11) > 0) return; //1 chance out of 10 per sec
      var item = Resources.items[itemsProbability.GetIndex(_randomGenerator.Next(0, 100))].Copy();
      item.randomizeSpawn();
      _items.Add(item);
    }

    public static void spawnTarget()
    {
      addTimeout(-50); //Increase spawn rate
      var target = Resources.targets[targetsProbability.GetIndex(_randomGenerator.Next(0, 100))].Copy();
      target.randomizeSpawn();
      _targets.Add(target);
    }

    public static void destroyEntities()
    {
      for (int index = 0; index < _bombs.Count; ++index)
      {
        if (!_bombs[index].getActivity())
        {
          _bombs.RemoveAt(index);
        }
      }
      for (int index = 0; index < _targets.Count; ++index)
      {
        if (!_targets[index].getActivity())
        {
          _targets.RemoveAt(index);
        }
      }
      for (int index = 0; index < _items.Count; ++index)
      {
        if (!_items[index].getActivity())
        {
          _items.RemoveAt(index);
        }
      }
    }

    public static void GameOver(Desktop menuUI)
    {
      GameMain.gameOver = true;
      GameEngine.setState(GameState.Menu);
      Menu.GameOverMenu(menuUI, "Score: " + player.stats.score.ToString() + "\n"
                              + "Shots fired: " + player.stats.bulletsFired.ToString() + "\n"
                              + "Contracts completed: " + player.stats.contractsCompleted.ToString() + "\n"
                              + "Accuracy: " + Math.Round((double)player.getAccuracy(), 2).ToString() + " %\n");
    }

    public static void Update(GameTime gameTime, Desktop menuUI, DeviceState state, DeviceState prevState)
    {
      if (Options.Config.Bindings[GameAction.Menu].IsControlPressed(state, prevState))
      {
        Menu.GameMenu(menuUI);
        GameEngine.setState(GameState.Menu);
      }
      if (!gameOver)
      {
        destroyEntities();
        if (state.keyboard.IsKeyDown(Keys.I) && prevState.keyboard.IsKeyUp(Keys.I)) spawnItem();
        foreach (Bomb bomb in _bombs)
          bomb.Update(gameTime, state, prevState);
        foreach (Target target in _targets)
          target.Update(gameTime);
        foreach (Item item in _items)
          item.Update(gameTime);
        player.Update(gameTime, menuUI, state, prevState);
        hud.Update(gameTime, ref player, state, prevState);
        _targetSpawnTimer.Update(gameTime);
        _secondSpawnTimer.Update(gameTime);
      }
    }

    public static void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
      if (!gameOver)
      {
        spriteBatch.Draw(Resources.mapWoods, new Rectangle(0, 0, Options.Config.Width, Options.Config.Height), Color.White);
        foreach (Bomb bomb in _bombs)
          bomb.Draw(spriteBatch);
        foreach (Target target in _targets)
          target.Draw(spriteBatch);
        foreach (Item item in _items)
          item.Draw(spriteBatch);
        player.Draw(spriteBatch);
        hud.Draw(graphics, spriteBatch);
      }
    }

    public static void PostDraw()
    {
      hud.DrawUI();
    }
  }
}
