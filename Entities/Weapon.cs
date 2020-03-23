// Decompiled with JetBrains decompiler
// Type: Target.Weapon
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    private int _magazine;
    private int _maxMagazine;
    private WeaponState _state;
    private Timer _reloadTimer;
    private float _fireDelay;

    public Weapon(string name, int maxMagazine)
    {
      _name = name;
      _maxMagazine = maxMagazine;
      _magazine = _maxMagazine;
      _state = WeaponState.Idle;
      _reloadTimer = new Timer();
      _fireDelay = 250f;
      _reloadTimer.addAction(TimerDirection.Forward, 1500, TimeoutBehaviour.Reset, () => { });
    }

    public int getMagazine()
    {
      return _magazine;
    }

    public int getMaxMagazine()
    {
      return _maxMagazine;
    }

    public void setMagazine(int bullets)
    {
      _magazine += bullets;
    }

    public WeaponState getState()
    {
      return _state;
    }

    public double getStatusTimer()
    {
      return _reloadTimer.getDuration();
    }


    public void fire(GameTime gameTime, MouseState mouse)
    {
      if (_magazine <= 0) reload();
      if (_reloadTimer.isActive()) return;
      _magazine--;
      GameMain._player.setBulletsFired(1);
      GameMain.hud.setRecoil();
      Resources.fire.Play(Options.Config.SoundVolume, 0f, 0f);
      for (int index = 0; index < GameMain._targets.Count; ++index)
        GameMain._targets[index].checkCollision();
      for (int index = 0; index < GameMain._items.Count; ++index)
        GameMain._items[index].checkCollision();
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
