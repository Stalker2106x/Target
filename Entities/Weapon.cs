// Decompiled with JetBrains decompiler
// Type: Target.Weapon
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Target.Utils;

namespace Target
{
    public enum WeaponState
    {
        Idle,
        Reloading
    }

    public class Weapon
    {
      private string _name;
      private int _damage;
      private int _magazine;
      private int _maxMagazine;
      private WeaponState _state;
      private int _reloadTimerActionIndex;
      private Timer _reloadTimer;

      public Weapon(string name, int maxMagazine)
      {
        _name = name;
        _maxMagazine = maxMagazine;
        _damage = 100;
        _magazine = _maxMagazine;
        _state = WeaponState.Idle;
        _reloadTimer = new Timer();
        _reloadTimerActionIndex = _reloadTimer.addAction(TimerDirection.Forward, 1500, TimeoutBehaviour.Reset, () => { });
      }

      public int getMagazine()
      {
        return _magazine;
      }

      public int getMaxMagazine()
      {
        return _maxMagazine;
      }

    public void addBullets(int bullets)
    {
      _magazine += bullets;
    }
    public void addMagazineSize(int expandSize)
    {
      _maxMagazine += expandSize;
    }
    public void addReloadDuration(float duration)
    {
      var action = _reloadTimer.actions[_reloadTimerActionIndex];
      action.timeout += duration;
      _reloadTimer.actions[_reloadTimerActionIndex] = action;
    }

    public WeaponState getState()
      {
        return _state;
      }

      public double getStatusTimer()
      {
        return _reloadTimer.getDuration();
      }

      public List<HitType> fire()
      {
        if (_magazine <= 0) reload();
        if (_reloadTimer.isActive()) return (null);
        List<HitType> hits = new List<HitType>();

        for (int index = 0; index < GameMain._items.Count; ++index)
        {
          HitType hit = GameMain._items[index].checkCollision();
          if (hit != HitType.Miss)
          {
            hits.Add(hit); //Add hit if different from miss
            return (hits); //return immediately, its a catch, we dont want to fire
          }
        }
        for (int index = 0; index < GameMain._targets.Count; ++index)
        {
          HitType hit = GameMain._targets[index].checkCollision(_damage);
          if (hit != HitType.Miss) hits.Add(hit); //Add hit if different from miss
        }
        if (hits.Count == 0) hits.Add(HitType.Miss); //No hit, add miss
        
        _magazine--;
        GameMain._player.addBulletsFired(1);
        GameMain.hud.crosshair.triggerRecoil();
        Resources.fire.Play(Options.Config.SoundVolume, 0f, 0f);
        return (hits);
      }

      public void reload()
      {
        if (_magazine >= _maxMagazine || _reloadTimer.isActive()) return;
        Resources.reload.Play(Options.Config.SoundVolume, 0f, 0f);
        _magazine = _maxMagazine;
        _reloadTimer.Start();
      }


      public void Update(GameTime gameTime)
      {
        _reloadTimer.Update(gameTime);
      }

      public void Draw(SpriteBatch spriteBatch)
      {
      }
    }
}
