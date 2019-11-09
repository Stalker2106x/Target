// Decompiled with JetBrains decompiler
// Type: Target.Target
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Target
{
  public enum TargetType
  {
    Recruit,
    Terrorist,
    Hostage,
  }
  
  public class Target
  {
    private MouseState mouseState;
    private int _hp;
    private Random _randomX;
    private Random _randomY;
    private Rectangle _sprite;
    private Texture2D _resource;
    private float _lifetime;
    private float _hurtDelay;
    private float _fireDelay;
    private bool _isActive;

    public Target(int hp)
    {
      _lifetime = 0.0f;
      _hurtDelay = 4f;
      _fireDelay = 1f;
      _isActive = true;
      _randomX = new Random();
      _randomY = new Random();
      _hp = hp;
      _sprite = new Rectangle(_randomX.Next(0, Options.Config.Width - Resources.soldier_idle.Width), _randomX.Next(0, Options.Config.Height - Resources.soldier_idle.Height), Resources.soldier_idle.Width, Resources.soldier_idle.Height);
      _resource = Resources.soldier_idle;
    }

    public bool getActivity()
    {
      return _isActive;
    }

    public void checkCollision()
    {
      if (!_sprite.Contains((int)HUD._target.X, (int)HUD._target.Y)) return;
      Color[] hitColor = new Color[1];
      Resources.soldier_idle.GetData<Color>(0, new Rectangle((int)HUD._target.X - _sprite.X, (int)HUD._target.Y - _sprite.Y, 1, 1), hitColor, 0, 1);
      
      if (hitColor[0].A == 0) return; //Transparent, no hit
      GameMain._player.setScore(20);
      GameMain._player.setBulletsHit(1);
      _isActive = false;
    }

    public void updateHurt(Player player, GameTime gameTime)
    {
      _lifetime += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (_lifetime < _hurtDelay) return;
      _lifetime = 0.0f;
      _resource = Resources.soldier_firing;
      Resources.burst.Play(Options.Config.SoundVolume, 0f, 0f);
      GameMain.hud.setBloodsplat();
      if (_randomX.Next(1, 3) == 1)
        Resources.pain1.Play(Options.Config.SoundVolume, 0f, 0f);
      else
        Resources.pain2.Play(Options.Config.SoundVolume, 0f, 0f);
      player.setHealth(-10);
    }

    public void Update(Player player, GameTime gameTime, MouseState mouse)
    {
      mouseState = mouse;
      updateHurt(player, gameTime);
      if (_lifetime > _fireDelay)
      {
         _resource = Resources.soldier_idle;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_resource, _sprite, Color.White);
    }
  }
}
