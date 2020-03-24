// Decompiled with JetBrains decompiler
// Type: Target.HUD
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;
using Target.Utils;

namespace Target
{
  public class HUD
  {
    private MouseState mouseState;

    private HorizontalProgressBar _healthIndicator;
    private HorizontalProgressBar _breathIndicator;

    private Label _scoreIndicator;
    public Label scoreIndicator { get { return (_scoreIndicator); } }

    private Label _multiplierIndicator;

    Label _actionIndicator;
    Timer _actionTimer;

    private int _ammo;
    private float _breathTimer;
    private float _gamePadSensivity;

    HorizontalProgressBar _reloadIndicator;

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

    private Desktop _UI;

    public HUD()
    {
      _gamePadSensivity = 10f;
      _hitmarkerOpacity = (float) byte.MaxValue;
      _actionTimer = new Timer();
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
      randomX = new Random();
      randomY = new Random();
      _target.X = Options.Config.Width / 2;
      _target.Y = Options.Config.Height / 2;

      _UI = new Desktop();
      addIndicators();
    }

    // NEW HUD

    public void addIndicators()
    {
      // Top Panel
      VerticalStackPanel topPanel = new VerticalStackPanel();
      topPanel.Spacing = 8;

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Gray);
      Stylesheet.Current.HorizontalProgressBarStyle.Filled = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Red);
      _healthIndicator = new HorizontalProgressBar();
      _healthIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _healthIndicator.VerticalAlignment = VerticalAlignment.Top;
      _healthIndicator.Width = 200;
      _healthIndicator.Height = 20;
      topPanel.Widgets.Add(_healthIndicator);

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Gray);
      Stylesheet.Current.HorizontalProgressBarStyle.Filled = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Blue);
      _breathIndicator = new HorizontalProgressBar();
      _breathIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _breathIndicator.VerticalAlignment = VerticalAlignment.Top;
      _breathIndicator.Width = 200;
      _breathIndicator.Height = 20;
      topPanel.Widgets.Add(_breathIndicator);

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      _scoreIndicator = new Label();
      _scoreIndicator.Text = "Score: 0";
      _scoreIndicator.Height = 6;
      _scoreIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _scoreIndicator.VerticalAlignment = VerticalAlignment.Top;
      topPanel.Widgets.Add(_scoreIndicator);

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      _multiplierIndicator = new Label();
      _multiplierIndicator.Text = "x1";
      _multiplierIndicator.Height = 6;
      _multiplierIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _multiplierIndicator.VerticalAlignment = VerticalAlignment.Top;
      topPanel.Widgets.Add(_multiplierIndicator);

      Stylesheet.Current.LabelStyle.Font = Resources.titleFont;
      _actionIndicator = new Label();
      _actionIndicator.Text = "";
      _actionIndicator.Height = 6;
      _actionIndicator.HorizontalAlignment = HorizontalAlignment.Center;
      _actionIndicator.VerticalAlignment = VerticalAlignment.Center;
      topPanel.Widgets.Add(_actionIndicator);
      _actionTimer.addAction(TimerDirection.Forward, 1500f, TimeoutBehaviour.Reset, () => { _actionIndicator.Text = ""; });

      _UI.Widgets.Add(topPanel);
      // Bottom Panel
      Panel bottomPanel = new Panel();

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.HorizontalProgressBarStyle.Filled = new ColoredRegion(DefaultAssets.WhiteRegion, Color.LimeGreen);
      _reloadIndicator = new HorizontalProgressBar();
      _reloadIndicator.HorizontalAlignment = HorizontalAlignment.Right;
      _reloadIndicator.VerticalAlignment = VerticalAlignment.Bottom;
      _reloadIndicator.Width = 200;
      _reloadIndicator.Height = 10;
      _reloadIndicator.Left = -50;
      bottomPanel.Widgets.Add(_reloadIndicator);

      _UI.Widgets.Add(bottomPanel);
    }

    //Old

    public void setHitmarker()
    {
      _hitmarker = true;
      _hitmarkerTimer = 0.0f;
    }

    public void setBloodsplat()
    {
      _bloodsplat = true;
      bloodsplatPos = new Rectangle(randomX.Next(0, (Options.Config.Width < Resources.bloodsplat.Width ? Options.Config.Width - Resources.bloodsplat.Width : Options.Config.Width)),
                                         randomY.Next(0, (Options.Config.Height < Resources.bloodsplat.Height ? Options.Config.Width - Resources.bloodsplat.Width: Options.Config.Width)),
                                         Resources.bloodsplat.Width, Resources.bloodsplat.Height);
      bloodsplatTimer = 0.0f;
    }
    public void setAction(string action)
    {
      _actionIndicator.Text = action;
      _actionTimer.Start();
    }

    public void udpateHealth(int maxHealth, int health)
    {
      _healthIndicator.Minimum = 0;
      _healthIndicator.Maximum = maxHealth;
      _healthIndicator.Value = health;
    }

    public void updateBreath(double breath)
    {
      _breathIndicator.Minimum = 0;
      _breathIndicator.Maximum = 2500;
      _breathIndicator.Value = (float)breath;
    }
    public void updateReload(Weapon playerWeapon)
    {
      _reloadIndicator.Minimum = 0;
      _reloadIndicator.Maximum = 1500;
      _reloadIndicator.Value = (float)playerWeapon.getStatusTimer();
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

    public void updateCrosshair(
      GameTime gameTime,
      ref Player player,
      MouseState mouse,
      MouseState oldMouse,
      GamePadState gamePad,
      GamePadState oldGamePad)
    {
      if ((double) HUD._target.X >= 0.0 && (double) HUD._target.X <= (double) Options.Config.Width && ((double) HUD._target.Y >= 0.0 && (double) HUD._target.Y <= (double) Options.Config.Height))
      {
        if (oldMouse.X != mouse.X || oldMouse.Y != mouse.Y)
        {
          HUD._target.X += (mouse.X - oldMouse.X) * Options.Config.MouseSensivity;
          HUD._target.Y += (mouse.Y - oldMouse.Y) * Options.Config.MouseSensivity;
        }
        else if ((double) gamePad.ThumbSticks.Right.X != 0.0 || (double) gamePad.ThumbSticks.Right.Y != 0.0)
        {
          HUD._target.X += gamePad.ThumbSticks.Right.X * _gamePadSensivity;
          HUD._target.Y -= gamePad.ThumbSticks.Right.Y * _gamePadSensivity;
        }
        else if (player.getBreathState() != Player.BreathState.Holding)
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
      Mouse.SetPosition(Options.Config.Width / 2, Options.Config.Height / 2);
      if ((double) HUD._target.X <= 0.0)
        HUD._target.X = 1f;
      else if ((double) HUD._target.X >= (double) Options.Config.Width)
        HUD._target.X = (float) (Options.Config.Width - 1);
      else if ((double) HUD._target.Y <= 0.0)
      {
        HUD._target.Y = 1f;
      }
      else
      {
        if ((double) HUD._target.Y < (double) Options.Config.Height)
          return;
        HUD._target.Y = (float) (Options.Config.Height - 1);
      }
    }
    
    public Texture2D getCrosshair()
    {
      if (_reloadIndicator.Value > 0) //We're reloading
      {
        return (Resources.reloadingCursor);
      }
      return (Resources.crosshair);
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
      _multiplierIndicator.Text = "x" + player.getMultiplier();
      _scoreIndicator.Text = "Score: " + player.getScore();
      udpateHealth(player.getMaxHealth(), player.getHealth());
      updateBreath(player.getBreathTimer());
      updateReload(player.getWeapon());

      updateDrawBonus(gameTime);
      updateCrosshair(gameTime, ref player, mouse, oldMouse, gamePad, oldGamePad);
      ammoIndicator(ref player);
      updateRecoil(gameTime);
      checkHitmarker(gameTime);
      checkBloodsplat(gameTime);
      _actionTimer.Update(gameTime);
    }

    public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(getCrosshair(), new Rectangle((int) HUD._target.X - Resources.crosshair.Width / 2, (int) HUD._target.Y - Resources.crosshair.Height / 2, Resources.crosshair.Width, Resources.crosshair.Height), Color.White);
      for (int ammo = _ammo; ammo >= 1; --ammo)
        spriteBatch.Draw(Resources.bullet, new Rectangle(Options.Config.Width - (32 + ammo * 16), Options.Config.Height - 52, 32, 32), Color.DimGray);
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

    public void DrawUI()
    {
      _UI.Render();
    }
  }
}
