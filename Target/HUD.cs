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
    private GraphicsDeviceManager m_graphics;
    private MouseState mouseState;
    private Rectangle m_healthBar;
    private Rectangle m_healthBarBG;
    private Rectangle m_breathBar;
    private Rectangle m_breathBarBG;
    private int m_ammo;
    private int m_score;
    private float m_breathTimer;
    private float m_gamePadSensivity;
    private bool m_reloadi;
    private Rectangle m_reloadBar;
    public static Vector2 m_target;
    private Random randomX;
    private Random randomY;
    private bool m_hitmarker;
    private float m_hitmarkerOpacity;
    private float m_hitmarkerTimer;
    private float m_hitmarkerDelay;
    private float drawBonusTimer;
    private float drawBonusDelay;
    private bool m_recoil;
    private float m_recoilTimer;
    private float m_recoilDelay;
    private bool m_bloodsplat;
    private float bloodsplatTimer;
    private Rectangle bloodsplatPos;
    private float bloodsplatDelay;

    public HUD(ref GraphicsDeviceManager graphics)
    {
      this.m_graphics = graphics;
      this.m_gamePadSensivity = 10f;
      this.m_hitmarkerOpacity = (float) byte.MaxValue;
      this.m_hitmarker = false;
      this.m_hitmarkerTimer = 0.0f;
      this.m_hitmarkerDelay = 750f;
      this.drawBonusTimer = 0.0f;
      this.drawBonusDelay = 2f;
      this.m_breathTimer = 0.0f;
      this.m_recoil = false;
      this.m_recoilTimer = 0.0f;
      this.m_recoilDelay = 750f;
      this.m_bloodsplat = false;
      this.bloodsplatTimer = 0.0f;
      this.bloodsplatDelay = 2f;
      this.m_ammo = 0;
      this.m_reloadi = false;
      this.m_healthBar = new Rectangle(12, 32, 200, 25);
      this.m_healthBarBG = new Rectangle(10, 30, 204, 29);
      this.m_breathBar = new Rectangle(12, 62, 200, 25);
      this.m_breathBarBG = new Rectangle(10, 60, 204, 29);
      this.m_reloadBar = new Rectangle(Game1.screenWidth - 225, Game1.screenHeight - 15, 0, 10);
      this.m_reloadBar = new Rectangle(Game1.screenWidth - 225, Game1.screenHeight - 15, 0, 10);
      this.randomX = new Random();
      this.randomY = new Random();
    }

    public void setHitmarker()
    {
      this.m_hitmarker = true;
      this.m_hitmarkerTimer = 0.0f;
    }

    public void setBloodsplat()
    {
      this.m_bloodsplat = true;
      this.bloodsplatPos = new Rectangle(this.randomX.Next(0, Game1.screenWidth - Resources.bloodsplat.Width), this.randomY.Next(0, Game1.screenHeight - Resources.bloodsplat.Height), Resources.bloodsplat.Width, Resources.bloodsplat.Height);
      this.bloodsplatTimer = 0.0f;
    }

    public void healthIndicator(ref Player player)
    {
      this.m_healthBar.Width = player.getHealth() * 2;
      this.m_healthBarBG.Width = player.getMaxHealth() * 2 + 4;
    }

    public void breathIndicator(ref Player player)
    {
      this.m_breathBar.Width = (int) player.getBreathTimer() / 15;
      this.m_breathBarBG.Width = 204;
    }

    public void ammoIndicator(ref Player player)
    {
      this.m_ammo = player.getWeapon().getMagazine();
    }

    public void setRecoil()
    {
      this.m_recoil = true;
    }

    public void updateRecoil(GameTime gameTime)
    {
      if (!this.m_recoil)
        return;
      this.m_recoilTimer += (float) gameTime.ElapsedGameTime.Milliseconds;
      if ((double) this.m_recoilTimer <= (double) this.m_recoilDelay / 4.0)
        HUD.m_target.Y -= 4f;
      else if ((double) this.m_recoilTimer >= (double) this.m_recoilDelay / 4.0 && (double) this.m_recoilTimer < (double) this.m_recoilDelay)
      {
        ++HUD.m_target.Y;
      }
      else
      {
        if ((double) this.m_recoilTimer < (double) this.m_recoilDelay)
          return;
        this.m_recoilTimer = 0.0f;
        this.m_recoil = false;
      }
    }

    public void checkHitmarker(GameTime gameTime)
    {
      if (!this.m_hitmarker)
        return;
      this.m_hitmarkerOpacity = (float) byte.MaxValue;
      if ((double) this.m_hitmarkerOpacity >= 0.0 && (double) this.m_hitmarkerOpacity <= (double) byte.MaxValue && (double) this.m_hitmarkerTimer >= (double) this.m_hitmarkerDelay * 0.150000005960464)
        this.m_hitmarkerOpacity -= 15f;
      this.m_hitmarkerTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.m_hitmarkerTimer < (double) this.m_hitmarkerDelay)
        return;
      this.m_hitmarkerTimer = 0.0f;
      this.m_hitmarker = false;
    }

    public void checkBloodsplat(GameTime gameTime)
    {
      if (!this.m_bloodsplat)
        return;
      this.bloodsplatTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.bloodsplatTimer < (double) this.bloodsplatDelay)
        return;
      this.bloodsplatTimer = 0.0f;
      this.m_bloodsplat = false;
    }

    public void updateDrawBonus(GameTime gameTime)
    {
      for (int index = 0; index < GameMain.m_items.Count; ++index)
      {
        if (GameMain.m_items[index].getDrawState())
        {
          this.drawBonusTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
          if ((double) this.drawBonusTimer >= (double) this.drawBonusDelay)
          {
            this.drawBonusTimer = 0.0f;
            GameMain.m_items[index].setActivity(false);
          }
        }
      }
    }

    public void reloadIndicator(ref Player player)
    {
      if (player.getWeapon().getState() == WeaponState.Reloading)
      {
        this.m_reloadi = true;
        this.m_reloadBar.X = (int) ((double) Game1.screenWidth - ((double) player.getWeapon().getReloadDelay() / 10.0 + 30.0));
        this.m_reloadBar.Width = (int) ((double) player.getWeapon().getTimer() / 10.0);
      }
      else
      {
        this.m_reloadi = false;
        this.m_reloadBar.Width = 0;
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
      if ((double) HUD.m_target.X >= 0.0 && (double) HUD.m_target.X <= (double) Game1.screenWidth && ((double) HUD.m_target.Y >= 0.0 && (double) HUD.m_target.Y <= (double) Game1.screenHeight))
      {
        if (oldMouse.X != mouse.X || oldMouse.Y != mouse.Y)
        {
          HUD.m_target.X += (float) (mouse.X - oldMouse.X);
          HUD.m_target.Y += (float) (mouse.Y - oldMouse.Y);
        }
        else if ((double) gamePad.ThumbSticks.Right.X != 0.0 || (double) gamePad.ThumbSticks.Right.Y != 0.0)
        {
          HUD.m_target.X += gamePad.ThumbSticks.Right.X * this.m_gamePadSensivity;
          HUD.m_target.Y -= gamePad.ThumbSticks.Right.Y * this.m_gamePadSensivity;
        }
        else if (player.getBreath())
        {
          this.m_breathTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
          if ((double) this.m_breathTimer < 500.0)
            HUD.m_target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          else if ((double) this.m_breathTimer > 500.0 && (double) this.m_breathTimer < 1250.0)
          {
            HUD.m_target.Y -= 0.5f;
            HUD.m_target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          }
          else if ((double) this.m_breathTimer >= 1250.0 && (double) this.m_breathTimer < 1750.0)
            HUD.m_target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          else if ((double) this.m_breathTimer >= 1750.0 && (double) this.m_breathTimer < 3000.0)
          {
            HUD.m_target.Y += 0.5f;
            HUD.m_target.X += 0.5f * (float) this.randomX.Next(-3, 4);
          }
          else if ((double) this.m_breathTimer >= 3000.0)
            this.m_breathTimer = 0.0f;
        }
        else
          this.m_breathTimer = 0.0f;
      }
      Mouse.SetPosition(Game1.screenWidth / 2, Game1.screenHeight / 2);
      if ((double) HUD.m_target.X <= 0.0)
        HUD.m_target.X = 1f;
      else if ((double) HUD.m_target.X >= (double) Game1.screenWidth)
        HUD.m_target.X = (float) (Game1.screenWidth - 1);
      else if ((double) HUD.m_target.Y <= 0.0)
      {
        HUD.m_target.Y = 1f;
      }
      else
      {
        if ((double) HUD.m_target.Y < (double) Game1.screenHeight)
          return;
        HUD.m_target.Y = (float) (Game1.screenHeight - 1);
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
      this.m_score = player.getScore();
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
      spriteBatch.Draw(Resources.crosshair, new Rectangle((int) HUD.m_target.X - Resources.crosshair.Width / 2, (int) HUD.m_target.Y - Resources.crosshair.Height / 2, Resources.crosshair.Width, Resources.crosshair.Height), Color.White);
      spriteBatch.Draw(Game1.createTexture2D(this.m_graphics), this.m_healthBarBG, Color.DimGray);
      spriteBatch.Draw(Game1.createTexture2D(this.m_graphics), this.m_healthBar, Color.Red);
      spriteBatch.Draw(Game1.createTexture2D(this.m_graphics), this.m_breathBarBG, Color.DimGray);
      spriteBatch.Draw(Game1.createTexture2D(this.m_graphics), this.m_breathBar, Color.Turquoise);
      if (this.m_reloadi)
        spriteBatch.Draw(Game1.createTexture2D(this.m_graphics), this.m_reloadBar, Color.LimeGreen);
      spriteBatch.DrawString(Resources.title, "Score: " + this.m_score.ToString(), new Vector2(22f, 79f), Color.Black);
      spriteBatch.DrawString(Resources.title, "Score: " + this.m_score.ToString(), new Vector2(20f, 80f), Color.White);
      for (int ammo = this.m_ammo; ammo >= 1; --ammo)
        spriteBatch.Draw(Resources.bullet, new Rectangle(Game1.screenWidth - (32 + ammo * 16), Game1.screenHeight - 52, 32, 32), Color.DimGray);
      if (this.m_hitmarker)
        spriteBatch.Draw(Resources.hitmarker, new Rectangle((int) HUD.m_target.X - Resources.hitmarker.Width / 2, (int) HUD.m_target.Y - Resources.hitmarker.Height / 2, Resources.hitmarker.Width, Resources.hitmarker.Height), Color.Lerp(Color.White, Color.Transparent, this.m_hitmarkerOpacity));
      if (this.m_bloodsplat)
        spriteBatch.Draw(Resources.bloodsplat, this.bloodsplatPos, Color.Red);
      for (int index = 0; index < GameMain.m_items.Count; ++index)
      {
        if (GameMain.m_items[index].getDrawState())
          spriteBatch.Draw(GameMain.m_items[index].getGFX(), new Rectangle(20, 150, GameMain.m_items[index].getGFX().Width, GameMain.m_items[index].getGFX().Height), Color.White * 0.75f);
      }
    }
  }
}
