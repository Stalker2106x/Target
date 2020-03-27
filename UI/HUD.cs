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
using Target.UI;
using Target.Utils;

namespace Target
{
  public class HUD
  {
    private HorizontalProgressBar _healthIndicator;
    private HorizontalProgressBar _kevlarIndicator;
    private HorizontalProgressBar _breathIndicator;

    private Label _scoreIndicator;
    public Label scoreIndicator { get { return (_scoreIndicator); } }

    private Label _multiplierIndicator;

    Label _actionIndicator;
    Timer _actionTimer;

    private int _ammo;

    HorizontalProgressBar _reloadIndicator;

    private Random _randomGenerator;

    public Crosshair crosshair;
    public ContractsPanel contractsPanel;

    private bool _hitmarker;
    private float _hitmarkerOpacity;
    private float _hitmarkerTimer;
    private float _hitmarkerDelay;

    private bool _bloodsplat;
    private float bloodsplatTimer;
    private Rectangle bloodsplatPos;
    private float bloodsplatDelay;

    private Desktop _UI;

    public HUD()
    {
      _hitmarkerOpacity = (float) byte.MaxValue;
      _randomGenerator = new Random();
      crosshair = new Crosshair();
      _actionTimer = new Timer();
      _hitmarker = false;
      _hitmarkerTimer = 0.0f;
      _hitmarkerDelay = 750f;
      _bloodsplat = false;
      bloodsplatTimer = 0.0f;
      bloodsplatDelay = 2f;
      _ammo = 0;
      _UI = new Desktop();
      contractsPanel = new ContractsPanel(_UI);
      addIndicators();
    }

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

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.HorizontalProgressBarStyle.Filled = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Blue);
      _kevlarIndicator = new HorizontalProgressBar();
      _kevlarIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _kevlarIndicator.VerticalAlignment = VerticalAlignment.Top;
      _kevlarIndicator.Width = 200;
      _kevlarIndicator.Height = 20;
      topPanel.Widgets.Add(_kevlarIndicator);

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Gray);
      Stylesheet.Current.HorizontalProgressBarStyle.Filled = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Green);
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

      Stylesheet.Current.LabelStyle.Font = Resources.alertFont;
      _actionIndicator = new Label();
      _actionIndicator.Text = "";
      _actionIndicator.TextColor = Color.DarkRed;
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

    public HorizontalProgressBar addBombIndicator(Point bombPos)
    {
      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.HorizontalProgressBarStyle.Filled = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Red);
      HorizontalProgressBar bombIndicator = new HorizontalProgressBar();
      bombIndicator.Top = bombPos.Y;
      bombIndicator.Left = bombPos.X;
      bombIndicator.Width = 100;
      bombIndicator.Height = 5;
      _UI.Widgets.Add(bombIndicator);
      return (bombIndicator);
    }

    public void removeBombIndicator(HorizontalProgressBar indicator)
    {
      _UI.Widgets.Remove(indicator);
    }

    public void setHitmarker()
    {
      _hitmarker = true;
      _hitmarkerTimer = 0.0f;
    }

    public void setBloodsplat()
    {
      _bloodsplat = true;
      bloodsplatPos = new Rectangle(_randomGenerator.Next(0, (Options.Config.Width < Resources.bloodsplat.Width ? Options.Config.Width - Resources.bloodsplat.Width : Options.Config.Width)),
                                    _randomGenerator.Next(0, (Options.Config.Height < Resources.bloodsplat.Height ? Options.Config.Width - Resources.bloodsplat.Width: Options.Config.Width)),
                                         Resources.bloodsplat.Width, Resources.bloodsplat.Height);
      bloodsplatTimer = 0.0f;
    }
    public void setAction(string action)
    {
      _actionIndicator.Text = action;
      _actionTimer.Start();
    }

    public void updateHealth(int maxHealth, int health)
    {
      _healthIndicator.Minimum = 0;
      _healthIndicator.Maximum = maxHealth;
      _healthIndicator.Value = health;
    }
    public void updateKevlar(int maxKevlar, int state)
    {
      _kevlarIndicator.Minimum = 0;
      _kevlarIndicator.Maximum = maxKevlar;
      _kevlarIndicator.Value = state;
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

    public void updateScoreMultiplier(float scoreMultiplier)
    {
      _multiplierIndicator.Text = "x" + scoreMultiplier;
    }
    public void updateScore(int score)
    {
      _scoreIndicator.Text = "Score:" + score;
    }

    public void ammoIndicator(ref Player player)
    {
      _ammo = player.getWeapon().getMagazine();
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

    public void Update(GameTime gameTime, ref Player player, DeviceState state, DeviceState prevState)
    {
      crosshair.Update(gameTime, state, prevState);
      contractsPanel.Update();

      ammoIndicator(ref player);
      checkHitmarker(gameTime);
      checkBloodsplat(gameTime);
      _actionTimer.Update(gameTime);
    }

    public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
      crosshair.Draw(graphics, spriteBatch);
      for (int ammo = _ammo; ammo >= 1; --ammo)
        spriteBatch.Draw(Resources.bullet, new Rectangle(Options.Config.Width - (32 + ammo * 16), Options.Config.Height - 52, 32, 32), Color.DimGray);
      if (_bloodsplat)
        spriteBatch.Draw(Resources.bloodsplat, bloodsplatPos, Color.Red);
    }

    public void DrawUI()
    {
      _UI.Render();
    }
  }
}
