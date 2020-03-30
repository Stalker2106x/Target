// Decompiled with JetBrains decompiler
// Type: Target.GameMenu
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using System.Collections.Generic;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;
using Myra.Graphics2D.TextureAtlases;
using Myra;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Threading;
using Myra.Graphics2D;
using TargetGame.Settings;

namespace TargetGame
{
  public static class Menu
  {
    /// <summary>
    /// Load default styles for menus
    /// </summary>
    public static void LoadUIStylesheet()
    {
      Stylesheet.Current.ButtonStyle.Width = 500;
      Stylesheet.Current.ButtonStyle.Background = new ColoredRegion(DefaultAssets.WhiteRegion, Color.Transparent);
      Stylesheet.Current.ButtonStyle.OverBackground = new ColoredRegion(DefaultAssets.WhiteRegion, new Color(255, 0, 0, 0.1f));
      Stylesheet.Current.ButtonStyle.PressedBackground = new ColoredRegion(DefaultAssets.WhiteRegion, new Color(255, 0, 0, 0.2f));
      Stylesheet.Current.TextBoxStyle.Font = Resources.regularFont;
      Stylesheet.Current.ComboBoxStyle.LabelStyle.Font = Resources.regularFont;
      Stylesheet.Current.ComboBoxStyle.Width = 200;
      Stylesheet.Current.HorizontalSliderStyle.Width = 200;
      Stylesheet.Current.LabelStyle.Font = Resources.titleFont;
    }

    /// <summary>
    /// Inject version footer to current Desktop
    /// </summary>
    public static void AddVersionFooter()
    {

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      Label version = new Label();
      version.HorizontalAlignment = HorizontalAlignment.Right;
      version.VerticalAlignment = VerticalAlignment.Bottom;
      version.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

      Desktop.Widgets.Add(version);
    }

    /// <summary>
    /// Main Menu loader
    /// </summary>
    public static void MainMenu()
    {
      LoadUIStylesheet();
      Desktop.Widgets.Clear();

      Panel panel = new Panel();
      panel.Background = new TextureRegion(Resources.menuBackground);

      VerticalStackPanel grid = new VerticalStackPanel();
      grid.VerticalAlignment = VerticalAlignment.Center;
      grid.HorizontalAlignment = HorizontalAlignment.Center;

      grid.Spacing = 8;

      Label title = new Label();
      title.Text = "(Target)";
      title.Padding = new Thickness(0, 0, 0, 100);
      grid.Widgets.Add(title);

      TextButton playBtn = new TextButton();
      playBtn.Text = "Play";
      playBtn.Click += (s, a) =>
      {
          GameMain.resetGame(GameState.Playing);
      };
      grid.Widgets.Add(playBtn);

      TextButton tutorialBtn = new TextButton();
      tutorialBtn.Text = "Tutorial";
      tutorialBtn.Click += (s, a) =>
      {
        GameMain.resetGame(GameState.Tutorial);
      };
      grid.Widgets.Add(tutorialBtn);

      TextButton optionsBtn = new TextButton();
      optionsBtn.Text = "Options";
      optionsBtn.Click += (s, a) =>
      {
          OptionsMenu(MainMenu);
      };
      grid.Widgets.Add(optionsBtn);

      TextButton quitBtn = new TextButton();
      quitBtn.Text = "Exit";
      quitBtn.Click += (s, a) =>
      {
          GameEngine.quit = true;
      };
      grid.Widgets.Add(quitBtn);

      panel.Widgets.Add(grid);
      Desktop.Root = panel;
      AddVersionFooter();
    }

    /// <summary>
    /// Options loader
    /// </summary>
    public static void OptionsMenu(Action prevMenu)
    {
      LoadUIStylesheet();
      Desktop.Widgets.Clear();

      Panel panel = new Panel();
      panel.Background = new TextureRegion(Resources.menuBackground);

      Grid grid = new Grid();
      int gridRow = 0;
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
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 25));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 25));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));

      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;

      Label resolutionLabel = new Label();
      resolutionLabel.Text = "Resolution";
      resolutionLabel.GridColumn = 1;
      resolutionLabel.GridRow = gridRow;
      resolutionLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(resolutionLabel);

      ComboBox resolutionCombo = new ComboBox();
      resolutionCombo.GridColumn = 2;
      resolutionCombo.GridRow = gridRow;
      resolutionCombo.HorizontalAlignment = HorizontalAlignment.Right;
      List<string> resolutions = Options.getResolutions();
      resolutions.ForEach(e => { resolutionCombo.Items.Add(new ListItem(e)); });
      resolutionCombo.SelectedIndex = Options.getDisplayMode();
      grid.Widgets.Add(resolutionCombo);
      gridRow++; //Next row

      Label displayLabel = new Label();
      displayLabel.Text = "Display mode";
      displayLabel.GridColumn = 1;
      displayLabel.GridRow = gridRow;
      displayLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(displayLabel);

      ComboBox displayCombo = new ComboBox();
      displayCombo.GridColumn = 2;
      displayCombo.GridRow = gridRow;
      displayCombo.HorizontalAlignment = HorizontalAlignment.Right;
      displayCombo.Items.Add(new ListItem("Windowed"));
      displayCombo.Items.Add(new ListItem("Fullscreen"));
      displayCombo.SelectedIndex = Convert.ToInt32(Options.Config.Fullscreen);
      grid.Widgets.Add(displayCombo);
      gridRow++; //Next row


      Label sensivityLabel = new Label();
      sensivityLabel.Text = "Mouse sensivity";
      sensivityLabel.GridColumn = 1;
      sensivityLabel.GridRow = gridRow;
      sensivityLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(sensivityLabel);

      HorizontalSlider sensivitySlider = new HorizontalSlider();
      sensivitySlider.GridColumn = 2;
      sensivitySlider.GridRow = gridRow;
      sensivitySlider.HorizontalAlignment = HorizontalAlignment.Right;
      sensivitySlider.Minimum = 0.1f;
      sensivitySlider.Maximum = 10f;
      sensivitySlider.Value = Options.Config.MouseSensivity;
      grid.Widgets.Add(sensivitySlider);
      gridRow++; //Next row

      Label musicLabel = new Label();
      musicLabel.Text = "Music volume";
      musicLabel.GridColumn = 1;
      musicLabel.GridRow = gridRow;
      musicLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(musicLabel);

      HorizontalSlider musicSlider = new HorizontalSlider();
      musicSlider.GridColumn = 2;
      musicSlider.GridRow = gridRow;
      musicSlider.HorizontalAlignment = HorizontalAlignment.Right;
      musicSlider.Minimum = 0f;
      musicSlider.Maximum = 1f;
      musicSlider.Value = Options.Config.MusicVolume;
      grid.Widgets.Add(musicSlider);
      gridRow++; //Next row

      Label soundLabel = new Label();
      soundLabel.Text = "Sounds volume";
      soundLabel.GridColumn = 1;
      soundLabel.GridRow = 4;
      soundLabel.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Widgets.Add(soundLabel);

      HorizontalSlider soundSlider = new HorizontalSlider();
      soundSlider.GridColumn = 2;
      soundSlider.GridRow = gridRow;
      soundSlider.HorizontalAlignment = HorizontalAlignment.Right;
      soundSlider.Minimum = 0f;
      soundSlider.Maximum = 1f;
      soundSlider.Value = Options.Config.SoundVolume;
      grid.Widgets.Add(soundSlider);
      gridRow++; //Next row

      gridRow++; //Skip row

      TextButton bindigsBtn = new TextButton();
      bindigsBtn.Text = "Bindings";
      bindigsBtn.GridColumn = 1;
      bindigsBtn.GridRow = gridRow;
      bindigsBtn.GridColumnSpan = 2;
      bindigsBtn.HorizontalAlignment = HorizontalAlignment.Center;
      bindigsBtn.Click += (s, a) =>
      {
        BindingsMenu(prevMenu);
      };
      grid.Widgets.Add(bindigsBtn);
      gridRow++; //Next row

      gridRow++; //Skip row
      Stylesheet.Current.LabelStyle.Font = Resources.titleFont;

      TextButton applyBtn = new TextButton();
      applyBtn.Text = "Apply";
      applyBtn.GridColumn = 1;
      applyBtn.GridRow = gridRow;
      applyBtn.GridColumnSpan = 2;
      applyBtn.HorizontalAlignment = HorizontalAlignment.Center;
      applyBtn.Click += (s, a) =>
      {
          if (displayCombo.SelectedItem == null || resolutionCombo.SelectedItem == null)
          {
              var messageBox = Dialog.CreateMessageBox("Error", "You must select a value for display/resolution!");
              messageBox.ShowModal();
              return;
          }
          Options.Config.Fullscreen = Convert.ToBoolean(displayCombo.SelectedIndex);
          Options.Config.Width = Options.Resolutions[(int)resolutionCombo.SelectedIndex].Width;
          Options.Config.Height = Options.Resolutions[(int)resolutionCombo.SelectedIndex].Height;
          Options.Config.MouseSensivity = sensivitySlider.Value;
          Options.Config.MusicVolume = musicSlider.Value;
          Options.Config.SoundVolume = soundSlider.Value;
          Options.applyConfig();
          Options.Config.Save();
          prevMenu();
      };
      grid.Widgets.Add(applyBtn);
      gridRow++; //Next row

      TextButton backBtn = new TextButton();
      backBtn.Text = "Cancel";
      backBtn.GridColumn = 1;
      backBtn.GridRow = gridRow;
      backBtn.GridColumnSpan = 2;
      backBtn.HorizontalAlignment = HorizontalAlignment.Center;
      backBtn.Click += (s, a) =>
      {
          prevMenu();
      };
      grid.Widgets.Add(backBtn);

      panel.Widgets.Add(grid);
      Desktop.Root = panel;
    }

    /// <summary>
    /// Bindings Menu loader
    /// </summary>
    public static void BindingsMenu(Action optionsPrevMenu)
    {
      LoadUIStylesheet();
      Desktop.Widgets.Clear();

      Panel panel = new Panel();
      panel.Background = new TextureRegion(Resources.menuBackground);

      Grid grid = new Grid();
      grid.VerticalAlignment = VerticalAlignment.Center;

      grid.RowSpacing = 8;

      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      for (int bindCount = 0; bindCount < Options.Config.Bindings.Count; bindCount++)
        grid.RowsProportions.Add(new Proportion(ProportionType.Part));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 75));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));
      grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 60));

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
      foreach (KeyValuePair<GameAction, ControlPair> entry in Options.Config.Bindings)
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
            Control? control;
            int attempts = 30;
            do {
              control = Control.GetAnyPressedKey(GameEngine.getDeviceState());
              Thread.Sleep(100);
              attempts--;
            } while (control == null && attempts > 0);
            if (control != null)
            {
              var bind = Options.Config.Bindings[entry.Key];
              bind.primary = control;
              Options.Config.Bindings[entry.Key] = bind;
            }
            primaryBind.Text = Options.Config.Bindings[entry.Key].primary.GetValueOrDefault().GetInput();
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
            Control? control;
            int attempts = 30;
            do
            {
              control = Control.GetAnyPressedKey(GameEngine.getDeviceState());
              Thread.Sleep(100);
              attempts--;
            } while (control == null && attempts > 0);
            if (control != null)
            {
              var bind = Options.Config.Bindings[entry.Key];
              bind.secondary = control;
              Options.Config.Bindings[entry.Key] = bind;
            }
            secondaryBind.Text = Options.Config.Bindings[entry.Key].secondary.GetValueOrDefault().GetInput();
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
        OptionsMenu(optionsPrevMenu);
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
        OptionsMenu(optionsPrevMenu);
      };
      grid.Widgets.Add(backBtn);

      panel.Widgets.Add(grid);
      Desktop.Root = panel;
    }

    /// <summary>
    /// In Game Pause Menu loader
    /// </summary>
    public static void GameMenu()
    {
      LoadUIStylesheet();
      Desktop.Widgets.Clear();

      Panel panel = new Panel();
      panel.Background = new TextureRegion(Resources.menuBackground);

      VerticalStackPanel grid = new VerticalStackPanel();
      grid.VerticalAlignment = VerticalAlignment.Center;
      grid.HorizontalAlignment = HorizontalAlignment.Center;

      grid.Spacing = 8;

      Label title = new Label();
      title.Text = "Paused";
      title.Padding = new Thickness(0, 0, 0, 100);
      title.HorizontalAlignment = HorizontalAlignment.Center;
      grid.Widgets.Add(title);

      TextButton resumeBtn = new TextButton();
      resumeBtn.Text = "Resume";
      resumeBtn.Click += (s, a) =>
      {
          GameEngine.setState(GameMain.tutorial != null ? GameState.Tutorial : GameState.Playing);
      };
      grid.Widgets.Add(resumeBtn);

      TextButton optionsBtn = new TextButton();
      optionsBtn.Text = "Options";
      optionsBtn.Click += (s, a) =>
      {
          OptionsMenu(GameMenu);
      };
      grid.Widgets.Add(optionsBtn);

      TextButton quitBtn = new TextButton();
      quitBtn.Text = "Quit";
      quitBtn.Click += (s, a) =>
      {
          MainMenu();
      };
      grid.Widgets.Add(quitBtn);

      panel.Widgets.Add(grid);
      Desktop.Root = panel;
    }

    /// <summary>
    /// Game Over summary Menu loader
    /// </summary>
    public static void GameOverMenu(string content)
    {
      AddVersionFooter();
      LoadUIStylesheet();
      Desktop.Widgets.Clear();

      Panel panel = new Panel();
      panel.Background = new TextureRegion(Resources.menuBackground);

      VerticalStackPanel grid = new VerticalStackPanel();
      grid.VerticalAlignment = VerticalAlignment.Center;
      grid.HorizontalAlignment = HorizontalAlignment.Center;

      grid.Spacing = 8;

      Label title = new Label();
      title.Text = "(Game Over)";
      title.Padding = new Thickness(0, 0, 0, 100);
      grid.Widgets.Add(title);

      Label detailsText = new Label();
      detailsText.Text = content;
      detailsText.Font = Resources.regularFont;
      detailsText.Padding = new Thickness(0, 0, 0, 100);
      grid.Widgets.Add(detailsText);

      TextButton continueBtn = new TextButton();
      continueBtn.Text = "Continue";
      continueBtn.HorizontalAlignment = HorizontalAlignment.Center;
      continueBtn.Click += (s, a) =>
      {
          MainMenu();
      };
      grid.Widgets.Add(continueBtn);

      panel.Widgets.Add(grid);
      Desktop.Root = panel;
    }
  }
}
