using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TargetGame.Settings;
using TargetGame.Utils;

namespace TargetGame
{
  public class Crosshair
  {
    public Point position;

    //Hitmarker
    bool _drawHitmarker;
    private Timer _hitmarkerTimer;

    private Random _randomGenerator;

    private const int _resetThreshold = 50;
    private int _framesToSkip; //Skip frames for cursor management

    //Sway
    private Timer _swayTimer;
    private Timer _swayVectorTimer;
    private Point _swayVector;

    //Recoil
    private Timer _recoilTimer;
    private Timer _recoilVectorTimer;
    private int _recoilY;

    public Crosshair()
    {
      _randomGenerator = new Random();
      position = new Point(Options.Config.Width / 2, Options.Config.Height / 2);
      initSway();
      initRecoil();
      _drawHitmarker = false;
      _hitmarkerTimer = new Timer();
      _hitmarkerTimer.addAction(TimerDirection.Forward, 500, TimeoutBehaviour.Reset, () => { _drawHitmarker = false; });
    }

    public void initSway()
    {
      _swayTimer = new Timer();
      _swayTimer.addAction(TimerDirection.Forward, 50, TimeoutBehaviour.StartOver, () => {
        if (_swayVector.X >= 0) position.X += _randomGenerator.Next(-1, (_swayVector.X + 2));
        else position.X -= _randomGenerator.Next(-1, (_swayVector.X * -1) + 2);
        if (_swayVector.Y >= 0) position.Y += _randomGenerator.Next(-1, (_swayVector.Y + 2));
        else position.Y -= _randomGenerator.Next(-1, (_swayVector.Y * -1) + 2);
      });
      _swayTimer.Start();

      _swayVectorTimer = new Timer();
      _swayVectorTimer.addAction(TimerDirection.Forward, 1, TimeoutBehaviour.None, () => { _swayVector = new Point(1, -1); });
      _swayVectorTimer.addAction(TimerDirection.Forward, 2000, TimeoutBehaviour.None, () => { _swayVector = new Point(0, 1); });
      _swayVectorTimer.addAction(TimerDirection.Forward, 4000, TimeoutBehaviour.None, () => { _swayVector = new Point(-1, -1); });
      _swayVectorTimer.addAction(TimerDirection.Forward, 6000, TimeoutBehaviour.None, () => { _swayVector = new Point(0, 1); });
      _swayVectorTimer.addAction(TimerDirection.Forward, 8000, TimeoutBehaviour.StartOver, () => { });
      _swayVectorTimer.Start();
    }

    public void setSway(bool enabled)
    {
      if (enabled)
      {
        _swayTimer.Start();
        _swayVectorTimer.Start();
      }
      else
      {
        _swayTimer.Stop();
        _swayVectorTimer.Stop();
      }
    }

    public void initRecoil()
    {
      _recoilTimer = new Timer();
      _recoilTimer.addAction(TimerDirection.Forward, 5, TimeoutBehaviour.StartOver, () => {
        if (_recoilY >= 0) position.Y += _randomGenerator.Next(-1, (_recoilY + 2));
        else position.Y -= _randomGenerator.Next(-1, (_recoilY * -1) + 2);
      });

      _recoilVectorTimer = new Timer();
      _recoilVectorTimer.addAction(TimerDirection.Forward, 1, TimeoutBehaviour.None, () => { _recoilY = -5; });
      _recoilVectorTimer.addAction(TimerDirection.Forward, 250, TimeoutBehaviour.None, () => { _recoilY = 3; });
      _recoilVectorTimer.addAction(TimerDirection.Forward, 500, TimeoutBehaviour.Reset, () => { _recoilTimer.Stop(); });
    }

    public bool checkCollision(Rectangle toCheck)
    {
      return (getRectangle().Intersects(toCheck));
    }

    public void triggerRecoil()
    {
      _recoilTimer.StartOver();
      _recoilVectorTimer.StartOver();
    }
    public void triggerHitmarker()
    {
      _drawHitmarker = true;
      Resources.hit.Play(Options.Config.SoundVolume, 0f, 0f);
      _hitmarkerTimer.StartOver();
    }

    public Texture2D getTexture()
    {
      if (GameMain.player.getWeapon().getStatusTimer() > 0)
      {
        return (Resources.reloadingCursor);
      }
      else if(GameMain.player.isDefusing())
      {
        return (Resources.defuseCursor);
      }
      foreach (var it in GameMain.items)
      {
        if (checkCollision(it.getRectangle())) return (Resources.grabCursor);
      }
      return (Resources.crosshair);
    }

    public Rectangle getRectangle()
    {
      return (new Rectangle(position.X - 16, position.Y - 16, 32, 32));
    }
    public Rectangle getHitmarkerRectangle()
    {
      return (new Rectangle(position.X - 32, position.Y - 32, 64, 64));
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      if (_framesToSkip > 0)
      {
        _framesToSkip--;
        return;
      }
      if (state.mouse.X < _resetThreshold || state.mouse.X > Options.Config.Width - _resetThreshold || state.mouse.Y < _resetThreshold || state.mouse.Y > Options.Config.Height - _resetThreshold)
      {
        Mouse.SetPosition(Options.Config.Width / 2, Options.Config.Height / 2);
        _framesToSkip = 3;
      }
      position.X += (int)((state.mouse.X - prevState.mouse.X) * Options.Config.MouseSensivity);
      position.Y += (int)((state.mouse.Y - prevState.mouse.Y) * Options.Config.MouseSensivity);
      if (position.X < 0) position.X = 0;
      else if (position.X > Options.Config.Width) position.X = Options.Config.Width;
      else if (position.Y < 0) position.Y = 0;
      else if (position.Y > Options.Config.Height) position.Y = Options.Config.Height;
      _swayTimer.Update(gameTime);
      _swayVectorTimer.Update(gameTime);
      _recoilTimer.Update(gameTime);
      _recoilVectorTimer.Update(gameTime);
      _hitmarkerTimer.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(getTexture(), getRectangle(), Color.White);
      if (_drawHitmarker) spriteBatch.Draw(Resources.hitmarker, getHitmarkerRectangle(), Color.White);
    }
  }
}
