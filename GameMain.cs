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
    Splashscreen,
    Menu,
    Playing,
    Tutorial
  }

  public static class GameMain
  {
    //Handles
    public static PlayerStats globalStats;
    public static Player player;
    public static HUD hud;
    public static Tutorial tutorial;

    //Data
    public static List<Target> targets;
    public static List<Bomb> bombs;
    public static List<Item> items;
    public static bool gameOver;

    public static ProbabilityRange itemsProbability = new ProbabilityRange();
    public static ProbabilityRange targetsProbability = new ProbabilityRange();

    private static int _targetSpawnTimerActionIndex;
    private static Timer _targetSpawnTimer;
    private static Timer _secondSpawnTimer;

    private static Random _randomGenerator = new Random();

    public static void initGame()
    {
      globalStats = PlayerStats.Load();

    }
    public static void resetGame(GameState state)
    {
      gameOver = false;
      hud = new HUD();
      player = new Player();
      bombs = new List<Bomb>();
      targets = new List<Target>();
      items = new List<Item>();
      _targetSpawnTimer = new Timer();
      _secondSpawnTimer = new Timer();
      if (state == GameState.Playing) setGameTimers();
      else if (state == GameState.Tutorial) tutorial = new Tutorial();
      GameEngine.setState(state);
    }

    public static void setGameTimers()
    {
      _targetSpawnTimerActionIndex = _targetSpawnTimer.addAction(TimerDirection.Forward, 3000, TimeoutBehaviour.StartOver, () => { spawnTarget(); });
      _targetSpawnTimer.Start();
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
      bomb.randomizePosition();
      bomb.activate();
      bombs.Add(bomb);
    }

    public static void spawnItem()
    {
      if (_randomGenerator.Next(0, 11) > 0) return; //1 chance out of 10 per sec
      var item = Resources.items[itemsProbability.GetIndex(_randomGenerator.Next(0, 100))].Copy();
      item.randomizePosition();
      //item.activate();
      items.Add(item);
    }

    public static void spawnTarget()
    {
      addTimeout(-50); //Increase spawn rate
      var target = Resources.targets[targetsProbability.GetIndex(_randomGenerator.Next(0, 100))].Copy();
      target.randomizePosition();
      target.activate();
      targets.Add(target);
    }

    public static void destroyEntities()
    {
      for (int index = 0; index < bombs.Count; ++index)
      {
        if (!bombs[index].getActivity())
        {
          bombs.RemoveAt(index);
        }
      }
      for (int index = 0; index < targets.Count; ++index)
      {
        if (!targets[index].getActivity())
        {
          targets.RemoveAt(index);
        }
      }
      for (int index = 0; index < items.Count; ++index)
      {
        if (!items[index].getActivity())
        {
          items.RemoveAt(index);
        }
      }
    }

    public static void GameOver()
    {
      GameMain.gameOver = true;
      GameEngine.setState(GameState.Menu);
      Menu.GameOverMenu("Score: " + player.getStats().score.ToString() + "\n"
                      + "Shots fired: " + player.getStats().bulletsFired.ToString() + "\n"
                      + "Contracts completed: " + player.getStats().contractsCompleted.ToString() + "\n"
                      + "Accuracy: " + Math.Round((double)player.getAccuracy(), 2).ToString() + " %\n");
    }

    public static void ExitToMain()
    {
      GameMain.gameOver = true;
      GameMain.tutorial = null;
      GameEngine.setState(GameState.Menu);
      Menu.MainMenu();
    }

    public static void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      if (Options.Config.Bindings[GameAction.Menu].IsControlPressed(state, prevState))
      {
        Menu.GameMenu();
        GameEngine.setState(GameState.Menu);
      }
      if (!gameOver)
      {
        destroyEntities();
        if (state.keyboard.IsKeyDown(Keys.I) && prevState.keyboard.IsKeyUp(Keys.I)) spawnItem();
        foreach (Bomb bomb in bombs)
          bomb.Update(gameTime, state, prevState);
        foreach (Target target in targets)
          target.Update(gameTime);
        foreach (Item item in items)
          item.Update(gameTime);
        player.Update(gameTime, state, prevState);
        hud.Update(gameTime, ref player, state, prevState);
        if (GameEngine.getState() == GameState.Tutorial) tutorial.Update(gameTime);
        _targetSpawnTimer.Update(gameTime);
        _secondSpawnTimer.Update(gameTime);
      }
    }

    public static void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
      if (!gameOver)
      {
        spriteBatch.Draw(Resources.mapWoods, new Rectangle(0, 0, Options.Config.Width, Options.Config.Height), Color.White);
        foreach (Bomb bomb in bombs)
          bomb.Draw(spriteBatch);
        foreach (Target target in targets)
          target.Draw(spriteBatch);
        foreach (Item item in items)
          item.Draw(spriteBatch);
        player.Draw(spriteBatch);
        hud.Draw(spriteBatch);
      }
    }
  }
}
