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

    public static Texture2D bomb;
    public static List<Target> targets;
    public static List<Item> items;

    public static Texture2D grabCursor;
    public static Texture2D reloadingCursor;
    public static Texture2D crosshair;
    public static Texture2D defuseCursor;

    public static Texture2D splashScreen;
    public static Texture2D menuBackground;
    public static Texture2D hands;
    public static Texture2D muzzleflash;
    public static Texture2D hitmarker;
    public static Texture2D bloodsplat;
    public static Texture2D bullet;
    public static Texture2D defuser;

    public static Texture2D mapWoods;

    public static Song menuTheme;
    public static SoundEffect menuClick;
    public static SoundEffect fire;
    public static SoundEffect burst;
    public static SoundEffect reload;

    public static SoundEffect cash;
    public static SoundEffect fail;
    public static SoundEffect nuke;
    public static SoundEffect rewind;
    public static SoundEffect contract;
    public static SoundEffect armor;
    public static SoundEffect medikit;

    public static SoundEffect bombtick;
    public static SoundEffect explosion;
    public static SoundEffect defusal;

    public static SoundEffect denied;
    public static SoundEffect unstoppable;
    public static SoundEffect headhunter;
    public static SoundEffect headshot;

    public static SoundEffect breath;
    public static SoundEffect outbreath;
    public static SoundEffect heartbeat;
    public static SoundEffect hit;
    public static SoundEffect pain1;
    public static SoundEffect pain2;

    public static void LoadContent(ContentManager content)
    {
      //UI
      Resources.titleFont = content.Load<SpriteFont>("Font/title");
      Resources.regularFont = content.Load<SpriteFont>("Font/regular");
      Resources.alertFont = content.Load<SpriteFont>("Font/alert");
      Resources.splashScreen = content.Load<Texture2D>("GFX/GUI/splashscreen");
      Resources.menuBackground = content.Load<Texture2D>("GFX/GUI/main");

      //Targets
      Resources.bomb = content.Load<Texture2D>("GFX/Target/bomb");
      Resources.targets = JsonConvert.DeserializeObject<List<Target>>(File.ReadAllText("Content/Data/targets.json"));
      foreach (var it in Resources.targets)
      {
        GameMain.targetsProbability.AddRange(it.spawn.probability);
        loadTargetResource(content, it);
      }

      //Items
      Resources.items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText("Content/Data/items.json"));
      foreach (var it in Resources.items)
      {
        GameMain.itemsProbability.AddRange(it.probability);
        it.texture = content.Load<Texture2D>("GFX/Item/" + it.type.ToString().ToLower());
      }

      //Crosshair
      Resources.reloadingCursor = content.Load<Texture2D>("GFX/Player/reloading");
      Resources.grabCursor = content.Load<Texture2D>("GFX/Player/grab");
      Resources.crosshair = content.Load<Texture2D>("GFX/Player/crosshair");
      Resources.defuseCursor = content.Load<Texture2D>("GFX/Player/defuse");

      //HUD
      Resources.hitmarker = content.Load<Texture2D>("GFX/GUI/hitmarker");
      Resources.hands = content.Load<Texture2D>("GFX/Player/hands");
      Resources.muzzleflash = content.Load<Texture2D>("GFX/Player/muzzleflash");
      Resources.bloodsplat = content.Load<Texture2D>("GFX/Player/bloodsplat");
      Resources.bullet = content.Load<Texture2D>("GFX/GUI/bullet");
      Resources.defuser = content.Load<Texture2D>("GFX/GUI/defuser");

      //Maps
      Resources.mapWoods = content.Load<Texture2D>("GFX/Maps/woods");

      //Sounds
      Resources.menuTheme = content.Load<Song>("Sound/GUI/menu_theme");
      Resources.menuClick = content.Load<SoundEffect>("Sound/GUI/click");
      Resources.fire = content.Load<SoundEffect>("Sound/Weapons/fire");
      Resources.burst = content.Load<SoundEffect>("Sound/Weapons/burst");
      Resources.reload = content.Load<SoundEffect>("Sound/Weapons/reload");

      Resources.cash = content.Load<SoundEffect>("Sound/Items/cash");
      Resources.fail = content.Load<SoundEffect>("Sound/Items/fail");
      Resources.nuke = content.Load<SoundEffect>("Sound/Items/nuke");
      Resources.rewind = content.Load<SoundEffect>("Sound/Items/rewind");
      Resources.contract = content.Load<SoundEffect>("Sound/Items/contract");
      Resources.armor = content.Load<SoundEffect>("Sound/Items/armor");
      Resources.medikit = content.Load<SoundEffect>("Sound/Items/medikit");

      Resources.bombtick = content.Load<SoundEffect>("Sound/Target/bombtick");
      Resources.explosion = content.Load<SoundEffect>("Sound/Target/explosion");
      Resources.defusal = content.Load<SoundEffect>("Sound/Target/defusal");
      Resources.hit = content.Load<SoundEffect>("Sound/Target/hit");

      Resources.unstoppable = content.Load<SoundEffect>("Sound/Combo/unstoppable");
      Resources.headhunter = content.Load<SoundEffect>("Sound/Combo/headhunter");
      Resources.denied = content.Load<SoundEffect>("Sound/Combo/denied");
      Resources.headshot = content.Load<SoundEffect>("Sound/Combo/headshot");

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
