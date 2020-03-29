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
  public class Weapon
  {
    private string _name;
    private int _damage;
    private int _magazine;
    private int _maxMagazine;
    private int _reloadTimerActionIndex;
    private Timer _reloadTimer;
    private Texture2D _texture;

    private bool _muzzleFlash;
    private Texture2D _muzzleFlashTexture;
    private Timer _muzzleFlashTimer;

    public Weapon(string name, int maxMagazine)
    {
      _name = name;
      _maxMagazine = maxMagazine;
      _damage = 100;
      _magazine = _maxMagazine;
      _reloadTimer = new Timer();
      _reloadTimerActionIndex = _reloadTimer.addAction(TimerDirection.Forward, 1500, TimeoutBehaviour.Reset, () => { });
      _muzzleFlashTimer = new Timer();
      _muzzleFlashTimer.addAction(TimerDirection.Forward, 100, TimeoutBehaviour.Reset, () => { _muzzleFlash = false; });
      _muzzleFlash = false;
      _texture = Resources.hands;
      _muzzleFlashTexture = Resources.muzzleflash;
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

    public double getStatusTimer()
    {
      return _reloadTimer.getDuration();
    }

    public Rectangle getMuzzleFlashRectangle()
    {
      var rect = getRectangle();
      rect.X += 90;
      rect.Y -= 20;
      rect.Width = _muzzleFlashTexture.Width;
      rect.Height = _muzzleFlashTexture.Height;
      return (rect);
    }

    public Rectangle getRectangle()
    {
      return (new Rectangle((int)(Options.Config.Width * 0.70f) + (GameMain.hud.crosshair.position.X / 10), (int)(Options.Config.Height * 0.70f) + (GameMain.hud.crosshair.position.Y / 10), _texture.Width, _texture.Height));
    }

    public List<HitType> fire()
    {
      if (_magazine <= 0) reload();
      if (_reloadTimer.isActive()) return (null);
      List<HitType> hits = new List<HitType>();

      for (int index = 0; index < GameMain.items.Count; ++index)
      {
        HitType hit = GameMain.items[index].checkCollision();
        if (hit != HitType.Miss)
        {
          hits.Add(hit); //Add hit if different from miss
          return (hits); //return immediately, its a catch, we dont want to fire
        }
      }
      triggerMuzzleFlash();
      for (int index = 0; index < GameMain.targets.Count; ++index)
      {
        HitType hit = GameMain.targets[index].checkCollision(_damage);
        if (hit != HitType.Miss) hits.Add(hit); //Add hit if different from miss
      }
      if (hits.Count == 0) hits.Add(HitType.Miss); //No hit, add miss
        
      _magazine--;
      GameMain.player.addBulletsFired(1);
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
      _muzzleFlashTimer.Update(gameTime);
    }

    public void triggerMuzzleFlash()
    {
      _muzzleFlashTimer.Start();
      _muzzleFlash = true;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (_muzzleFlash) spriteBatch.Draw(_muzzleFlashTexture, getMuzzleFlashRectangle(), Color.White);
      spriteBatch.Draw(_texture, getRectangle(), Color.White);
    }
  }
}
