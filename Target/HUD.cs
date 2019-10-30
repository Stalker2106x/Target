// Decompiled with JetBrains decompiler
// Type: Target.HUD
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Target
{
  class HUD
  {
    private const int BarHeight = 25;
    private const int BarWidth = 25;

        private GraphicsDeviceManager _graphics;
    private MouseState mouseState;
    private Rectangle _healthBar;
    private Rectangle _healthBarBG;
    private Rectangle _breathBar;
    private Rectangle _breathBarBG;
    private int _ammo;
    private int _score;
    private float _breathTimer;
    private float _gamePadSensivity;
    private bool _reloadi;
    private Rectangle _reloadBar;
    public static Vector2 _target;
    private Random randomX;
    private Random randomY;
    private bool _hitmarker;
    private float _hitmarkerOpacity;
    private float _hitmarkerTimer;
    private float _hitmarkerDelay;
    private float drawBonusTimer;
    private float drawBonusDelay;
    private bool _recoil;
    private float _recoilTimer;
    private float _recoilDelay;
    private bool _bloodsplat;
    private float bloodsplatTimer;
    private Rectangle bloodsplatPos;
    private float bloodsplatDelay;

    public HUD(ref GraphicsDeviceManager graphics)
    {
      _graphics = graphics;
      _gamePadSensivity = 10f;
      _hitmarkerOpacity = (float) byte.MaxValue;
      _hitmarker = false;
      _hitmarkerTimer = 0.0f;
      _hitmarkerDelay = 750f;
      drawBonusTimer = 0.0f;
      drawBonusDelay = 2f;
      _breathTimer = 0.0f;
      _recoil = false;
      _recoilTimer = 0.0f;
      _recoilDelay = 750f;
      _bloodsplat = false;
      bloodsplatTimer = 0.0f;
      bloodsplatDelay = 2f;
      _ammo = 0;
      _reloadi = false;
      _healthBar = new Rectangle(12, 32, 200, 25);
      _healthBarBG = new Rectangle(10, 30, 204, 27);
      _breathBar = new Rectangle(12, 62, 200, 25);
      _breathBarBG = new Rectangle(10, 60, 204, 29);
      _reloadBar = new Rectangle(Game1.screenWidth - 225, Game1.screenHeight - 15, 0, 10);
      _reloadBar = new Rectangle(Game1.screenWidth - 225, Game1.screenHeight - 15, 0, 10);
      randomX = new Random();
      randomY = new Random();
    }

    public void setHitmarker()
    {
      _hitmarker = true;
      _hitmarkerTimer = 0.0f;
    }

    public void setBloodsplat()
    {
      _bloodsplat = true;
      bloodsplatPos = new Rectangle(randomX.Next(0, (Game1.screenWidth < Resources.bloodsplat.Width ? Game1.screenWidth - Resources.bloodsplat.Width : Game1.screenWidth)),
                                         randomY.Next(0, (Game1.screenHeight < Resources.bloodsplat.Height ? Game1.screenWidth - Resources.bloodsplat.Width: Game1.screenWidth)),
                                         Resources.bloodsplat.Width, Resources.bloodsplat.Height);
      bloodsplatTimer = 0.0f;
    }

    public void healthIndicator(ref Player player)
    {
      _healthBar.Width = player.getHealth() * 2;
      _healthBarBG.Width = player.getMaxHealth() * 2 + 4;
    }

    public void breathIndicator(ref Player player)
    {
      _breathBar.Width = (int) player.getBreathTimer() / 15;
      _breathBarBG.Width = 204;
    }

    public void ammoIndicator(ref Player player)
    {
      _ammo = player.getWeapon().getMagazine();
    }

    public void setRecoil()
    {
      _recoil = true;
    }

    public void updateRecoil(GameTime gameTime)
    {
      if (!_recoil)
        return;
      _recoilTimer += (float) gameTime.ElapsedGameTime.Milliseconds;
      if ((double) _recoilTimer <= (double) _recoilDelay / 4.0)
        HUD._target.Y -= 4f;
      else if ((double) _recoilTimer >= (double) _recoilDelay / 4.0 && (double) _recoilTimer < (double) _recoilDelay)
      {
        ++HUD._target.Y;
      }
      else
      {
        if ((double) _recoilTimer < (double) _recoilDelay)
          return;
        _recoilTimer = 0.0f;
        _recoil = false;
      }
    }

    public void checkHitmarker(GameTime gameTime)
    {
      if (!_hitmarker)
        return;
      _hitmarkerOpacity = (float) byte.MaxValue;
      if ((double) _hitmarkerOpacity >= 0.0 && (double) _hitmarkerOpacity <= (double) byte.MaxValue && (double) _hitmarkerTimer >= (double) _hitmarkerDelay * 0.150000005960464)
        _hitmarkerOpacity -= 15f;
      _hitmarkerTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) _hitmarkerTimer < (double) _hitmarkerDelay)
        return;
      _hitmarkerTimer = 0.0f;
      _hitmarker = false;
    }

    public void checkBloodsplat(GameTime gameTime)
    {
      if (!_bloodsplat)
        return;
      bloodsplatTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) bloodsplatTimer < (double) bloodsplatDelay)
        return;
      bloodsplatTimer = 0.0f;
      _bloodsplat = false;
    }

    public void updateDrawBonus(GameTime gameTime)
    {
      for (int index = 0; index < GameMain._items.Count; ++index)
      {
        if (GameMain._items[index].getDrawState())
        {
          drawBonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
          if ((double) drawBonusTimer >= (double) drawBonusDelay)
          {
            drawBonusTimer = 0.0f;
            GameMain._items[index].setActivity(false);
          }
        }
      }
    }

    public void reloadIndicator(ref Player player)
    {
      if (player.getWeapon().getState() == WeaponState.Reloading)
      {
        _reloadi = true;
        _reloadBar.X = (int) ((double) Game1.screenWidth - ((double) player.getWeapon().getReloadDelay() / 10.0 + 30.0));
        _reloadBar.Width = (int) ((double) player.getWeapon().getTimer() / 10.0);
      }
      else
      {
        _reloadi = false;
        _reloadBar.Width = 0;
      }
    }

    public void updateCrosshair(
      GameTime gameTime,
      ref Player player,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if ((double) HUD._target.X >= 0.0 && (double) HUD._target.X <= (double) Game1.screenWidth && ((double) HUD._target.Y >= 0.0 && (double) HUD._target.Y <= (double) Game1.screenHeight))
      {
        if (oldMouse.X != mouse.X || oldMouse.Y != mouse.Y)
        {
          HUD._target.X += (float) (mouse.X - oldMouse.X);
          HUD._target.Y += (float) (mouse.Y - oldMouse.Y);
        }
        else if ((double) gamePad.ThumbSticks.Right.X != 0.0 || (double) gamePad.ThumbSticks.Right.Y != 0.0)
        {
          HUD._target.X += gamePad.ThumbSticks.Right.X * _gamePadSensivity;
          HUD._target.Y -= gamePad.ThumbSticks.Right.Y * _gamePadSensivity;
        }
        else if (player.getBreath())
        {
          _breathTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
          if ((double) _breathTimer < 500.0)
            HUD._target.X += 0.5f * (float) randomX.Next(-3, 4);
          else if ((double) _breathTimer > 500.0 && (double) _breathTimer < 1250.0)
          {
            HUD._target.Y -= 0.5f;
            HUD._target.X += 0.5f * (float) randomX.Next(-3, 4);
          }
          else if ((double) _breathTimer >= 1250.0 && (double) _breathTimer < 1750.0)
            HUD._target.X += 0.5f * (float) randomX.Next(-3, 4);
          else if ((double) _breathTimer >= 1750.0 && (double) _breathTimer < 3000.0)
          {
            HUD._target.Y += 0.5f;
            HUD._target.X += 0.5f * (float) randomX.Next(-3, 4);
          }
          else if ((double) _breathTimer >= 3000.0)
            _breathTimer = 0.0f;
        }
        else
          _breathTimer = 0.0f;
      }
      Mouse.SetPosition(Game1.screenWidth / 2, Game1.screenHeight / 2);
      if ((double) HUD._target.X <= 0.0)
        HUD._target.X = 1f;
      else if ((double) HUD._target.X >= (double) Game1.screenWidth)
        HUD._target.X = (float) (Game1.screenWidth - 1);
      else if ((double) HUD._target.Y <= 0.0)
      {
        HUD._target.Y = 1f;
      }
      else
      {
        if ((double) HUD._target.Y < (double) Game1.screenHeight)
          return;
        HUD._target.Y = (float) (Game1.screenHeight - 1);
      }
    }

    public void Update(
      ref Player player,
      GameTime gameTime,
      KeyboardState keyboard,
      KeyboardState oldKeyboard,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      mouseState = mouse;
      _score = player.getScore();
      updateDrawBonus(gameTime);
      updateCrosshair(gameTime, ref player, mouse, oldMouse, gamePad, oldGamePad);
      healthIndicator(ref player);
      breathIndicator(ref player);
      ammoIndicator(ref player);
      updateRecoil(gameTime);
      reloadIndicator(ref player);
      checkHitmarker(gameTime);
      checkBloodsplat(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Resources.crosshair, new Rectangle((int) HUD._target.X - Resources.crosshair.Width / 2, (int) HUD._target.Y - Resources.crosshair.Height / 2, Resources.crosshair.Width, Resources.crosshair.Height), Color.White);
      spriteBatch.Draw(Game1.createTexture2D(_graphics), _healthBarBG, new Color(105, 105, 105, 180));
      spriteBatch.Draw(Game1.createTexture2D(_graphics), _healthBar, new Color(255, 0, 0, 180));
      spriteBatch.Draw(Game1.createTexture2D(_graphics), _breathBarBG, new Color(105, 105, 105, 180));
      spriteBatch.Draw(Game1.createTexture2D(_graphics), _breathBar, new Color(0, 0, 150, 180));
      if (_reloadi)
        spriteBatch.Draw(Game1.createTexture2D(_graphics), _reloadBar, Color.LimeGreen);
      spriteBatch.DrawString(Resources.title, "Score: " + _score.ToString(), new Vector2(22f, 79f), Color.Black);
      spriteBatch.DrawString(Resources.title, "Score: " + _score.ToString(), new Vector2(20f, 80f), Color.White);
      for (int ammo = _ammo; ammo >= 1; --ammo)
        spriteBatch.Draw(Resources.bullet, new Rectangle(Game1.screenWidth - (32 + ammo * 16), Game1.screenHeight - 52, 32, 32), Color.DimGray);
      if (_hitmarker)
        spriteBatch.Draw(Resources.hitmarker, new Rectangle((int) HUD._target.X - Resources.hitmarker.Width / 2, (int) HUD._target.Y - Resources.hitmarker.Height / 2, Resources.hitmarker.Width, Resources.hitmarker.Height), Color.Lerp(Color.White, Color.Transparent, _hitmarkerOpacity));
      if (_bloodsplat)
        spriteBatch.Draw(Resources.bloodsplat, bloodsplatPos, Color.Red);
      for (int index = 0; index < GameMain._items.Count; ++index)
      {
        if (GameMain._items[index].getDrawState())
          spriteBatch.Draw(GameMain._items[index].getGFX(), new Rectangle(20, 150, GameMain._items[index].getGFX().Width, GameMain._items[index].getGFX().Height), Color.White * 0.75f);
      }
    }
  }
}
