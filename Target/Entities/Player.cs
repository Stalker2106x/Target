// Decompiled with JetBrains decompiler
// Type: Target.Player
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Target
{
  public class Player
  {
    private int _hp;
    private int _maxHp;
    private int _score;
    private bool _breath;
    private bool _forceRecover;
    private float breathTimer;
    private int _bulletsFired;
    private int _bulletsHit;
    private Weapon _weapon;
    private SoundEffectInstance _heartbeat;

    public Player()
    {
      _breath = true;
      _forceRecover = false;
      breathTimer = 0.0f;
      _hp = 100;
      _maxHp = 100;
      _bulletsFired = 0;
      _bulletsHit = 0;
      _weapon = new Weapon("Barett .50", 15);
      _heartbeat = Resources.heartbeat.CreateInstance();
      _heartbeat.Volume = Options.Config.SoundVolume;
      _heartbeat.IsLooped = true;
    }

    public int getHealth()
    {
      return _hp;
    }

    public int getMaxHealth()
    {
      return _maxHp;
    }

    public int getScore()
    {
      return _score;
    }

    public bool getBreath()
    {
      return _breath;
    }

    public float getBreathTimer()
    {
      return breathTimer;
    }

    public float getAccuracy()
    {
      return (float) ((double) _bulletsHit / (double) _bulletsFired * 100.0);
    }

    public int getBulletsFired()
    {
      return _bulletsFired;
    }

    public void setScore(int score)
    {
      _score += score;
    }

    public void setHealth(int hp)
    {
      _hp += hp;
    }

    public void setBulletsFired(int bullet)
    {
      _bulletsFired += bullet;
    }

    public void setBulletsHit(int bullet)
    {
      _bulletsHit += bullet;
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

    public void Update(
      GameTime gameTime,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if ((keyboard.IsKeyDown(Keys.LeftShift) || gamePad.IsButtonDown(Buttons.LeftShoulder)) && !_forceRecover && (double)breathTimer <= 2500.0)
      {
        if (oldKeyboard.IsKeyUp(Keys.LeftShift) && oldGamePad.IsButtonUp(Buttons.LeftShoulder)) //First time
        {
            breathTimer += 250f;
            Resources.breath.Play(Options.Config.SoundVolume, 0f, 0f);
            _heartbeat.Play();
        }
        _breath = false;
        breathTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (breathTimer > 2500.0) //Too much
        {
          _breath = true;
          _forceRecover = true;
        }
      }
      else
      {
        _heartbeat.Stop();
        _breath = true;
        if ((double)breathTimer >= 0.0)
            breathTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        else if (_forceRecover) //done recovering
            _forceRecover = false;
      }
      if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released || gamePad.IsButtonDown(Buttons.RightTrigger))
        _weapon.fire(gameTime, mouse);
      else if (keyboard.IsKeyDown(Keys.R) && oldKeyboard.IsKeyUp(Keys.R) || gamePad.IsButtonDown(Buttons.X))
        _weapon.reload();
      healthCap();
      _weapon.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
