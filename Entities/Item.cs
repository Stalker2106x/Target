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
    Health,
    FastReload,
    SpawnReducer,
    Nuke,
    Contract,
    Points
  }

  public class Item
  {
    private Random randomGenerator = new Random();
    private ItemType _type;
    private Vector2 _position;
    private Texture2D _gfx;
    private Rectangle _sprite;
    private bool _isActive;
    private bool _draw;

    public Item()
    {
      _isActive = true;
      _draw = false;
      _position = new Vector2((float)randomGenerator.Next(0, Options.Config.Width - 100), 0.0f);
      _sprite = new Rectangle((int) _position.X, 0, 100, 100);
      switch (randomGenerator.Next(1, 6))
      {
        case 1:
          _type = ItemType.Health;
          _gfx = Resources.itemHealth;
          break;
        case 2:
          _type = ItemType.FastReload;
          _gfx = Resources.itemFastReload;
          break;
        case 3:
          _type = ItemType.SpawnReducer;
          _gfx = Resources.itemSpawnReducer;
          break;
        case 4:
          _type = ItemType.Nuke;
          _gfx = Resources.itemNuke;
          break;
        case 5:
          _type = ItemType.Contract;
          _gfx = Resources.itemContract;
          break;
        case 6:
          _type = ItemType.Points;
          _gfx = Resources.itemPoints;
          break;
      }
    }

    public bool getActivity()
    {
      return _isActive;
    }

    public bool getDrawState()
    {
      return _draw;
    }

    public Texture2D getGFX()
    {
      return _gfx;
    }

    public void setActivity(bool boolen)
    {
      _isActive = boolen;
    }

    public HitType checkCollision()
    {
      if (!_sprite.Contains((int)GameMain.hud.crosshair.position.X, (int)GameMain.hud.crosshair.position.Y)) return (HitType.Miss); //Missed
      _draw = true;
      GameMain._player.setBulletsHit(1);
      switch (_type)
      {
        case ItemType.Health:
          GameMain._player.addHealth(50);
          break;
        case ItemType.FastReload:
          GameMain._player.getWeapon().addBullets(-GameMain._player.getWeapon().getMagazine());
          Resources.reload.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.getWeapon().addBullets(GameMain._player.getWeapon().getMaxMagazine());
          break;
        case ItemType.SpawnReducer:
          GameMain.addTimeout(1000);
          break;
        case ItemType.Nuke:
          GameMain._targets.Clear();
          break;
        case ItemType.Contract:
          GameMain._player.addContract();
          break;
        case ItemType.Points:
          Resources.cash.Play(Options.Config.SoundVolume, 0f, 0f);
          GameMain._player.addContract();
          break;
      }
      return (HitType.Catch);
    }

    public void checkOnScreen()
    {
      if ((double) _position.Y <= (double)Options.Config.Width)
        return;
      _isActive = false;
    }

    public void Update(GameTime gameTime)
    {
      _position.Y += 5f;
      checkOnScreen();
      _sprite.X = (int) _position.X;
      _sprite.Y = (int) _position.Y;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (_draw)
        return;
      spriteBatch.Draw(_gfx, _sprite, Color.White);
    }
  }
}
