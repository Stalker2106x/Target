using Microsoft.Xna.Framework;
using TargetGame.Settings;

namespace TargetGame.Utils
{
  class Tools
  {
    public static bool IsOnScreen(Point p)
    {
      return ((p.X > 0 && p.X < Options.Config.Width) && (p.Y > 0 && p.Y < Options.Config.Height));
    }
    public static bool IsOnScreen(Rectangle r)
    {
      return ((r.X + r.Width > 0 && r.X < Options.Config.Width) && (r.Y + r.Height > 0 && r.Y < Options.Config.Height));
    }
  }
}
