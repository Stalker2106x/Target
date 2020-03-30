// Decompiled with JetBrains decompiler
// Type: Target.Player
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TargetGame.Entities;
using TargetGame.Settings;
using TargetGame.Utils;

namespace TargetGame
{

  public class Player
  {
    public enum BreathState
    {
      Breathing,
      Holding,
      ForceRecovery
    }

    private int _health;
    public int health {
      get { return (_health); }
    }

    private int _healthMax;

    private int _kevlar;
    private int _kevlarMax;

    private bool _defusing;
    public bool defusing { get { return (_defusing); } set { _defusing = value; } }

    private BreathState _breathState;
    private Timer _breathTimer;

    private float _scoreMultiplier;

    private SessionStats _stats;

    private int _comboHeadshot;
    private Weapon _weapon;
    private bool _defuser;
    private SoundEffectInstance _heartbeat;

    public List<Contract> contracts;

    public Player()
    {
      _breathState = BreathState.Breathing;
      _breathTimer = new Timer();
      _kevlarMax = 3;
      _kevlar = 0;
      _healthMax = 100;
      _health = _healthMax;
      _scoreMultiplier = 1;
      _comboHeadshot = 0;
      _stats = new SessionStats();
      contracts = new List<Contract>();
      _weapon = new Weapon("P250", 10);
      _heartbeat = Resources.heartbeat.CreateInstance();
      _heartbeat.Volume = Options.Config.SoundVolume;
      _heartbeat.IsLooped = true;

      _breathTimer.addAction(TimerDirection.Forward, 2501, TimeoutBehaviour.None, () =>
      {
        Resources.outbreath.Play(Options.Config.SoundVolume, 0f, 0f);
        _breathState = BreathState.ForceRecovery;
        GameMain.hud.crosshair.setSway(true);
        _breathTimer.setDuration(2500);
        _breathTimer.Reverse();
      });
      _breathTimer.addAction(TimerDirection.Backward, -1, TimeoutBehaviour.Reset, () =>
      {
        _breathState = BreathState.Breathing;
        _breathTimer.Reverse();
      });
    }

    public ref SessionStats getStats()
    {
      return ref (_stats);
    }

    public float getMultiplier()
    {
      return _scoreMultiplier;
    }

    public BreathState getBreathState()
    {
      return (_breathState);
    }
    public double getBreathTimer()
    {
      return (_breathTimer.getDuration());
    }


    public void setBulletsHit(int hit)
    {
      _stats.bulletsHit += hit;
    }

    public void setDefuser(bool has)
    {
      _defuser = has;
      GameMain.hud.updateDefuser(has);
    }

    public bool isDefusing()
    {
      return (_defusing);
    }

    public void addHealth(int hp, bool ignoreKevlar = false)
    {
      if (hp < 0) //Removing health
      {
        if (_kevlar > 0) //Has kevlar
        {
          _kevlar--;
          return;
        }
        GameMain.hud.triggerBloodsplat();
      }
      _health += hp;
      if (_health > _healthMax) _health = _healthMax;
    }

    public bool hasDefuser()
    {
      return (_defuser);
    }

    public void addKevlar()
    {
      _kevlar = _kevlarMax;
    }

    public void addBulletsFired(int hit)
    {
      _stats.bulletsFired += hit;
    }

    public void addScore(int score)
    {
      _stats.score += (int)(score * _scoreMultiplier);
    }

    public void addContractCompleted()
    {
      _stats.contractsCompleted++;
    }

    public void resetComboHeadshot()
    {
      _comboHeadshot = 0;
      if (_scoreMultiplier > 1)
      {
        _scoreMultiplier = 1;
        Resources.denied.Play(Options.Config.SoundVolume, 0f, 0f);
      }
    }

    public void addComboHeadshot(int headshot)
    {
      _comboHeadshot += headshot;
      if (_comboHeadshot % 10 == 0)
      {
        if (_comboHeadshot % 30 == 0) Resources.unstoppable.Play(Options.Config.SoundVolume, 0f, 0f);
        else Resources.headhunter.Play(Options.Config.SoundVolume, 0f, 0f);
        GameMain.hud.setAction("HEAD HUNTER !");
        _scoreMultiplier += 0.5f;
      }
      if (_stats.headshotComboMax < _comboHeadshot) _stats.headshotComboMax = _comboHeadshot;
    }

    public void addContract()
    {
      contracts.Add(new Contract());
    }

    public Weapon getWeapon()
    {
      return _weapon;
    }

    private void fire()
    {
      List<HitType> hits = _weapon.fire();

      if (hits == null) return; //We did not shoot
      _stats.bulletsHit++;
      foreach (var hit in hits)
      {
        switch (hit)
        {
          case HitType.Headshot:
            GameMain.hud.setAction("Headshot!");
            Resources.headshot.Play(Options.Config.SoundVolume, 0f, 0f);
            GameMain.hud.crosshair.triggerHitmarker();
            _stats.headshotsOrCritical++;
            addComboHeadshot(1);
            break;
          case HitType.Hit:
            GameMain.hud.crosshair.triggerHitmarker();
            resetComboHeadshot();
            break;
          case HitType.Critical:
            GameMain.hud.crosshair.triggerHitmarker();
            GameMain.hud.setAction("Critical!");
            _stats.headshotsOrCritical++;
            break;
          case HitType.Legshot:
            GameMain.hud.crosshair.triggerHitmarker();
            GameMain.hud.setAction("Legshot...");
            _stats.legshots++;
            resetComboHeadshot();
            break;
          case HitType.Catch:
            _stats.bonusCatched++;
            _stats.bulletsHit--;
            break;
          case HitType.Miss:
          default:
            _stats.bulletsHit--;
            resetComboHeadshot();
            break;
        }
        foreach (Contract contract in contracts) contract.Update(hit);
      }
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      //Breath management
      if (_breathState != BreathState.ForceRecovery && Options.Config.Bindings[GameAction.HoldBreath].IsControlDown(state))
      {
        if (_breathTimer.getDuration() == 0 || Options.Config.Bindings[GameAction.HoldBreath].IsControlPressed(state, prevState)) //First time
        {
          Resources.breath.Play(Options.Config.SoundVolume, 0f, 0f);
          _breathTimer.addMilliseconds(500);
          _heartbeat.Play();
        }
        if (!_breathTimer.isActive()) _breathTimer.Start();
        if (_breathTimer.getDirection() != TimerDirection.Forward) _breathTimer.Reverse();
        _breathState = BreathState.Holding;
        GameMain.hud.crosshair.setSway(false);
      }
      else
      {
        if (_breathState != BreathState.ForceRecovery)
        {
          if (_breathTimer.getDirection() != TimerDirection.Backward) _breathTimer.Reverse();
          _breathState = BreathState.Breathing;
          GameMain.hud.crosshair.setSway(true);
        }
        _heartbeat.Stop();
      }
      _breathTimer.Update(gameTime);
      //Weapon management
      if (Options.Config.Bindings[GameAction.Fire].IsControlPressed(state, prevState))
        fire();
      if (Options.Config.Bindings[GameAction.Reload].IsControlPressed(state, prevState))
        _weapon.reload();
      _weapon.Update(gameTime);
      if (contracts.Count > 0) for (int i = contracts.Count-1; i >= 0; i--) if (contracts[i].inactive) contracts.RemoveAt(i); //Clear obsolete contracts
      if (_health <= 0)
      {

        _heartbeat.Stop(); //Clean heartbeat state
        GameMain.GameOver();
      }
      //Bomb Management
      foreach (var bomb in GameMain.bombs)
      {
        if (GameMain.hud.crosshair.checkCollision(bomb.getRectangle()))
        {
          if (Options.Config.Bindings[GameAction.Defuse].IsControlPressed(state, prevState)) bomb.Defuse(); //User started defusing
          else if (!Options.Config.Bindings[GameAction.Defuse].IsControlDown(state)) bomb.Rearm(); //Not holding, rearm
          //Fall here if holding, nothing happens, let the timer go out
        }
        else bomb.Rearm(); //None, rearm
      }
      //HUD
      GameMain.hud.updateHealth(_healthMax, _health);
      GameMain.hud.updateKevlar(_kevlarMax, _kevlar);
      GameMain.hud.updateBreath(_breathTimer.getDuration());
      GameMain.hud.updateScoreMultiplier(_scoreMultiplier);
      GameMain.hud.updateScore(_stats.score);
      GameMain.hud.updateMagazine(_weapon.getMagazine());
      GameMain.hud.updateReload(_weapon);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _weapon.Draw(spriteBatch);
    }
  }
}
