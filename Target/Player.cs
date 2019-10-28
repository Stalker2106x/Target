// Decompiled with JetBrains decompiler
// Type: Target.Player
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Target
{
  internal class Player
  {
    private int m_hp;
    private int m_maxHp;
    private int m_score;
    private bool m_breath;
    private bool m_recover;
    private float breathTimer;
    private int m_bulletsFired;
    private int m_bulletsHit;
    private Weapon m_weapon;

    public Player()
    {
      this.m_breath = true;
      this.m_recover = false;
      this.breathTimer = 0.0f;
      this.m_hp = 100;
      this.m_maxHp = 100;
      this.m_bulletsFired = 0;
      this.m_bulletsHit = 0;
      this.m_weapon = new Weapon("Barett .50", 15);
    }

    public int getHealth()
    {
      return this.m_hp;
    }

    public int getMaxHealth()
    {
      return this.m_maxHp;
    }

    public int getScore()
    {
      return this.m_score;
    }

    public bool getBreath()
    {
      return this.m_breath;
    }

    public float getBreathTimer()
    {
      return this.breathTimer;
    }

    public float getAccuracy()
    {
      return (float) ((double) this.m_bulletsHit / (double) this.m_bulletsFired * 100.0);
    }

    public int getBulletsFired()
    {
      return this.m_bulletsFired;
    }

    public void setScore(int score)
    {
      this.m_score += score;
    }

    public void setHealth(int hp)
    {
      this.m_hp += hp;
    }

    public void setBulletsFired(int bullet)
    {
      this.m_bulletsFired += bullet;
    }

    public void setBulletsHit(int bullet)
    {
      this.m_bulletsHit += bullet;
    }

    public Weapon getWeapon()
    {
      return this.m_weapon;
    }

    public void healthCap()
    {
      if (this.m_hp <= this.m_maxHp)
        return;
      this.m_hp = this.m_maxHp;
    }

    public void Update(
      GameTime gameTime,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if (keyboard.IsKeyDown(Keys.LeftShift) || gamePad.IsButtonDown(Buttons.LeftShoulder))
      {
        this.m_recover = true;
        if ((double) this.breathTimer <= 2500.0)
        {
          this.breathTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
          if (oldKeyboard.IsKeyUp(Keys.LeftShift) && oldGamePad.IsButtonUp(Buttons.LeftShoulder))
            Resources.breath.Play();
          this.m_breath = false;
        }
        else
          this.m_breath = true;
      }
      else
      {
        this.m_breath = true;
        if (this.m_recover)
        {
          if ((double) this.breathTimer >= 0.0)
            this.breathTimer -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
          else
            this.m_recover = false;
        }
      }
      if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released || gamePad.IsButtonDown(Buttons.RightTrigger))
        this.m_weapon.fire(gameTime, mouse);
      else if (keyboard.IsKeyDown(Keys.R) && oldKeyboard.IsKeyUp(Keys.R) || gamePad.IsButtonDown(Buttons.X))
        this.m_weapon.reload();
      this.healthCap();
      this.m_weapon.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
