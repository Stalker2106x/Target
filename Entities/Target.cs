// Decompiled with JetBrains decompiler
// Type: Target.Target
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TargetGame.Settings;
using TargetGame.Utils;

namespace TargetGame.Entities
{
  /// <summary>
  /// Defines the Type/Location of a hit 
  /// </summary>
  public enum HitType
  {
    Miss,
    Catch,
    Hit,
    Headshot,
    Legshot,
    Critical
  }

  /// <summary>
  /// Holds spawn location and spawn chance
  /// </summary>
  public struct SpawnParameters
  {
    public enum Location
    {
      Any,
      RightBorder
    }

    public Location location;
    public int probability;
  }

  /// <summary>
  /// Holds correspondance between hitbox Color/HitType
  /// </summary>
  public struct Hitbox
  {
    public HitType red;
    public HitType green;
    public HitType blue;
  }

  /// <summary>
  /// Target class that represents spawnable entities
  /// </summary>
  public class Target : IEntity
  {
    /// <summary>
    /// Animation state of target
    /// </summary>
    public enum State
    {
      Idle,
      Firing
    }

    private string _name;
    public string name { get { return (_name); } set { _name = value; } }

    private State _state;

    private int _health;

    private int _damage;
    public int damage { get { return (_damage); } set { _damage = value; } }

    private int _score;
    public int score { get { return (_score); } set { _score = value; } }

    private int _fireDelay;
    public int fireDelay { get { return (_fireDelay); } set { _fireDelay = value; } }

    private Point _move;
    public Point move { get { return (_move); } set { _move = value; } }

    private Hitbox _hitbox;
    public Hitbox hitbox { get { return (_hitbox); } set { _hitbox = value; } }

    private int _lifetime;
    public int lifetime { get { return (_lifetime); } set { _lifetime = value; } }

    private SpawnParameters _spawn;
    public SpawnParameters spawn { get { return (_spawn); } set { _spawn = value; } }
    
    private static Random randomGenerator = new Random();
    private Point _position;
    private Timer _lifetimeTimer;

    private TargetResource _resource;
    public TargetResource resource {
      set
      {
        _resource = value;
      }
    }

    private Timer _attackTimer;
    private bool _active;

    public Target()
    {
      _state = State.Idle;
      _active = true;
    }

    public Target Copy()
    {
      return (Target)this.MemberwiseClone();
    }

    /**************************
     * IEntity implementation
     **************************/

    public void randomizePosition()
    {
      do
      {
        if (_spawn.location == SpawnParameters.Location.Any) _position.X = randomGenerator.Next(0, Options.Config.Width - getTexture().Width);
        else if (_spawn.location == SpawnParameters.Location.RightBorder) _position.X = randomGenerator.Next(Options.Config.Width - getTexture().Width, Options.Config.Width);
        _position.Y = randomGenerator.Next(0, Options.Config.Height - getTexture().Height);
      } while (GameMain.player.getWeapon().getRectangle().Contains(_position) || GameMain.hud.getTopLeftRectangle().Contains(_position));
    }

    public void setPosition(Point pos)
    {
      _position = pos;
    }

    public void activate()
    {
      if (_damage > 0)
      {
        _attackTimer = new Timer();
        _attackTimer.addAction(TimerDirection.Forward, _fireDelay, TimeoutBehaviour.StartOver, () => { fire(); });
        _attackTimer.addAction(TimerDirection.Forward, 250, TimeoutBehaviour.None, () => { _state = State.Idle; }); //Reset state
        _attackTimer.Start();
      }
      if (_lifetime > 0) //Target has a limited lifetime
      {
        _lifetimeTimer = new Timer();
        _lifetimeTimer.addAction(TimerDirection.Forward, _lifetime, TimeoutBehaviour.Stop, () => { _active = false; });
        _lifetimeTimer.Start();
      }
    }
    public Rectangle getRectangle()
    {
      return (new Rectangle(_position.X, _position.Y, getTexture().Width, getTexture().Height));
    }

    public bool getActivity()
    {
      return _active;
    }

    /**************************
     * Specific Methods
     **************************/

    private Texture2D getTexture()
    {
      switch (_state)
      {
        case State.Idle:
          return _resource.idle;
        case State.Firing:
          return _resource.firing;
      }
      return _resource.idle;
    }
    private Texture2D getHitbox()
    {
      switch (_state)
      {
        case State.Idle:
          return _resource.idle_hitbox;
        case State.Firing:
          return _resource.firing_hitbox;
      }
      return _resource.idle_hitbox;
    }

    public void addHealth(int value)
    {
      _health = value;
      if (_health <= 0)
      {
        _active = false;
        GameMain.player.addScore(_score);
      }
    }

    public HitType checkCollision(int damage)
    {
      Rectangle targetRect = getRectangle();
      if (!targetRect.Contains(GameMain.hud.crosshair.position.X, GameMain.hud.crosshair.position.Y)) return (HitType.Miss);
      Color[] hitColor = new Color[1];
      getHitbox().GetData<Color>(0, new Rectangle((int)GameMain.hud.crosshair.position.X - targetRect.X, (int)GameMain.hud.crosshair.position.Y - targetRect.Y, 1, 1), hitColor, 0, 1);
      if (hitColor[0].A == 0) return (HitType.Miss); //Transparent, no hit
      HitType hit;

      if (hitColor[0].R >= 255) //Headshot
      {
        addHealth(-(damage * 2));
        hit = _hitbox.red;
      }
      else if (hitColor[0].G >= 255) //Legshot
      {
        addHealth(-(int)(damage * 0.75));
        hit = _hitbox.green;
      }
      else
      {
        addHealth(-(int)(damage));
        hit = _hitbox.blue; //Regular
      }
      return (hit);
    }

    public void fire()
    {
      _state = State.Firing;
      Resources.burst.Play(Options.Config.SoundVolume, 0f, 0f);
      if (randomGenerator.Next(1, 3) == 1)
        Resources.pain1.Play(Options.Config.SoundVolume, 0f, 0f);
      else
        Resources.pain2.Play(Options.Config.SoundVolume, 0f, 0f);
      if (_damage > 0) GameMain.player.addHealth(-_damage);
    }

    /**************************
     * Base Update/Draw routines
     **************************/

    public void Update(GameTime gameTime)
    {
      _position.X += _move.X;
      _position.Y += _move.Y;
      if (_damage > 0) _attackTimer.Update(gameTime);
      if (lifetime > 0) _lifetimeTimer.Update(gameTime);
      if (!Utils.Tools.IsOnScreen(getRectangle())) _active = false; //Destroy out of screen stuff
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(getTexture(), getRectangle(), Color.White);
    }
  }
}
