using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Target.Utils;

namespace Target.Entities
{
  public class Bomb
  {
    private static Random _randomGenerator = new Random();

    private bool _active;
    private Point _position;
    private Texture2D _texture;
    private Timer _durationTimer;
    private HorizontalProgressBar _indicator;

    public Bomb()
    {
      _active = true;
      _position = new Point(_randomGenerator.Next(0, Options.Config.Width), _randomGenerator.Next(0, Options.Config.Height));
      _texture = Resources.bomb;
      _durationTimer = new Timer();
      _durationTimer.addAction(TimerDirection.Backward, -1, TimeoutBehaviour.Stop, () => { Defuse(); });
      _durationTimer.addAction(TimerDirection.Forward, 10000, TimeoutBehaviour.Stop, () => { Explode(); });
      _durationTimer.Start();
      _indicator = GameMain.hud.addBombIndicator(_position);
      _indicator.Maximum = 10000;
      _indicator.Minimum = -1;
      _indicator.Value = 0;
    }

    public bool getActivity()
    {
      return _active;
    }

    public void Destroy()
    {
      _active = false;
      GameMain.hud.removeBombIndicator(_indicator);
    }

    public void Defuse()
    {
      Destroy();
      GameMain._player.setDefuser(false);
    }

    public void Explode()
    {
      Destroy();
    }

    public Rectangle getRectangle()
    {
      return (new Rectangle(_position.X, _position.Y, _texture.Width, _texture.Height));
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      if (GameMain.hud.crosshair.checkCollision(getRectangle()))
      {
        if (Options.Config.Bindings[GameAction.Defuse].IsControlPressed(state, prevState)) //First time
        {
          _durationTimer.Reverse();
          if (GameMain._player.hasDefuser()) _durationTimer.setTimeScale(3);
          else _durationTimer.setTimeScale(2);
        }
        else if (!Options.Config.Bindings[GameAction.Defuse].IsControlDown(state) && _durationTimer.getDirection() != TimerDirection.Forward) //Not holding
        {
          _durationTimer.Reverse();
          _durationTimer.setTimeScale(1);
        }
      }
      else if (_durationTimer.getDirection() != TimerDirection.Forward)
      {
        _durationTimer.Reverse();
        _durationTimer.setTimeScale(1);
      }
      _durationTimer.Update(gameTime);
      _indicator.Value = (float)_durationTimer.getDuration();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, getRectangle(), Color.White);
    }
  }
}
