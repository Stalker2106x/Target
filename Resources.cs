// Decompiled with JetBrains decompiler
// Type: Target.Resources
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Target.Utils;

namespace Target
{
  class Resources
  {
    public static SpriteFont titleFont;
    public static SpriteFont regularFont;
    public static SpriteFont alertFont;

    public static List<Target> targets;

    public static Texture2D menuBackground;
    public static Texture2D hitmarker;
    public static Texture2D bloodsplat;
    public static Texture2D reloadingCursor;
    public static Texture2D crosshair;
    public static Texture2D bullet;

    public static Texture2D itemHealth;
    public static Texture2D itemFastReload;
    public static Texture2D itemPoints;
    public static Texture2D itemSpawnReducer;
    public static Texture2D itemNuke;
    public static Texture2D itemContract;

    public static Texture2D mapWoods;

    public static Song menuTheme;
    public static SoundEffect menuClick;
    public static SoundEffect fire;
    public static SoundEffect burst;
    public static SoundEffect reload;
    public static SoundEffect cash;
    public static SoundEffect fail; 
    public static SoundEffect unstoppable;
    public static SoundEffect headhunter;
    public static SoundEffect headshot;
    public static SoundEffect breath;
    public static SoundEffect outbreath;
    public static SoundEffect heartbeat;
    public static SoundEffect pain1;
    public static SoundEffect pain2;

    public static void LoadContent(ContentManager content)
    {
      //UI
      Resources.titleFont = content.Load<SpriteFont>("Font/title");
      Resources.regularFont = content.Load<SpriteFont>("Font/regular");
      Resources.alertFont = content.Load<SpriteFont>("Font/alert");
      Resources.menuBackground = content.Load<Texture2D>("GFX/GUI/main");

      //Enemy
      Resources.targets = JsonConvert.DeserializeObject<List<Target>>(File.ReadAllText("Content/Data/targets.json"));
      foreach (var it in Resources.targets)
      {
        GameMain.targetsProbability.AddRange(it.spawn.probability);
        loadTargetResource(content, it);
      }
      //Bonus
      Resources.itemHealth = content.Load<Texture2D>("GFX/Item/health");
      Resources.itemFastReload = content.Load<Texture2D>("GFX/Item/fastReload");
      Resources.itemPoints = content.Load<Texture2D>("GFX/Item/points");
      Resources.itemSpawnReducer = content.Load<Texture2D>("GFX/Item/time");
      Resources.itemNuke = content.Load<Texture2D>("GFX/Item/nuke");
      Resources.itemContract = content.Load<Texture2D>("GFX/Item/contract");

      //HUD
      Resources.hitmarker = content.Load<Texture2D>("GFX/GUI/hitmarker");
      Resources.bloodsplat = content.Load<Texture2D>("GFX/Player/bloodsplat");
      Resources.reloadingCursor = content.Load<Texture2D>("GFX/Player/reloading");
      Resources.crosshair = content.Load<Texture2D>("GFX/Player/crosshair");
      Resources.bullet = content.Load<Texture2D>("GFX/GUI/bullet");
      
      //Maps
      Resources.mapWoods = content.Load<Texture2D>("GFX/Maps/woods");

      //Sounds
      Resources.menuTheme = content.Load<Song>("Sound/GUI/menu_theme");
      Resources.menuClick = content.Load<SoundEffect>("Sound/GUI/click");
      Resources.fire = content.Load<SoundEffect>("Sound/Weapons/fire");
      Resources.burst = content.Load<SoundEffect>("Sound/Weapons/burst");
      Resources.reload = content.Load<SoundEffect>("Sound/Weapons/reload");
      Resources.cash = content.Load<SoundEffect>("Sound/Target/cash");
      Resources.fail = content.Load<SoundEffect>("Sound/Target/fail");
      Resources.unstoppable = content.Load<SoundEffect>("Sound/Target/unstoppable");
      Resources.headhunter = content.Load<SoundEffect>("Sound/Target/headhunter");
      Resources.headshot = content.Load<SoundEffect>("Sound/Target/headshot");
      Resources.breath = content.Load<SoundEffect>("Sound/Player/breath");
      Resources.outbreath = content.Load<SoundEffect>("Sound/Player/outbreath");
      Resources.heartbeat = content.Load<SoundEffect>("Sound/Player/heartbeat");
      Resources.pain1 = content.Load<SoundEffect>("Sound/Player/pain1");
      Resources.pain2 = content.Load<SoundEffect>("Sound/Player/pain2");
    }

    public static void loadTargetResource(ContentManager content, Target target)
    {
      TargetResource resource = new TargetResource();

      resource.idle = content.Load<Texture2D>("GFX/Target/" + target.name.ToLower() + "/idle");
      resource.idle_hitbox = content.Load<Texture2D>("GFX/Target/" + target.name.ToLower() + "/idle_hitbox");
      if (target.damage > 0)
      {
        resource.firing = content.Load<Texture2D>("GFX/Target/" + target.name.ToLower() + "/firing");
        resource.firing_hitbox = content.Load<Texture2D>("GFX/Target/" + target.name.ToLower() + "/firing_hitbox");
      }
      target.resource = resource;
    }
  }
}
