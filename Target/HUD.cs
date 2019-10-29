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
  internal class HUD
  {
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
      this._graphics = graphics;
      this._gamePadSensivity = 10f;
      this._hitmarkerOpacity = (float) byte.MaxValue;
      this._hitmarker = false;
      this._hitmarkerTimer = 0.0f;
      this._hitmarkerDelay = 750f;
      this.drawBonusTimer = 0.0f;
      this.drawBonusDelay = 2f;
      this._breathTimer = 0.0f;
      this._recoil = false;
      this._recoilTimer = 0.0f;
      this._recoilDelay = 750f;
      this._bloodsplat = false;
      this.bloodsplatTimer = 0.0f;
      this.bloodsplatDelay = 2f;
      this._ammo = 0;
      this._reloadi = false;
      this._healthBar = new Rectangle(12, 32, 200, 25);
      this._healthBarBG = new Rectangle(10, 30, 204, 27);
      this._breathBar = new Rectangle(12, 62, 200, 25);
      this._breathBarBG = new Rectangle(10, 60, 204, 29);
      this._reloadBar = new Rectangle(Game1.screenWidth - 225, Game1.screenHeight - 15, 0, 10);
      this._reloadBar = new Rectangle(Game1.screenWidth - 225, Game1.screenHeight - 15, 0, 10);
      this.randomX = new Random();
      this.randomY = new Random();
    }

    public void setHitmarker()
    {
      this._hitmarker = true;
      this._hitmarkerTimer = 0.0f;
    }

    public void setBloodsplat()
    {
      this._bloodsplat = true;
      this.bloodsplatPos = new Rectangle(this.randomX.Next(0, Game1.screenWidth - Resources.bloodsplat.Width), this.randomY.Next(0, Game1.screenHeight - Resources.bloodsplat.Height), Resources.bloodsplat.Width, Resources.bloodsplat.Height);
      this.bloodsplatTimer = 0.0f;
    }

    public void healthIndicator(ref Player player)
    {
      this._healthBar.Width = player.getHealth() * 2;
      this._healthBarBG.Width = player.getMaxHealth() * 2 + 4;
    }

    public void breathIndicator(ref Player player)
    {
      this._breathBar.Width = (int) player.getBreathTimer() / 15;
      this._breathBarBG.Width = 204;
    }

    public void ammoIndicator(ref Player player)
    {
      this._ammo = player.getWeapon().getMagazine();
    }

    public void setRecoil()
    {
      this._recoil = true;
    }

    public void updateRecoil(GameTime gameTime)
    {
      if (!this._recoil)
        return;
      this._recoilTimer += (float) gameTime.ElapsedGameTime.Milliseconds;
      if ((double) this._recoilTimer <= (double) this._recoilDelay / 4.0)
        HUD._target.Y -= 4f;
      else if ((double) this._recoilTimer >= (double) this._recoilDelay / 4.0 && (double) this._recoilTimer < (double) this._recoilDelay)
      {
        ++HUD._target.Y;
      }
      else
      {
        if ((double) this._recoilTimer < (double) this._recoilDelay)
          return;
        this._recoilTimer = 0.0f;
        this._recoil = false;
      }
    }

    public void checkHitmarker(GameTime gameTime)
    {
      if (!this._hitmarker)
        return;
      this._hitmarkerOpacity = (float) byte.MaxValue;
      if ((double) this._hitmarkerOpacity >= 0.0 && (double) this._hitmarkerOpacity <= (double) byte.MaxValue && (double) this._hitmarkerTimer >= (double) this._hitmarkerDelay * 0.150000005960464)
        this._hitmarkerOpacity -= 15f;
      this._hitmarkerTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this._hitmarkerTimer < (double) this._hitmarkerDelay)
        return;
      this._hitmarkerTimer = 0.0f;
      this._hitmarker = false;
    }

    public void checkBloodsplat(GameTime gameTime)
    {
      if (!this._bloodsplat)
        return;
      this.bloodsplatTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.bloodsplatTimer < (double) this.bloodsplatDelay)
        return;
      this.bloodsplatTimer = 0.0f;
      this._bloodsplat = false;
    }

    public void updateDrawBonus(GameTime gameTime)
    {
      for (int index = 0; index < GameMain._items.Count; ++index)
      {
        if (GameMain._items[index].getDrawState())
        {
          this.drawBonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
          if ((double) this.drawBonusTimer >= (double) this.drawBonusDelay)
          {
            this.drawBonusTimer = 0.0f;
            GameMain._items[index].setActivity(false);
          }
        }
      }
    }

    public void reloadIndicator(ref Player player)
    {
      if (player.getWeapon().getState() == WeaponState.Reloading)
      {
        this._reloadi = true;
        this._reloadBar.X = (int) ((double) Game1.screenWidth - ((double) player.getWeapon().getReloadDelay() / 10.0 + 30.0));
        this._reloadBar.Width = (int) ((double) player.getWeapon().getTimer() / 10.0);
      }
      else
      {
        this._reloadi = false;
        this._reloadBar.Width = 0;
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
          HUD._target.X += gamePad.ThumbSticks.Right.X * this._gamePadSensivity;
          HUD._target.Y -= gamePad.ThumbSticks.Right.Y * this._gamePadSensivity;
        }
        else if (player.getBreath())
        {
          this._breathTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
          if ((double) this._breathTimer < 500.0)
            HUD._target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          else if ((double) this._breathTimer > 500.0 && (double) this._breathTimer < 1250.0)
          {
            HUD._target.Y -= 0.5f;
            HUD._target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          }
          else if ((double) this._breathTimer >= 1250.0 && (double) this._breathTimer < 1750.0)
            HUD._target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          else if ((double) this._breathTimer >= 1750.0 && (double) this._breathTimer < 3000.0)
          {
            HUD._target.Y += 0.5f;
            HUD._target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          }
          else if ((double) this._breathTimer >= 3000.0)
            this._breathTimer = 0.0f;
        }
        else
          this._breathTimer = 0.0f;
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
      this.mouseState = mouse;
      this._score = player.getScore();
      this.updateDrawBonus(gameTime);
      this.updateCrosshair(gameTime, ref player, mouse, oldMouse, gamePad, oldGamePad);
      this.healthIndicator(ref player);
      this.breathIndicator(ref player);
      this.ammoIndicator(ref player);
      this.updateRecoil(gameTime);
      this.reloadIndicator(ref player);
      this.checkHitmarker(gameTime);
      this.checkBloodsplat(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Resources.crosshair, new Rectangle((int) HUD._target.X - Resources.crosshair.Width / 2, (int) HUD._target.Y - Resources.crosshair.Height / 2, Resources.crosshair.Width, Resources.crosshair.Height), Color.White);
      spriteBatch.Draw(Game1.createTexture2D(this._graphics), this._healthBarBG, Color.DimGray);
      spriteBatch.Draw(Game1.createTexture2D(this._graphics), this._healthBar, Color.Red);
      spriteBatch.Draw(Game1.createTexture2D(this._graphics), this._breathBarBG, Color.DimGray);
      spriteBatch.Draw(Game1.createTexture2D(this._graphics), this._breathBar, Color.Turquoise);
      if (this._reloadi)
        spriteBatch.Draw(Game1.createTexture2D(this._graphics), this._reloadBar, Color.LimeGreen);
      spriteBatch.DrawString(Resources.title, "Score: " + this._score.ToString(), new Vector2(22f, 79f), Color.Black);
      spriteBatch.DrawString(Resources.title, "Score: " + this._score.ToString(), new Vector2(20f, 80f), Color.White);
      for (int ammo = this._ammo; ammo >= 1; --ammo)
        spriteBatch.Draw(Resources.bullet, new Rectangle(Game1.screenWidth - (32 + ammo * 16), Game1.screenHeight - 52, 32, 32), Color.DimGray);
      if (this._hitmarker)
        spriteBatch.Draw(Resources.hitmarker, new Rectangle((int) HUD._target.X - Resources.hitmarker.Width / 2, (int) HUD._target.Y - Resources.hitmarker.Height / 2, Resources.hitmarker.Width, Resources.hitmarker.Height), Color.Lerp(Color.White, Color.Transparent, this._hitmarkerOpacity));
      if (this._bloodsplat)
        spriteBatch.Draw(Resources.bloodsplat, this.bloodsplatPos, Color.Red);
      for (int index = 0; index < GameMain._items.Count; ++index)
      {
        if (GameMain._items[index].getDrawState())
          spriteBatch.Draw(GameMain._items[index].getGFX(), new Rectangle(20, 150, GameMain._items[index].getGFX().Width, GameMain._items[index].getGFX().Height), Color.White * 0.75f);
      }
    }
  }
}
