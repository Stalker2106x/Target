// Decompiled with JetBrains decompiler
// Type: Target.Resources
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Target
{
  class Resources
  {
    public static SpriteFont titleFont;
    public static SpriteFont regularFont;
    public static Texture2D menuBackground;
    public static Texture2D soldier_idle;
    public static Texture2D soldier_firing;
    public static Texture2D hitmarker;
    public static Texture2D bloodsplat;
    public static Texture2D r700;
    public static Texture2D crosshair;
    public static Texture2D bullet;
    public static Texture2D itemHealth;
    public static Texture2D itemFastReload;
    public static Texture2D itemDeath;
    public static Texture2D itemSpawnReducer;
    public static Texture2D mapWoods;
    public static Texture2D gamepadKeys;
    public static Texture2D keyboardKeys;
    public static SoundEffect menuClick;
    public static SoundEffect fire;
    public static SoundEffect burst;
    public static SoundEffect reload;
    public static SoundEffect breath;
    public static SoundEffect heartbeat;
    public static SoundEffect pain1;
    public static SoundEffect pain2;

    public static void LoadContent(ContentManager content)
    {
      Resources.titleFont = content.Load<SpriteFont>("Font/title");
      Resources.regularFont = content.Load<SpriteFont>("Font/regular");
      //Resources.menuBackground = content.Load<Texture2D>("GFX/GUI/menuBackground");
      Resources.menuBackground = content.Load<Texture2D>("GFX/GUI/main");
      //Resources.target = content.Load<Texture2D>("GFX/Enemy/target");
      Resources.soldier_idle = content.Load<Texture2D>("GFX/Enemy/soldier_idle");
      Resources.soldier_firing = content.Load<Texture2D>("GFX/Enemy/soldier_firing");
      Resources.hitmarker = content.Load<Texture2D>("GFX/GUI/hitmarker");
      Resources.bloodsplat = content.Load<Texture2D>("GFX/Player/bloodsplat");
      Resources.crosshair = content.Load<Texture2D>("GFX/Player/crosshair");
      Resources.bullet = content.Load<Texture2D>("GFX/Weapons/bullet");
      Resources.r700 = content.Load<Texture2D>("GFX/Weapons/r700");
      Resources.itemHealth = content.Load<Texture2D>("GFX/Entity/health");
      Resources.itemFastReload = content.Load<Texture2D>("GFX/Entity/fastReload");
      Resources.itemDeath = content.Load<Texture2D>("GFX/Entity/death");
      Resources.itemSpawnReducer = content.Load<Texture2D>("GFX/Entity/time");
      Resources.mapWoods = content.Load<Texture2D>("GFX/Maps/woods");
      Resources.gamepadKeys = content.Load<Texture2D>("GFX/GUI/gamepadKeys");
      Resources.keyboardKeys = content.Load<Texture2D>("GFX/GUI/keyboardKeys");
      Resources.menuClick = content.Load<SoundEffect>("Sound/GUI/click");
      Resources.fire = content.Load<SoundEffect>("Sound/Weapons/fire");
      Resources.burst = content.Load<SoundEffect>("Sound/Weapons/burst");
      Resources.reload = content.Load<SoundEffect>("Sound/Weapons/reload");
      Resources.breath = content.Load<SoundEffect>("Sound/Player/breath");
      Resources.heartbeat = content.Load<SoundEffect>("Sound/Player/heartbeat");
      Resources.pain1 = content.Load<SoundEffect>("Sound/Player/pain1");
      Resources.pain2 = content.Load<SoundEffect>("Sound/Player/pain2");
    }
  }
}
