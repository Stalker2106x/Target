// Decompiled with JetBrains decompiler
// Type: Target.Player
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    private bool _forceRecover;

    private BreathState _breathState;
    private Timer _breathTimer;

    private float _scoreMultiplier;
    public float scoreMultiplier { get { return (_scoreMultiplier); } set { _scoreMultiplier = value; } }

    private int _comboHeadshot;
    private int _bulletsFired;
    private int _bulletsHit;
    private Weapon _weapon;
    private SoundEffectInstance _heartbeat;

    public Player()
    {
      _breathState = BreathState.Breathing;
      _forceRecover = false;
      _breathTimer = new Timer();
      _hp = 100;
      _maxHp = 100;
      _comboHeadshot = 0;
      _bulletsFired = 0;
      _bulletsHit = 0;
      _weapon = new Weapon("P250", 15);
      _heartbeat = Resources.heartbeat.CreateInstance();
      _heartbeat.Volume = Options.Config.SoundVolume;
      _heartbeat.IsLooped = true;

      _breathTimer.addAction(TimerDirection.Forward, 2501, TimeoutBehaviour.None, () =>
      {
        Resources.outbreath.Play(Options.Config.SoundVolume, 0f, 0f);
        _breathState = BreathState.ForceRecovery;
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

    public void resetComboHeadshot(int headshot)
    {
      _comboHeadshot = 0;
    }

    public void setComboHeadshot(int headshot)
    {
      _comboHeadshot += headshot;
      if (_comboHeadshot % 10 == 0)
      {
        Resources.headhunter.Play(Options.Config.SoundVolume, 0f, 0f);
        GameMain.hud.setAction("HEAD HUNTER !");
        _scoreMultiplier += 0.5f;
        GameMain.hud.scoreIndicator.Text = "Combo x" + _scoreMultiplier;
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
      //Breath management
      if (_breathState != BreathState.ForceRecovery && (keyboard.IsKeyDown(Keys.LeftShift) || gamePad.IsButtonDown(Buttons.LeftShoulder)))
      {
        if (_breathTimer.getDuration() == 0
          || oldKeyboard.IsKeyUp(Keys.LeftShift) && oldGamePad.IsButtonUp(Buttons.LeftShoulder)) //First time
        {
          Resources.breath.Play(Options.Config.SoundVolume, 0f, 0f);
          _heartbeat.Play();
        }
        if (!_breathTimer.isActive()) _breathTimer.Start();
        if (_breathTimer.getDirection() != TimerDirection.Forward) _breathTimer.Reverse();
        _breathState = BreathState.Holding;
      }
      else
      {
        if (_breathState != BreathState.ForceRecovery)
        {
          if (_breathTimer.getDirection() != TimerDirection.Backward) _breathTimer.Reverse();
          _breathState = BreathState.Breathing;
        }
        _heartbeat.Stop();
      }
      _breathTimer.Update(gameTime);

      //Fire management
      if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released || gamePad.IsButtonDown(Buttons.RightTrigger))
        _weapon.fire(gameTime, mouse);
      if (keyboard.IsKeyDown(Keys.R) && oldKeyboard.IsKeyUp(Keys.R) || gamePad.IsButtonDown(Buttons.X))
        _weapon.reload();
      healthCap();
      _weapon.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
