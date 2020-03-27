// Decompiled with JetBrains decompiler
// Type: Target.Item
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Target
{
  public enum ItemType
  {
    Medikit,
    InstantReload,
    SpawnReducer,
    Nuke,
    Contract,
    Points,
    KevlarVest,
    Defuser
  }

  public class Item
  {
    private static Random randomGenerator = new Random();

    private ItemType _type;
    public ItemType type { get { return (_type); } set { _type = value; } }

    private int _probability;
    public int probability { get { return (_probability); } set { _probability = value; } }

    private Point _position;
    private Texture2D _texture;
    public Texture2D texture {
      set
      {
        _texture = value;
      }
    }
    private bool _isActive;

    public Item()
    {
      _isActive = true;
    }

    public Item Copy()
    {
      return (Item)this.MemberwiseClone();
    }

    public void randomizeSpawn()
    {
      _position = new Point(randomGenerator.Next(0, Options.Config.Width - getRectangle().Width), 0);
    }

    public Rectangle getRectangle()
    {
      return (new Rectangle(_position.X, _position.Y, _texture.Width, _texture.Height));
    }

    public bool getActivity()
    {
      return _isActive;
    }

    public HitType checkCollision()
    {
      if (!getRectangle().Contains((int)GameMain.hud.crosshair.position.X, (int)GameMain.hud.crosshair.position.Y)) return (HitType.Miss); //Missed
      GameMain._player.setBulletsHit(1);
      Catch();
      _isActive = false;
      return (HitType.Catch);
    }

    public void Catch()
    {
      GameMain.hud.setAction(type.ToString());
      switch (_type)
      {
        case ItemType.Medikit:
          Resources.medikit.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.addHealth(50);
          break;
        case ItemType.InstantReload:
          GameMain._player.getWeapon().addBullets(-GameMain._player.getWeapon().getMagazine());
          Resources.reload.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.getWeapon().addBullets(GameMain._player.getWeapon().getMaxMagazine());
          break;
        case ItemType.SpawnReducer:
          Resources.rewind.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.addTimeout(1000);
          break;
        case ItemType.Nuke:
          Resources.nuke.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._targets.Clear();
          break;
        case ItemType.Contract:
          Resources.contract.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.addContract();
          break;
        case ItemType.Points:
          Resources.cash.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.addContract();
          break;
        case ItemType.KevlarVest:
          Resources.armor.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.addKevlar();
          break;
        case ItemType.Defuser:
          GameMain._player.setDefuser(true);
          break;
      }
    }

    public void Update(GameTime gameTime)
    {
      _position.Y += 5;
      if (_position.Y > Options.Config.Height) _isActive = false; //out of screen
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, getRectangle(), Color.White);
    }
  }
}
