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

namespace Target
{
    public static class Menu
    {
        public static void LoadContent(ContentManager content)
        {
            /*SpriteFont font = content.Load<SpriteFont>("font/general");
            Stylesheet.Current.TextBoxStyle.Font = font;*/
            Stylesheet.Current.ButtonStyle.Width = 100;
            Stylesheet.Current.ComboBoxStyle.Width = 100;
            //Stylesheet.Current.TextFieldStyle.Width = 100;
        }

        public static void MainMenu(Desktop host)
        {
            host.Widgets.Clear();
            Grid grid = new Grid();

            grid.RowSpacing = 8;

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 200));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));

            TextButton hostBtn = new TextButton();
            hostBtn.Text = "Play";
            hostBtn.GridColumn = 1;
            hostBtn.GridRow = 1;
            hostBtn.HorizontalAlignment = HorizontalAlignment.Center;
            hostBtn.Click += (s, a) =>
            {
                GameMain.resetGame();
                Game1.gameState = GameState.Playing;
            };
            grid.Widgets.Add(hostBtn);

            TextButton optionsBtn = new TextButton();
            optionsBtn.Text = "Options";
            optionsBtn.GridColumn = 1;
            optionsBtn.GridRow = 3;
            optionsBtn.HorizontalAlignment = HorizontalAlignment.Center;
            optionsBtn.Click += (s, a) =>
            {
                OptionsMenu(host, MainMenu);
            };
            grid.Widgets.Add(optionsBtn);

            TextButton quitBtn = new TextButton();
            quitBtn.Text = "Quit";
            quitBtn.GridColumn = 1;
            quitBtn.GridRow = 4;
            quitBtn.HorizontalAlignment = HorizontalAlignment.Center;
            quitBtn.Click += (s, a) =>
            {
                Game1.quit = true;
            };
            grid.Widgets.Add(quitBtn);

            host.Widgets.Add(grid);
        }
        
        public static void OptionsMenu(Desktop host, Action<Desktop> prevMenu)
        {
            host.Widgets.Clear();
            Grid grid = new Grid();

            grid.RowSpacing = 8;

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 200));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 200));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));


            TextBox resolutionLabel = new TextBox();
            resolutionLabel.Text = "Resolution";
            resolutionLabel.GridColumn = 1;
            resolutionLabel.GridRow = 1;
            resolutionLabel.HorizontalAlignment = HorizontalAlignment.Left;
            grid.Widgets.Add(resolutionLabel);

            ComboBox resolutionCombo = new ComboBox();
            resolutionCombo.GridColumn = 2;
            resolutionCombo.GridRow = 1;
            resolutionCombo.HorizontalAlignment = HorizontalAlignment.Right;
            List<string> resolutions = Options.getResolutions();
            resolutions.ForEach(e => { resolutionCombo.Items.Add(new ListItem(e)); });
            resolutionCombo.SelectedIndex = Options.getDisplayMode();
            grid.Widgets.Add(resolutionCombo);

            TextBox displayLabel = new TextBox();
            displayLabel.Text = "Display mode";
            displayLabel.GridColumn = 1;
            displayLabel.GridRow = 2;
            displayLabel.HorizontalAlignment = HorizontalAlignment.Left;
            grid.Widgets.Add(displayLabel);

            ComboBox displayCombo = new ComboBox();
            displayCombo.GridColumn = 2;
            displayCombo.GridRow = 2;
            displayCombo.HorizontalAlignment = HorizontalAlignment.Right;
            displayCombo.Items.Add(new ListItem("Windowed"));
            displayCombo.Items.Add(new ListItem("Fullscreen"));
            displayCombo.SelectedIndex = Convert.ToInt32(Options.Config.Fullscreen);
            grid.Widgets.Add(displayCombo);

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
            grid.Widgets.Add(soundSlider);

            TextButton applyBtn = new TextButton();
            applyBtn.Text = "Apply";
            applyBtn.GridColumn = 1;
            applyBtn.GridRow = 5;
            applyBtn.GridColumnSpan = 2;
            applyBtn.HorizontalAlignment = HorizontalAlignment.Center;
            applyBtn.Click += (s, a) =>
            {
                if (displayCombo.SelectedItem == null || resolutionCombo.SelectedItem == null)
                {
                    var messageBox = Dialog.CreateMessageBox("Error", "You must select a value for display/resolution!");
                    messageBox.ShowModal(host);
                    return;
                }
                Options.Config.Fullscreen = Convert.ToBoolean(displayCombo.SelectedIndex);
                Options.Config.Width = Options.Resolutions[(int)resolutionCombo.SelectedIndex].Width;
                Options.Config.Height = Options.Resolutions[(int)resolutionCombo.SelectedIndex].Height;
                Options.Config.MusicVolume = musicSlider.Value;
                Options.Config.SoundVolume = soundSlider.Value;
                Options.applyConfig();
                Options.SetConfigFile();
                OptionsMenu(host, prevMenu);
            };
            grid.Widgets.Add(applyBtn);

            TextButton backBtn = new TextButton();
            backBtn.Text = "Back";
            backBtn.GridColumn = 1;
            backBtn.GridRow = 7;
            backBtn.GridColumnSpan = 2;
            backBtn.HorizontalAlignment = HorizontalAlignment.Center;
            backBtn.Click += (s, a) =>
            {
                prevMenu(host);
            };
            grid.Widgets.Add(backBtn);

            host.Widgets.Add(grid);
        }

        public static void GameMenu(Desktop host)
        {
            host.Widgets.Clear();
            Grid grid = new Grid();

            grid.RowSpacing = 8;

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 200));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));

            TextButton resumeBtn = new TextButton();
            resumeBtn.Text = "Resume";
            resumeBtn.GridColumn = 1;
            resumeBtn.GridRow = 1;
            resumeBtn.HorizontalAlignment = HorizontalAlignment.Center;
            resumeBtn.Click += (s, a) =>
            {
                Game1.gameState = GameState.Playing;
            };
            grid.Widgets.Add(resumeBtn);

            TextButton optionsBtn = new TextButton();
            optionsBtn.Text = "Options";
            optionsBtn.GridColumn = 1;
            optionsBtn.GridRow = 3;
            optionsBtn.HorizontalAlignment = HorizontalAlignment.Center;
            optionsBtn.Click += (s, a) =>
            {
                OptionsMenu(host, GameMenu);
            };
            grid.Widgets.Add(optionsBtn);

            TextButton quitBtn = new TextButton();
            quitBtn.Text = "End game";
            quitBtn.GridColumn = 1;
            quitBtn.GridRow = 4;
            quitBtn.HorizontalAlignment = HorizontalAlignment.Center;
            quitBtn.Click += (s, a) =>
            {
                MainMenu(host);
            };
            grid.Widgets.Add(quitBtn);

            host.Widgets.Add(grid);
        }

        public static void GameOverMenu(Desktop host, string content)
        {
            host.Widgets.Clear();
            Grid grid = new Grid();

            grid.RowSpacing = 8;

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 200));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion(ProportionType.Part));

            TextBox detailsText = new TextBox();
            detailsText.Text = content;
            detailsText.GridColumn = 1;
            detailsText.GridRow = 1;
            detailsText.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.Widgets.Add(detailsText);

            TextButton optionsBtn = new TextButton();
            optionsBtn.Text = "Continue";
            optionsBtn.GridColumn = 1;
            optionsBtn.GridRow = 2;
            optionsBtn.HorizontalAlignment = HorizontalAlignment.Center;
            optionsBtn.Click += (s, a) =>
            {
                MainMenu(host);
            };
            grid.Widgets.Add(optionsBtn);

            host.Widgets.Add(grid);
        }
    }
}
