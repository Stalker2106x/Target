// Decompiled with JetBrains decompiler
// Type: Target.Item
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TargetGame.Settings;

namespace TargetGame.Entities
{
  /// <summary>
  /// Item indentification type for bonus
  /// </summary>
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

  /// <summary>
  /// Spawnable Bonus item
  /// </summary>
  public class Item : IEntity
  {
    private static Random randomGenerator = new Random();

    private ItemType _type;
    public ItemType type { get { return (_type); } set { _type = value; } }

    private int _probability;
    public int probability { get { return (_probability); } set { _probability = value; } }

    private Point _move;
    public Point move { get { return (_move); } set { _move = value; } }

    private Point _position;
    private Texture2D _texture;
    public Texture2D texture { set { _texture = value; } }
    private bool _isActive;

    public Item()
    {
      _isActive = true;
      _move = new Point(0, 5);
    }

    public Item Copy()
    {
      return (Item)this.MemberwiseClone();
    }

    /**************************
     * IEntity implementation
     **************************/

    public void setPosition(Point pos)
    {
      _position = pos;
    }

    public void randomizePosition()
    {
      _position = new Point(randomGenerator.Next(0, Options.Config.Width - getRectangle().Width), 0);
    }

    public void activate()
    {
      //Nothing to activate
    }

    public Rectangle getRectangle()
    {
      return (new Rectangle(_position.X, _position.Y, _texture.Width, _texture.Height));
    }

    public bool getActivity()
    {
      return _isActive;
    }

    /**************************
     * Specific Methods
     **************************/

    public HitType checkCollision()
    {
      if (!getRectangle().Contains((int)GameMain.hud.crosshair.position.X, (int)GameMain.hud.crosshair.position.Y)) return (HitType.Miss); //Missed
      GameMain.player.setBulletsHit(1);
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
          GameMain.player.addHealth(50);
          break;
        case ItemType.InstantReload:
          GameMain.player.getWeapon().addBullets(-GameMain.player.getWeapon().getMagazine());
          Resources.reload.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.player.getWeapon().addBullets(GameMain.player.getWeapon().getMaxMagazine());
          break;
        case ItemType.SpawnReducer:
          Resources.rewind.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.addTimeout(500);
          break;
        case ItemType.Nuke:
          Resources.nuke.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.targets.Clear();
          break;
        case ItemType.Contract:
          Resources.contract.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.player.addContract();
          break;
        case ItemType.Points:
          Resources.cash.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.player.addContract();
          break;
        case ItemType.KevlarVest:
          Resources.armor.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain.player.addKevlar();
          break;
        case ItemType.Defuser:
          GameMain.player.setDefuser(true);
          break;
      }
    }

    /**************************
     * Update/Draw Methods
     **************************/

    public void Update(GameTime gameTime)
    {
      _position += _move;
      if (_position.Y > Options.Config.Height) _isActive = false; //out of screen
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, getRectangle(), Color.White);
    }
  }
}
