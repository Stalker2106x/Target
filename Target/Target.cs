// Decompiled with JetBrains decompiler
// Type: Target.Target
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Target
{
  internal class Target
  {
    private MouseState mouseState;
    private int m_hp;
    private Random m_randomX;
    private Random m_randomY;
    private Rectangle m_sprite;
    private float m_lifetime;
    private float m_hurtDelay;
    private bool m_isActive;

    public Target(int hp)
    {
      this.m_lifetime = 0.0f;
      this.m_hurtDelay = 4f;
      this.m_isActive = true;
      this.m_randomX = new Random();
      this.m_randomY = new Random();
      this.m_hp = hp;
      this.m_sprite = new Rectangle(this.m_randomX.Next(0, Game1.screenWidth - Resources.target.Width), this.m_randomX.Next(0, Game1.screenHeight - Resources.target.Height), Resources.target.Width, Resources.target.Height);
    }

    public bool getActivity()
    {
      return this.m_isActive;
    }

    public void checkCollision()
    {
      if (!this.m_sprite.Contains((int) HUD.m_target.X, (int) HUD.m_target.Y))
        return;
      GameMain.m_player.setScore(20);
      GameMain.m_player.setBulletsHit(1);
      this.m_isActive = false;
    }

    public void updateHurt(ref Player player, GameTime gameTime)
    {
      this.m_lifetime += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.m_lifetime < (double) this.m_hurtDelay)
        return;
      this.m_lifetime = 0.0f;
      Resources.fire.Play(0.5f, 0.0f, 0.0f);
      GameMain.m_hud.setBloodsplat();
      if (this.m_randomX.Next(1, 3) == 1)
        Resources.pain1.Play();
      else
        Resources.pain2.Play();
      player.setHealth(-20);
    }

    public void Update(ref Player player, GameTime gameTime, MouseState mouse)
    {
      this.mouseState = mouse;
      this.updateHurt(ref player, gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Resources.target, this.m_sprite, Color.White);
    }
  }
}
