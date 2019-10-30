// Decompiled with JetBrains decompiler
// Type: Target.Weapon
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Target
{
  public class Weapon
  {
    private string _name;
    private int _magazine;
    private int _maxMagazine;
    private WeaponState _weaponState;
    private float timer;
    private float reloadDelay;
    private float fireDelay;
    private bool _activeBonus;
    private float bonusTimer;

    public Weapon(string name, int maxMagazine)
    {
      _activeBonus = false;
      _name = name;
      _maxMagazine = maxMagazine;
      _magazine = _maxMagazine;
      _weaponState = WeaponState.Available;
      timer = 0.0f;
      reloadDelay = 1500f;
      fireDelay = 250f;
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
      return _weaponState;
    }

    public float getReloadDelay()
    {
      return reloadDelay;
    }

    public void setReloadDelay(float delay)
    {
      reloadDelay += delay;
    }

    public float getTimer()
    {
      return timer;
    }

    public void updateBonusTimer(GameTime gameTime)
    {
      if (!_activeBonus)
        return;
      bonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) bonusTimer < 20.0)
        return;
      bonusTimer = 0.0f;
      _activeBonus = false;
    }

    public void updateBonus(GameTime gameTime)
    {
      if (!_activeBonus)
        return;
      bonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) bonusTimer < 20.0)
        return;
      bonusTimer = 0.0f;
      _activeBonus = false;
    }

    public void fire(GameTime gameTime, MouseState mouse)
    {
      if (_magazine <= 0 || _weaponState != WeaponState.Available)
        return;
      _magazine--;
      GameMain._player.setBulletsFired(1);
      _weaponState = WeaponState.Firing;
      GameMain.hud.setRecoil();
      Resources.fire.Play(0.5f, 0.0f, 0.0f);
      for (int index = 0; index < GameMain._targets.Count; ++index)
        GameMain._targets[index].checkCollision();
      for (int index = 0; index < GameMain._items.Count; ++index)
        GameMain._items[index].checkCollision();
    }

    public void reload()
    {
      if (_magazine >= _maxMagazine || _weaponState != WeaponState.Available)
        return;
      Resources.reload.Play();
      _magazine = _maxMagazine;
      _weaponState = WeaponState.Reloading;
    }

    public void updateState(GameTime gameTime)
    {
      if (_weaponState != WeaponState.Available)
        timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if (_weaponState == WeaponState.Reloading)
      {
        if ((double) timer < (double) reloadDelay)
          return;
        timer = 0.0f;
        _weaponState = WeaponState.Available;
      }
      else
      {
        if (_weaponState != WeaponState.Firing || (double) timer < (double) fireDelay)
          return;
        timer = 0.0f;
        _weaponState = WeaponState.Available;
      }
    }

    public void Update(GameTime gameTime)
    {
      updateBonusTimer(gameTime);
      updateBonus(gameTime);
      updateState(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
