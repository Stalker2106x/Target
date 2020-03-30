using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TargetGame.Entities
{
  /// <summary>
  /// Interface for spawnable entities
  /// </summary>
  interface IEntity
  {
    void randomizePosition();
    void activate();
    Rectangle getRectangle();
    bool getActivity();

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
  }
}
