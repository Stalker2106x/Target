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
  internal class Item
  {
    private Random randomX = new Random();
    private Random randomType = new Random();
    private ItemType m_type;
    private Vector2 m_position;
    private Texture2D m_gfx;
    private Rectangle m_sprite;
    private bool m_isActive;
    private bool m_draw;

    public Item()
    {
      this.randomX = new Random();
      this.m_isActive = true;
      this.m_draw = false;
      this.m_position = new Vector2((float) this.randomX.Next(0, Game1.screenWidth - 100), 0.0f);
      this.m_sprite = new Rectangle((int) this.m_position.X, 0, 100, 100);
      switch (this.randomType.Next(1, 4))
      {
        case 1:
          this.m_type = ItemType.Health;
          this.m_gfx = Resources.itemHealth;
          break;
        case 2:
          this.m_type = ItemType.FastReload;
          this.m_gfx = Resources.itemFastReload;
          break;
        case 3:
          this.m_type = ItemType.Death;
          this.m_gfx = Resources.itemDeath;
          break;
      }
    }

    public bool getActivity()
    {
      return this.m_isActive;
    }

    public bool getDrawState()
    {
      return this.m_draw;
    }

    public Texture2D getGFX()
    {
      return this.m_gfx;
    }

    public void setActivity(bool boolen)
    {
      this.m_isActive = boolen;
    }

    public void checkCollision()
    {
      if (!this.m_sprite.Contains((int) HUD.m_target.X, (int) HUD.m_target.Y))
        return;
      this.m_draw = true;
      GameMain.m_player.setBulletsHit(1);
      switch (this.m_type)
      {
        case ItemType.Health:
          GameMain.m_player.setHealth(40);
          break;
        case ItemType.FastReload:
          GameMain.m_player.getWeapon().setMagazine(-GameMain.m_player.getWeapon().getMagazine());
          Resources.reload.Play();
          GameMain.m_player.getWeapon().setMagazine(GameMain.m_player.getWeapon().getMaxMagazine());
          break;
        case ItemType.Death:
          GameMain.gameOver = true;
          break;
      }
    }

    public void checkOnScreen()
    {
      if ((double) this.m_position.Y <= (double) Game1.screenWidth)
        return;
      this.m_isActive = false;
    }

    public void Update(GameTime gameTime)
    {
      this.m_position.Y += 5f;
      this.checkOnScreen();
      this.m_sprite.X = (int) this.m_position.X;
      this.m_sprite.Y = (int) this.m_position.Y;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.m_draw)
        return;
      spriteBatch.Draw(this.m_gfx, this.m_sprite, Color.White);
    }
  }
}
