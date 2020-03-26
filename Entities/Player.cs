// Decompiled with JetBrains decompiler
// Type: Target.Player
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Target.Entities;
using Target.Utils;

namespace Target
{
  public class Player
  {
    public enum BreathState
    {
      Breathing,
      Holding,
      ForceRecovery
    }

    private int _hp;
    private int _maxHp;
    private int _score;

    private BreathState _breathState;
    private Timer _breathTimer;

    private float _scoreMultiplier;

    private int _comboHeadshot;
    private int _bulletsFired;
    private int _bulletsHit;
    private Weapon _weapon;
    private SoundEffectInstance _heartbeat;

    private List<Contract> _contracts;

    public Player()
    {
      _breathState = BreathState.Breathing;
      _breathTimer = new Timer();
      _hp = 100;
      _maxHp = 100;
      _score = 0;
      _scoreMultiplier = 1;
      _comboHeadshot = 0;
      _bulletsFired = 0;
      _bulletsHit = 0;
      _contracts = new List<Contract>();
      _weapon = new Weapon("P250", 15);
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

    public int getHealth()
    {
      return _hp;
    }

    public int getMaxHealth()
    {
      return _maxHp;
    }

    public float getMultiplier()
    {
      return _scoreMultiplier;
    }

    public int getScore()
    {
      return _score;
    }

    public BreathState getBreathState()
    {
      return (_breathState);
    }
    public double getBreathTimer()
    {
      return (_breathTimer.getDuration());
    }

    public float getAccuracy()
    {
      if (_bulletsFired == 0) return (100); //No bullets, 100%
      return ((float)(_bulletsHit / _bulletsFired) * 100.0f);
    }

    public int getBulletsFired()
    {
      return _bulletsFired;
    }

    public void setScore(int score)
    {
      _score += (int)(score * _scoreMultiplier);
    }

    public void setHealth(int hp)
    {
      _hp += hp;
    }

    public void resetComboHeadshot()
    {
      _comboHeadshot = 0;
      _scoreMultiplier = 1;
    }

    public void setComboHeadshot(int headshot)
    {
      _comboHeadshot += headshot;
      if (_comboHeadshot % 10 == 0)
      {
        Resources.headhunter.Play(Options.Config.SoundVolume, 0f, 0f);
        GameMain.hud.setAction("HEAD HUNTER !");
        _scoreMultiplier += 0.5f;
      }
    }

    public void setBulletsFired(int bullet)
    {
      _bulletsFired += bullet;
    }

    public void setBulletsHit(int bullet)
    {
      _bulletsHit += bullet;
    }
    public void addContract()
    {
      _contracts.Add(new Contract());
    }

    public Weapon getWeapon()
    {
      return _weapon;
    }

    public void healthCap()
    {
      if (_hp <= _maxHp)
        return;
      _hp = _maxHp;
    }

    private void fire()
    {
      List<HitType> hits = _weapon.fire();

      if (hits == null) return; //We did not shoot
      foreach (var hit in hits)
      {
        _bulletsHit++;
        switch (hit)
        {
          case HitType.Headshot:
            GameMain.hud.setAction("Headshot!");
            Resources.headshot.Play(Options.Config.SoundVolume, 0f, 0f);
            setScore(40);
            setComboHeadshot(1);
            break;
          case HitType.Hit:
            setScore(20);
            resetComboHeadshot();
            break;
          case HitType.Legshot:
            GameMain.hud.setAction("Legshot...");
            GameMain._player.setScore(10);
            resetComboHeadshot();
            break;
          case HitType.Catch:
            GameMain.hud.setAction("BONUS");
            break;
          case HitType.Miss:
          default:
            _bulletsHit--; //Remove the hit
            resetComboHeadshot();
            break;
        }
        foreach (Contract contract in _contracts) contract.Update(hit);
      }
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      //Breath management
      if (_breathState != BreathState.ForceRecovery && Options.Bindings[GameAction.HoldBreath].IsControlDown(state))
      {
        if (_breathTimer.getDuration() == 0 || Options.Bindings[GameAction.HoldBreath].IsControlPressed(state, prevState)) //First time
        {
          Resources.breath.Play(Options.Config.SoundVolume, 0f, 0f);
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
      if (Options.Bindings[GameAction.Fire].IsControlPressed(state, prevState))
        fire();
      if (Options.Bindings[GameAction.Reload].IsControlPressed(state, prevState))
        _weapon.reload();
      healthCap();
      _weapon.Update(gameTime);
      if (_contracts.Count > 0) for (int i = _contracts.Count-1; i >= 0; i--) if (_contracts[i].inactive) _contracts.RemoveAt(i); //Clear obsolete contracts
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
