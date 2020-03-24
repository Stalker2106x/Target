using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Target
{
    public class GameSettings
    {
        public bool Fullscreen { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float MouseSensivity { get; set; }
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }

        public GameSettings()
        {
            Fullscreen = false;
            Width = Options.Resolutions[0].Width;
            Height = Options.Resolutions[0].Height;
            MouseSensivity = 1.0f;
            MusicVolume = 1.0f;
            SoundVolume = 1.0f;
        }

        public GameSettings(bool fullscreen, int width, int height, float musicVolume, float soundVolume)
        {
            Fullscreen = fullscreen;
            Width = width;
            Height = height;
            MusicVolume = musicVolume;
            SoundVolume = soundVolume;
        }
    }

    public class Options
    {
        static String _configfile;
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
            Config = new GameSettings();
            _configfile = "./Content/Config.cfg";
            LoadConfigFile();
        }

        public void LoadConfigFile()
        {
            StreamReader file;
            String line;

            try { file = new StreamReader(_configfile); }
            catch (System.IO.IOException)
            {
                return;
            }
            while ((line = file.ReadLine()) != null)
            {
                setSetting(line.Substring(0, line.IndexOf('=')), line.Substring(line.IndexOf('=') + 1, line.Length - line.IndexOf('=') - 1));
            }
            file.Close();
            applyConfig();
        }

        public static void SetConfigFile()
        {
            var fs = new StreamWriter(_configfile);
            fs.WriteLine("fullscreen=" + (Config.Fullscreen == true ? "true" : "false"));
            fs.WriteLine("width=" + Config.Width);
            fs.WriteLine("height=" + Config.Height);
            fs.WriteLine("sensivity=" + Config.MouseSensivity);
            fs.WriteLine("musicvolume=" + Config.MusicVolume);
            fs.WriteLine("soundvolume=" + Config.SoundVolume);
            fs.Close();
        }

        public void setSetting(String setting, String value)
        {
            switch (setting)
            {
                case "fullscreen":
                    Config.Fullscreen = (value == "true" ? true : false);
                    break;
                case "width":
                    int width;
                    Int32.TryParse(value, out width);
                    Config.Width = width;
                    break;
                case "height":
                    int height;

                    Int32.TryParse(value, out height);
                    Config.Height = height;
                    break;
                case "sensivity":
                    float sensivity;

                    float.TryParse(value, out sensivity);
                    Config.MouseSensivity = sensivity;
                    break;
                case "soundvolume":
                    float soundvol;

                    float.TryParse(value, out soundvol);
                    Config.SoundVolume = soundvol;
                    break;
                case "musicvolume":
                    float musicvol;

                    float.TryParse(value, out musicvol);
                    Config.MusicVolume = musicvol;
                    break;
            }
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
