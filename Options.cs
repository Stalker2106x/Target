using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Target
{
  public class GameSettings
  {
    const string ConfigPath = "Content/Config.cfg";
    public bool Fullscreen { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public float MouseSensivity { get; set; }
    public float MusicVolume { get; set; }
    public float SoundVolume { get; set; }

    public Dictionary<GameAction, ControlPair> Bindings;
    public GameSettings()
    {
        Fullscreen = false;
        Width = Options.Resolutions[0].Width;
        Height = Options.Resolutions[0].Height;
        MouseSensivity = 1.0f;
        MusicVolume = 1.0f;
        SoundVolume = 1.0f;
        DefaultBindings();
    }

    public void DefaultBindings()
    {
      Bindings = new Dictionary<GameAction, ControlPair>();
      Bindings.Add(GameAction.Fire, new ControlPair(new Control(MouseButton.Left), new Control(Buttons.RightShoulder)));
      Bindings.Add(GameAction.Reload, new ControlPair(new Control(Keys.R), new Control(Buttons.X)));
      Bindings.Add(GameAction.HoldBreath, new ControlPair(new Control(Keys.LeftShift), new Control(Buttons.LeftShoulder)));
      Bindings.Add(GameAction.Menu, new ControlPair(new Control(Keys.Escape), new Control(Buttons.Start)));
    }

    public static GameSettings Load()
    {
      if (!File.Exists(ConfigPath)) return (new GameSettings());
      return (JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText(ConfigPath)));
    }
    public void Save()
    {
      File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(this));
    }
  }

    public class Options
    {
        public static GraphicsDeviceManager GDevice { get; set; }
        public static GraphicsAdapter GAdapter { get; set; }
        public static List<DisplayMode> Resolutions { get; set; }

        public static GameSettings Config { get; set; }

        public Options(GraphicsDeviceManager gdevice, GraphicsAdapter gadapter)
        {
            GDevice = gdevice;
            Resolutions = new List<DisplayMode>();
            GAdapter = gadapter;
            LoadResolutions();
            Config = GameSettings.Load();
        }

        public void LoadResolutions()
        {
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Resolutions.Add(mode);
            }
        }

        public static void applyConfig()
        {

            setResolution();
            GDevice.ApplyChanges();
            GDevice.IsFullScreen = Config.Fullscreen;
            GDevice.ApplyChanges();
            MediaPlayer.Volume = Options.Config.MusicVolume;
    }

        public static List<String> getResolutions()
        {
            List<String> resolutions = new List<String>();

            for (int i = 0; i < Resolutions.Count; i++)
                resolutions.Add(Resolutions[i].Width + "x" + Resolutions[i].Height);
            return (resolutions);
        }

        public static int getDisplayMode()
        {
            for (int i = 0; i < Resolutions.Count; i++)
            {
                if (Resolutions[i].Width == Config.Width && Resolutions[i].Height == Config.Height)
                    return (i);
            }
            return (0);
        }

        public static void setResolution()
        {
            GDevice.PreferredBackBufferWidth = Config.Width;
            GDevice.PreferredBackBufferHeight = Config.Height;
        }

        public static void setFullscreen(bool active)
        {
            GDevice.IsFullScreen = active;
        }
    }
}
