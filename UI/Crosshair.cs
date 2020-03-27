using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Target.Utils;

namespace Target
{
  public class Crosshair
  {

    public Vector2 position;

    private Random _randomGenerator;

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
      position = new Vector2(Options.Config.Width / 2, Options.Config.Height / 2);
      initSway();
      initRecoil();
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

    public void triggerRecoil()
    {
      _recoilTimer.StartOver();
      _recoilVectorTimer.StartOver();
    }
    
    public Texture2D getTexture()
    {
      if (GameMain._player.getWeapon().getState() == WeaponState.Reloading)
      {
        return (Resources.reloadingCursor);
      }
      return (Resources.crosshair);
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      if (_framesToSkip > 0)
      {
        _framesToSkip--;
        return;
      }
      if (state.mouse.X < 0 || state.mouse.X > Options.Config.Width || state.mouse.Y < 0 || state.mouse.Y > Options.Config.Height)
      {
        Mouse.SetPosition(Options.Config.Width / 2, Options.Config.Height / 2);
        _framesToSkip = 1;
      }
      position.X += state.mouse.X - prevState.mouse.X;
      position.Y += state.mouse.Y - prevState.mouse.Y;
      _swayTimer.Update(gameTime);
      _swayVectorTimer.Update(gameTime);
      _recoilTimer.Update(gameTime);
      _recoilVectorTimer.Update(gameTime);
    }

    public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(getTexture(), new Rectangle((int)position.X - Resources.crosshair.Width / 2, (int)position.Y - Resources.crosshair.Height / 2, Resources.crosshair.Width, Resources.crosshair.Height), Color.White);
    }
  }
}
