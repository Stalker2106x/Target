using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;
using System.Collections.Generic;
using System.Text;
using Target.Utils;

namespace Target.Entities
{
  public class Bomb
  {
    private const int _bombDuration = 5000;

    private static Random _randomGenerator = new Random();

    private bool _active;
    private Point _position;
    private Texture2D _texture;

    private HorizontalProgressBar _indicator;

    private Timer _durationTimer;
    private Timer _soundTimer;

    public Bomb()
    {
      _active = true;
      _texture = Resources.bomb;

      _durationTimer = new Timer();
      _durationTimer.addAction(TimerDirection.Backward, -1, TimeoutBehaviour.Stop, () => { CompleteDefusal(); });
      _durationTimer.addAction(TimerDirection.Forward, _bombDuration, TimeoutBehaviour.Stop, () => { Explode(); });
      _soundTimer = new Timer();
      _soundTimer.addAction(TimerDirection.Forward, 1000, TimeoutBehaviour.StartOver, () => { Resources.bombtick.Play(Options.Config.SoundVolume, 0f, 0f); });
    }

    public void activate()
    {
      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, new Color(Color.LightGray, 0));
      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.LightGray);
      Stylesheet.Current.HorizontalProgressBarStyle.Filler = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Red);
      _indicator = new HorizontalProgressBar();
      _indicator.Minimum = -1;
      _indicator.Maximum = _bombDuration;
      _indicator.Left = (int)(_position.X - (_texture.Width * 0.25));
      _indicator.Top = _position.Y;
      _indicator.Width = (int)(_texture.Width * 1.5);
      _indicator.Height = 10;
      GameMain.hud.addIndicator(_indicator);
      _soundTimer.Start();
      _durationTimer.Start();
    }

    public void randomizePosition()
    {
      _position = new Point(_randomGenerator.Next(0, Options.Config.Width - getRectangle().Width), _randomGenerator.Next(0, (Options.Config.Height - getRectangle().Height)));
    }

    public void setPosition(Point pos)
    {
      _position = pos;
    }

    public bool getActivity()
    {
      return _active;
    }

    public void Destroy()
    {
      _active = false;
      GameMain.hud.removeIndicator(_indicator);
      GameMain.player.defusing = false;
    }

    public void CompleteDefusal()
    {
      Destroy();
      Resources.defusal.Play(Options.Config.SoundVolume, 0f, 0f);
      GameMain.player.setDefuser(false);
      //GameMain._player.stats.bombsDefused += 1;
    }

    public void Explode()
    {
      Resources.explosion.Play(Options.Config.SoundVolume, 0f, 0f);
      Destroy();
      GameMain.player.addScore(-500);
      GameMain.player.addHealth(-50, true);
    }

    public void Defuse()
    {
      GameMain.player.defusing = true;
      _soundTimer.Reset();
      _durationTimer.Reverse();
      if (GameMain.player.hasDefuser()) _durationTimer.setTimeScale(4);
      else _durationTimer.setTimeScale(3);
    }
    public void Rearm()
    {
      GameMain.player.defusing = false;
      _soundTimer.Start();
      _durationTimer.Reverse();
      _durationTimer.setTimeScale(1);
    }

    public Rectangle getRectangle()
    {
      return (new Rectangle(_position.X, _position.Y, _texture.Width, _texture.Height));
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      if (GameMain.hud.crosshair.checkCollision(getRectangle()))
      {
        if (Options.Config.Bindings[GameAction.Defuse].IsControlPressed(state, prevState)) Defuse(); //User started defusing
        else if (!Options.Config.Bindings[GameAction.Defuse].IsControlDown(state) && _durationTimer.getDirection() != TimerDirection.Forward) Rearm(); //Not holding, rearm
        //Fall here if holding, nothing happens, let the timer go out
      }
      else if (_durationTimer.getDirection() != TimerDirection.Forward) Rearm(); //None, rearm
      _soundTimer.Update(gameTime);
      _durationTimer.Update(gameTime);
      _indicator.Value = (float)_durationTimer.getDuration();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, getRectangle(), Color.White);
    }
  }
}
