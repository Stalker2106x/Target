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
  internal class Weapon
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
      this._activeBonus = false;
      this._name = name;
      this._maxMagazine = maxMagazine;
      this._magazine = this._maxMagazine;
      this._weaponState = WeaponState.Available;
      this.timer = 0.0f;
      this.reloadDelay = 1500f;
      this.fireDelay = 250f;
    }

    public int getMagazine()
    {
      return this._magazine;
    }

    public int getMaxMagazine()
    {
      return this._maxMagazine;
    }

    public void setMagazine(int bullets)
    {
      this._magazine += bullets;
    }

    public WeaponState getState()
    {
      return this._weaponState;
    }

    public float getReloadDelay()
    {
      return this.reloadDelay;
    }

    public void setReloadDelay(float delay)
    {
      this.reloadDelay += delay;
    }

    public float getTimer()
    {
      return this.timer;
    }

    public void updateBonusTimer(GameTime gameTime)
    {
      if (!this._activeBonus)
        return;
      this.bonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.bonusTimer < 20.0)
        return;
      this.bonusTimer = 0.0f;
      this._activeBonus = false;
    }

    public void updateBonus(GameTime gameTime)
    {
      if (!this._activeBonus)
        return;
      this.bonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.bonusTimer < 20.0)
        return;
      this.bonusTimer = 0.0f;
      this._activeBonus = false;
    }

    public void fire(GameTime gameTime, MouseState mouse)
    {
      if (this._magazine <= 0 || this._weaponState != WeaponState.Available)
        return;
      --this._magazine;
      GameMain._player.setBulletsFired(1);
      this._weaponState = WeaponState.Firing;
      GameMain._hud.setRecoil();
      Resources.fire.Play(0.5f, 0.0f, 0.0f);
      for (int index = 0; index < GameMain._targets.Count; ++index)
        GameMain._targets[index].checkCollision();
      for (int index = 0; index < GameMain._items.Count; ++index)
        GameMain._items[index].checkCollision();
    }

    public void reload()
    {
      if (this._magazine >= this._maxMagazine || this._weaponState != WeaponState.Available)
        return;
      Resources.reload.Play();
      this._magazine = this._maxMagazine;
      this._weaponState = WeaponState.Reloading;
    }

    public void updateState(GameTime gameTime)
    {
      if (this._weaponState != WeaponState.Available)
        this.timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._weaponState == WeaponState.Reloading)
      {
        if ((double) this.timer < (double) this.reloadDelay)
          return;
        this.timer = 0.0f;
        this._weaponState = WeaponState.Available;
      }
      else
      {
        if (this._weaponState != WeaponState.Firing || (double) this.timer < (double) this.fireDelay)
          return;
        this.timer = 0.0f;
        this._weaponState = WeaponState.Available;
      }
    }

    public void Update(GameTime gameTime)
    {
      this.updateBonusTimer(gameTime);
      this.updateBonus(gameTime);
      this.updateState(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
