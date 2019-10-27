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
    private string m_name;
    private int m_magazine;
    private int m_maxMagazine;
    private WeaponState m_weaponState;
    private float timer;
    private float reloadDelay;
    private float fireDelay;
    private bool m_activeBonus;
    private float bonusTimer;

    public Weapon(string name, int maxMagazine)
    {
      this.m_activeBonus = false;
      this.m_name = name;
      this.m_maxMagazine = maxMagazine;
      this.m_magazine = this.m_maxMagazine;
      this.m_weaponState = WeaponState.Available;
      this.timer = 0.0f;
      this.reloadDelay = 1500f;
      this.fireDelay = 250f;
    }

    public int getMagazine()
    {
      return this.m_magazine;
    }

    public int getMaxMagazine()
    {
      return this.m_maxMagazine;
    }

    public void setMagazine(int bullets)
    {
      this.m_magazine += bullets;
    }

    public WeaponState getState()
    {
      return this.m_weaponState;
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
      if (!this.m_activeBonus)
        return;
      this.bonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.bonusTimer < 20.0)
        return;
      this.bonusTimer = 0.0f;
      this.m_activeBonus = false;
    }

    public void updateBonus(GameTime gameTime)
    {
      if (!this.m_activeBonus)
        return;
      this.bonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.bonusTimer < 20.0)
        return;
      this.bonusTimer = 0.0f;
      this.m_activeBonus = false;
    }

    public void fire(GameTime gameTime, MouseState mouse)
    {
      if (this.m_magazine <= 0 || this.m_weaponState != WeaponState.Available)
        return;
      --this.m_magazine;
      GameMain.m_player.setBulletsFired(1);
      this.m_weaponState = WeaponState.Firing;
      GameMain.m_hud.setRecoil();
      Resources.fire.Play(0.5f, 0.0f, 0.0f);
      for (int index = 0; index < GameMain.m_targets.Count; ++index)
        GameMain.m_targets[index].checkCollision();
      for (int index = 0; index < GameMain.m_items.Count; ++index)
        GameMain.m_items[index].checkCollision();
    }

    public void reload()
    {
      if (this.m_magazine >= this.m_maxMagazine || this.m_weaponState != WeaponState.Available)
        return;
      Resources.reload.Play();
      this.m_magazine = this.m_maxMagazine;
      this.m_weaponState = WeaponState.Reloading;
    }

    public void updateState(GameTime gameTime)
    {
      if (this.m_weaponState != WeaponState.Available)
        this.timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this.m_weaponState == WeaponState.Reloading)
      {
        if ((double) this.timer < (double) this.reloadDelay)
          return;
        this.timer = 0.0f;
        this.m_weaponState = WeaponState.Available;
      }
      else
      {
        if (this.m_weaponState != WeaponState.Firing || (double) this.timer < (double) this.fireDelay)
          return;
        this.timer = 0.0f;
        this.m_weaponState = WeaponState.Available;
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
