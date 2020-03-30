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
using TargetGame.Settings;
using TargetGame.UI;
using TargetGame.Utils;

namespace TargetGame
{
  public class HUD
  {
    private const float _proportionsX = 0.15f; //Width of HUD elements

    private HorizontalProgressBar _healthIndicator;
    private HorizontalProgressBar _kevlarIndicator;
    private HorizontalProgressBar _breathIndicator;

    private Label _scoreIndicator;
    public Label scoreIndicator { get { return (_scoreIndicator); } }

    private Label _multiplierIndicator;

    private Image _defuserIndicator;

    private Label _actionIndicator;
    private Timer _actionTimer;

    private int _ammo;

    private HorizontalProgressBar _reloadIndicator;

    private Random _randomGenerator;

    public Crosshair crosshair;
    public ContractsPanel contractsPanel;

    private bool _bloodsplat;
    private Rectangle _bloodsplatPos;
    private Timer _bloodsplatTimer;

    private Panel _ui;

    public HUD()
    {
      _randomGenerator = new Random();
      crosshair = new Crosshair();
      contractsPanel = new ContractsPanel();
      _ui = new Panel();
      _actionTimer = new Timer();
      _bloodsplat = false;
      _bloodsplatTimer = new Timer();
      _bloodsplatTimer.addAction(TimerDirection.Forward, 2000, TimeoutBehaviour.Reset, () => { _bloodsplat = false; });
      _ammo = 0;
      addIndicators();
    }

    public void activate()
    {
      Desktop.Root = _ui;
    }

    public void addIndicators()
    {
      // Top Panel
      VerticalStackPanel topPanel = new VerticalStackPanel();
      topPanel.Spacing = 8;

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Gray);
      Stylesheet.Current.HorizontalProgressBarStyle.Filler = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Red);
      _healthIndicator = new HorizontalProgressBar();
      _healthIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _healthIndicator.VerticalAlignment = VerticalAlignment.Top;
      _healthIndicator.Width = (int)(Options.Config.Width * _proportionsX);
      _healthIndicator.Height = 20;
      topPanel.Widgets.Add(_healthIndicator);

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.HorizontalProgressBarStyle.Filler = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Blue);
      _kevlarIndicator = new HorizontalProgressBar();
      _kevlarIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _kevlarIndicator.VerticalAlignment = VerticalAlignment.Top;
      _kevlarIndicator.Width = (int)(Options.Config.Width * _proportionsX);
      _kevlarIndicator.Height = 20;
      topPanel.Widgets.Add(_kevlarIndicator);

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Gray);
      Stylesheet.Current.HorizontalProgressBarStyle.Filler = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Green);
      _breathIndicator = new HorizontalProgressBar();
      _breathIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _breathIndicator.VerticalAlignment = VerticalAlignment.Top;
      _breathIndicator.Width = (int)(Options.Config.Width * _proportionsX);
      _breathIndicator.Height = 20;
      topPanel.Widgets.Add(_breathIndicator);

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      _scoreIndicator = new Label();
      _scoreIndicator.Text = "Score: 0";
      _scoreIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _scoreIndicator.VerticalAlignment = VerticalAlignment.Top;
      topPanel.Widgets.Add(_scoreIndicator);

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      _multiplierIndicator = new Label();
      _multiplierIndicator.Text = "x1";
      _multiplierIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _multiplierIndicator.VerticalAlignment = VerticalAlignment.Top;
      topPanel.Widgets.Add(_multiplierIndicator);

      _defuserIndicator = new Image();
      _defuserIndicator.Opacity = 0;
      _defuserIndicator.Renderable = new TextureRegion(Resources.defuser, new Rectangle(0, 0, Resources.defuser.Width, Resources.defuser.Height));
      _defuserIndicator.HorizontalAlignment = HorizontalAlignment.Left;
      _defuserIndicator.VerticalAlignment = VerticalAlignment.Top;
      topPanel.Widgets.Add(_defuserIndicator);

      Stylesheet.Current.LabelStyle.Font = Resources.alertFont;
      _actionIndicator = new Label();
      _actionIndicator.Text = "";
      _actionIndicator.TextColor = Color.DarkRed;
      _actionIndicator.HorizontalAlignment = HorizontalAlignment.Center;
      _actionIndicator.VerticalAlignment = VerticalAlignment.Center;
      topPanel.Widgets.Add(_actionIndicator);
      _actionTimer.addAction(TimerDirection.Forward, 1500f, TimeoutBehaviour.Reset, () => { _actionIndicator.Text = ""; });

      _ui.Widgets.Add(topPanel);
      // Bottom Panel
      Panel bottomPanel = new Panel();

      Stylesheet.Current.HorizontalProgressBarStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.HorizontalProgressBarStyle.Filler = new ColoredRegion(DefaultAssets.WhiteRegion, Color.LimeGreen);
      _reloadIndicator = new HorizontalProgressBar();
      _reloadIndicator.HorizontalAlignment = HorizontalAlignment.Right;
      _reloadIndicator.VerticalAlignment = VerticalAlignment.Bottom;
      _reloadIndicator.Width = (int)(Options.Config.Width * _proportionsX);
      _reloadIndicator.Height = 20;
      _reloadIndicator.Left = -50;
      bottomPanel.Widgets.Add(_reloadIndicator);

      _ui.Widgets.Add(bottomPanel);
    }

    public void addIndicator(Widget indicator)
    {
      _ui.Widgets.Add(indicator);
    }

    public void removeIndicator(Widget indicator)
    {
      _ui.Widgets.Remove(indicator);
    }

    public Rectangle getTopLeftRectangle()
    {
      return (new Rectangle(0, 0, (int)(Options.Config.Width * _proportionsX), 100));
    }

    public void triggerBloodsplat()
    {
      _bloodsplat = true;
      _bloodsplatPos = new Rectangle(_randomGenerator.Next(0, Options.Config.Width - Resources.bloodsplat.Width),
                                     _randomGenerator.Next(0, Options.Config.Height - Resources.bloodsplat.Height),
                                     Resources.bloodsplat.Width, Resources.bloodsplat.Height);
      _bloodsplatTimer.Start();
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

    public void updateDefuser(bool state)
    {
      _defuserIndicator.Opacity = (state ? 1 : 0);
    }


    public void updateMagazine(int ammo)
    {
      _ammo = ammo;
    }

    public void Update(GameTime gameTime, DeviceState state, DeviceState prevState)
    {
      crosshair.Update(gameTime, state, prevState);
      contractsPanel.Update();

      _actionTimer.Update(gameTime);
      _bloodsplatTimer.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      crosshair.Draw(spriteBatch);
      for (int ammo = _ammo; ammo >= 1; --ammo)
        spriteBatch.Draw(Resources.bullet, new Rectangle(Options.Config.Width - (32 + ammo * 16), Options.Config.Height - 52, 32, 32), Color.DimGray);
      if (_bloodsplat) spriteBatch.Draw(Resources.bloodsplat, _bloodsplatPos, Color.Red);
    }

    public void DrawUI()
    {
      Desktop.Render();
    }
  }
}
