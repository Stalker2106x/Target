// Decompiled with JetBrains decompiler
// Type: Target.Program
// Assembly: Target, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 210E06DD-6036-47D0-ADB5-DBEC4EDD925B
// Assembly location: D:\Projets\Target\Target.exe

using System;

namespace Target
{
  public static class Program
  {
    [STAThread]
    private static void Main()
    {
      using (Game1 game1 = new Game1())
        game1.Run();
    }
  }
}
