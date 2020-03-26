// Decompiled with JetBrains decompiler
// Type: Target.GameMenu
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using System.Collections.Generic;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using Microsoft.Xna.Framework.Content;
using System;
using Myra.Graphics2D.TextureAtlases;
using Myra;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Threading;

namespace Target
{
  public static class Menu
  {
    public static Action<GameTime, DeviceState> Update = null; //Update method to override

    public static void LoadUIStylesheet()
    {
      Stylesheet.Current.ButtonStyle.Width = 500;
      Stylesheet.Current.ButtonStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.ButtonStyle.OverBackground = new ColoredRegion(DefaultAssets.WhiteRegion, new Color(255, 0, 0, 0.1f));
      Stylesheet.Current.ButtonStyle.PressedBackground = new ColoredRegion(DefaultAssets.WhiteRegion, new Color(255, 0, 0, 0.2f));
      Stylesheet.Current.ComboBoxStyle.Width = 100;
      Stylesheet.Current.LabelStyle.Font = Resources.titleFont;
    }

    public static void AddVersionFooter(Desktop menuUI)
    {

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      Label version = new Label();
      version.HorizontalAlignment = HorizontalAlignment.Right;
      version.VerticalAlignment = VerticalAlignment.Bottom;
      version.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

      menuUI.Widgets.Add(version);
    }

    public static void MainMenu(Desktop menuUI)
    {
      LoadUIStylesheet();
      menuUI.Widgets.Clear();
      VerticalStackPanel grid = new VerticalStackPanel();
      grid.VerticalAlignment = VerticalAlignment.Center;
      grid.HorizontalAlignment = HorizontalAlignment.Center;

      grid.Spacing = 8;

      Image background = new Image();
      background.Renderable = new TextureRegion(Resources.menuBackground);
      menuUI.Widgets.Add(background);

      Label title = new Label();
      title.Text = "(Target)";
      title.PaddingBottom = 100;
      grid.Widgets.Add(title);

      TextButton playBtn = new TextButton();
      playBtn.Text = "Play";
      playBtn.Click += (s, a) =>
      {
          GameMain.resetGame();
          GameEngine.setState(GameState.Playing);
      };
      grid.Widgets.Add(playBtn);

      TextButton optionsBtn = new TextButton();
      optionsBtn.Text = "Options";
      optionsBtn.Click += (s, a) =>
      {
          OptionsMenu(menuUI, MainMenu);
      };
      grid.Widgets.Add(optionsBtn);

      TextButton quitBtn = new TextButton();
      quitBtn.Text = "Exit";
      quitBtn.Click += (s, a) =>
      {
          GameEngine.quit = true;
      };
      grid.Widgets.Add(quitBtn);

      menuUI.Widgets.Add(grid);
      AddVersionFooter(menuUI);
    }
        
    public static void OptionsMenu(Desktop menuUI, Action<Desktop> prevMenu)
    {
      LoadUIStylesheet();
      menuUI.Widgets.Clear();
      Grid grid = new Grid();
      grid.VerticalAlignment = VerticalAlignment.Center;

      grid.RowSpacing = 8;

      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 75));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));

      Image background = new Image();
      background.Renderable = new TextureRegion(Resources.menuBackground);
      menuUI.Widgets.Add(background);

      TextBox resolutionLabel = new TextBox();
      resolutionLabel.Text = "Resolution";
      resolutionLabel.GridColumn = 1;
      resolutionLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(resolutionLabel);

      ComboBox resolutionCombo = new ComboBox();
      resolutionCombo.GridColumn = 2;
      resolutionCombo.GridRow = 0;
      resolutionCombo.HorizontalAlignment = HorizontalAlignment.Right;
      List<string> resolutions = Options.getResolutions();
      resolutions.ForEach(e => { resolutionCombo.Items.Add(new ListItem(e)); });
      resolutionCombo.SelectedIndex = Options.getDisplayMode();
      grid.Widgets.Add(resolutionCombo);

      TextBox displayLabel = new TextBox();
      displayLabel.Text = "Display mode";
      displayLabel.GridColumn = 1;
      displayLabel.GridRow = 1;
      displayLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(displayLabel);

      ComboBox displayCombo = new ComboBox();
      displayCombo.GridColumn = 2;
      displayCombo.GridRow = 1;
      displayCombo.HorizontalAlignment = HorizontalAlignment.Right;
      displayCombo.Items.Add(new ListItem("Windowed"));
      displayCombo.Items.Add(new ListItem("Fullscreen"));
      displayCombo.SelectedIndex = Convert.ToInt32(Options.Config.Fullscreen);
      grid.Widgets.Add(displayCombo);

      TextBox sensivityLabel = new TextBox();
      sensivityLabel.Text = "Mouse sensivity";
      sensivityLabel.GridColumn = 1;
      sensivityLabel.GridRow = 2;
      sensivityLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(sensivityLabel);

      HorizontalSlider sensivitySlider = new HorizontalSlider();
      sensivitySlider.GridColumn = 2;
      sensivitySlider.GridRow = 2;
      sensivitySlider.Width = 100;
      sensivitySlider.HorizontalAlignment = HorizontalAlignment.Right;
      sensivitySlider.Minimum = 0.1f;
      sensivitySlider.Maximum = 10f;
      sensivitySlider.Value = Options.Config.MouseSensivity;
      grid.Widgets.Add(sensivitySlider);

      TextBox musicLabel = new TextBox();
      musicLabel.Text = "Music volume";
      musicLabel.GridColumn = 1;
      musicLabel.GridRow = 3;
      musicLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(musicLabel);

      HorizontalSlider musicSlider = new HorizontalSlider();
      musicSlider.GridColumn = 2;
      musicSlider.GridRow = 3;
      musicSlider.Width = 100;
      musicSlider.HorizontalAlignment = HorizontalAlignment.Right;
      musicSlider.Minimum = 0f;
      musicSlider.Maximum = 1f;
      musicSlider.Value = Options.Config.MusicVolume;
      grid.Widgets.Add(musicSlider);

      TextBox soundLabel = new TextBox();
      soundLabel.Text = "Sounds volume";
      soundLabel.GridColumn = 1;
      soundLabel.GridRow = 4;
      soundLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(soundLabel);

      HorizontalSlider soundSlider = new HorizontalSlider();
      soundSlider.GridColumn = 2;
      soundSlider.GridRow = 4;
      soundSlider.Width = 100;
      soundSlider.HorizontalAlignment = HorizontalAlignment.Right;
      soundSlider.Minimum = 0f;
      soundSlider.Maximum = 1f;
      soundSlider.Value = Options.Config.SoundVolume;
      grid.Widgets.Add(soundSlider);

      TextButton bindigsBtn = new TextButton();
      bindigsBtn.Text = "Bindings";
      bindigsBtn.GridColumn = 1;
      bindigsBtn.GridRow = 6;
      bindigsBtn.GridColumnSpan = 2;
      bindigsBtn.HorizontalAlignment = HorizontalAlignment.Center;
      bindigsBtn.Click += (s, a) =>
      {
        BindingsMenu(menuUI, prevMenu);
      };
      grid.Widgets.Add(bindigsBtn);

      TextButton applyBtn = new TextButton();
      applyBtn.Text = "Apply";
      applyBtn.GridColumn = 1;
      applyBtn.GridRow = 7;
      applyBtn.GridColumnSpan = 2;
      applyBtn.HorizontalAlignment = HorizontalAlignment.Center;
      applyBtn.Click += (s, a) =>
      {
          if (displayCombo.SelectedItem == null || resolutionCombo.SelectedItem == null)
          {
              var messageBox = Dialog.CreateMessageBox("Error", "You must select a value for display/resolution!");
              messageBox.ShowModal(menuUI);
              return;
          }
          Options.Config.Fullscreen = Convert.ToBoolean(displayCombo.SelectedIndex);
          Options.Config.Width = Options.Resolutions[(int)resolutionCombo.SelectedIndex].Width;
          Options.Config.Height = Options.Resolutions[(int)resolutionCombo.SelectedIndex].Height;
          Options.Config.MouseSensivity = sensivitySlider.Value;
          Options.Config.MusicVolume = musicSlider.Value;
          Options.Config.SoundVolume = soundSlider.Value;
          Options.applyConfig();
          Options.SetConfigFile();
          prevMenu(menuUI);
      };
      grid.Widgets.Add(applyBtn);

      TextButton backBtn = new TextButton();
      backBtn.Text = "Cancel";
      backBtn.GridColumn = 1;
      backBtn.GridRow = 8;
      backBtn.GridColumnSpan = 2;
      backBtn.HorizontalAlignment = HorizontalAlignment.Center;
      backBtn.Click += (s, a) =>
      {
          prevMenu(menuUI);
      };
      grid.Widgets.Add(backBtn);

      menuUI.Widgets.Add(grid);
      AddVersionFooter(menuUI);
    }

    public static void BindingsMenu(Desktop menuUI, Action<Desktop> optionsPrevMenu)
    {
      LoadUIStylesheet();
      menuUI.Widgets.Clear();
      Grid grid = new Grid();
      grid.VerticalAlignment = VerticalAlignment.Center;

      grid.RowSpacing = 8;

      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 75));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));

      Image background = new Image();
      background.Renderable = new TextureRegion(Resources.menuBackground);
      menuUI.Widgets.Add(background);

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      //Header
      Label actionLabel = new Label();
      actionLabel.Text = "Action";
      actionLabel.GridColumn = 1;
      actionLabel.GridRow = 0;
      actionLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(actionLabel);

      Label primaryBindLabel = new Label();
      primaryBindLabel.Text = "Primary";
      primaryBindLabel.GridColumn = 2;
      primaryBindLabel.GridRow = 0;
      primaryBindLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(primaryBindLabel);

      Label secondaryBindLabel = new Label();
      secondaryBindLabel.Text = "Secondary";
      secondaryBindLabel.GridColumn = 3;
      secondaryBindLabel.GridRow = 0;
      secondaryBindLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(secondaryBindLabel);

      //Binds
      int i = 1;
      foreach (KeyValuePair<GameAction, ControlPair> entry in Options.Bindings)
      {
        Label action = new Label();
        action.Text = entry.Key.ToString();
        action.GridColumn = 1;
        action.GridRow = i;
        action.HorizontalAlignment = HorizontalAlignment.Left;
        grid.Widgets.Add(action);

        TextButton primaryBind = new TextButton();
        primaryBind.Text = entry.Value.primary.GetValueOrDefault().GetInput();
        primaryBind.GridColumn = 2;
        primaryBind.GridRow = i;
        primaryBind.Click += (s, a) =>
        {
          (new Thread(() => {
            primaryBind.Text = "...";
            Control control;
            int attempts = 30;
            do {
              control = Control.GetAnyPressedKey(GameEngine.getDeviceState());
              Thread.Sleep(100);
              attempts--;
            } while (!control.IsBound() && attempts > 0);
            if (control.GetInput() != "Unbound")
            {
              var bind = Options.Bindings[entry.Key];
              bind.primary = control;
              Options.Bindings[entry.Key] = bind;
            }
            primaryBind.Text = Options.Bindings[entry.Key].primary.GetValueOrDefault().GetInput();
          })).Start();
        };
        grid.Widgets.Add(primaryBind);

        TextButton secondaryBind = new TextButton();
        secondaryBind.Text = entry.Value.secondary.GetValueOrDefault().GetInput();
        secondaryBind.GridColumn = 3;
        secondaryBind.GridRow = i;
        secondaryBind.Click += (s, a) =>
        {
          (new Thread(() => {
            secondaryBind.Text = "...";
            Control control;
            int attempts = 30;
            do
            {
              control = Control.GetAnyPressedKey(GameEngine.getDeviceState());
              Thread.Sleep(100);
              attempts--;
            } while (!control.IsBound() && attempts > 0);
            if (control.GetInput() != "Unbound")
            {
              var bind = Options.Bindings[entry.Key];
              bind.secondary = control;
              Options.Bindings[entry.Key] = bind;
            }
            secondaryBind.Text = Options.Bindings[entry.Key].secondary.GetValueOrDefault().GetInput();
          })).Start();
        };
        grid.Widgets.Add(secondaryBind);

        i++;
      }

      Stylesheet.Current.LabelStyle.Font = Resources.titleFont;
      TextButton applyBtn = new TextButton();
      applyBtn.Text = "Apply";
      applyBtn.GridColumn = 1;
      applyBtn.GridRow = i+1;
      applyBtn.GridColumnSpan = 3;
      applyBtn.HorizontalAlignment = HorizontalAlignment.Center;
      applyBtn.Click += (s, a) =>
      {
        OptionsMenu(menuUI, optionsPrevMenu);
      };
      grid.Widgets.Add(applyBtn);

      TextButton backBtn = new TextButton();
      backBtn.Text = "Cancel";
      backBtn.GridColumn = 1;
      backBtn.GridRow = i + 2;
      backBtn.GridColumnSpan = 3;
      backBtn.HorizontalAlignment = HorizontalAlignment.Center;
      backBtn.Click += (s, a) =>
      {
        OptionsMenu(menuUI, optionsPrevMenu);
      };
      grid.Widgets.Add(backBtn);

      menuUI.Widgets.Add(grid);
      AddVersionFooter(menuUI);
    }

    public static void GameMenu(Desktop menuUI)
    {
      AddVersionFooter(menuUI);
      LoadUIStylesheet();
      menuUI.Widgets.Clear();
      VerticalStackPanel grid = new VerticalStackPanel();
      grid.VerticalAlignment = VerticalAlignment.Center;
      grid.HorizontalAlignment = HorizontalAlignment.Center;

      grid.Spacing = 8;

      Image background = new Image();
      background.Renderable = new TextureRegion(Resources.menuBackground);
      menuUI.Widgets.Add(background);

      Label title = new Label();
      title.Text = "Paused";
      title.PaddingBottom = 100;
      title.HorizontalAlignment = HorizontalAlignment.Center;
      grid.Widgets.Add(title);

      TextButton resumeBtn = new TextButton();
      resumeBtn.Text = "Resume";
      resumeBtn.Click += (s, a) =>
      {
          GameEngine.setState(GameState.Playing);
      };
      grid.Widgets.Add(resumeBtn);

      TextButton optionsBtn = new TextButton();
      optionsBtn.Text = "Options";
      optionsBtn.Click += (s, a) =>
      {
          OptionsMenu(menuUI, GameMenu);
      };
      grid.Widgets.Add(optionsBtn);

      TextButton quitBtn = new TextButton();
      quitBtn.Text = "Quit";
      quitBtn.Click += (s, a) =>
      {
          MainMenu(menuUI);
      };
      grid.Widgets.Add(quitBtn);

      menuUI.Widgets.Add(grid);
    }

    public static void GameOverMenu(Desktop menuUI, string content)
    {
        AddVersionFooter(menuUI);
        LoadUIStylesheet();
        menuUI.Widgets.Clear();
        VerticalStackPanel grid = new VerticalStackPanel();
        grid.VerticalAlignment = VerticalAlignment.Center;
        grid.HorizontalAlignment = HorizontalAlignment.Center;

        grid.Spacing = 8;

        Image background = new Image();
        background.Renderable = new TextureRegion(Resources.menuBackground);
        menuUI.Widgets.Add(background);

        Label title = new Label();
        title.Text = "(Game Over)";
        title.PaddingBottom = 100;
        grid.Widgets.Add(title);

        Label detailsText = new Label();
        detailsText.Text = content;
        detailsText.Font = Resources.regularFont;
        detailsText.PaddingBottom = 100;
        grid.Widgets.Add(detailsText);

        TextButton continueBtn = new TextButton();
        continueBtn.Text = "Continue";
        continueBtn.Click += (s, a) =>
        {
            MainMenu(menuUI);
        };
        grid.Widgets.Add(continueBtn);

        menuUI.Widgets.Add(grid);
        AddVersionFooter(menuUI);
    }
  }
}
