using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TargetGame.Utils;

namespace TargetGame
{
  public class SessionStats
  {
    public int score;
    public int bulletsFired;
    public int bulletsHit;
    public int headshotsOrCritical;
    public int headshotComboMax;
    public int contractsCompleted;
    public int bombsDefused;
    public int bonusCatched;

    public int legshots;

    public float getAccuracy()
    {
      if (bulletsFired == 0) return (0); //No bullets, 0%
      return (((float)bulletsHit / (float)bulletsFired) * 100.0f);
    }
    public float getHeadshotRatio()
    {
      if (bulletsHit == 0) return (0); //No hs, 0%
      return (((float)headshotsOrCritical / (float)bulletsHit) * 100.0f);
    }

    public virtual string AsString()
    {
      return ("Score: " + score + "\n"
            + "Bullets fired: " + bulletsFired + "\n"
            + "Bullets hit: " + bulletsHit + "\n"
            + "Accuracy: " + getAccuracy() + "%\n"
            + "Headshots/Critical: " + headshotsOrCritical + "\n"
            + "Headshots/Critical Ratio: " + getHeadshotRatio() + "%\n"
            + "Contracts completed: " + headshotComboMax + "\n"
            + "Bombs defused: " + bombsDefused + "\n"
            + "Bonus catched: " + bonusCatched + "\n"
            );
    }
  }
  public class PlayerStats : SessionStats
  {
    const string SavePath = "Content/User.json";

    public int highScore;
    public bool cheatsEnabled;

    public string saveLock;

    public static PlayerStats Load()
    {
      if (!File.Exists(SavePath)) return (new PlayerStats());
      PlayerStats statsLoaded = JsonConvert.DeserializeObject<PlayerStats>(File.ReadAllText(SavePath));
      if (statsLoaded.saveLock == Tools.Sha256(statsLoaded.AsString())) return (statsLoaded);
      else return (new PlayerStats()); //Stats corrupted
    }
    public void Save()
    {
      saveLock = Tools.Sha256(AsString());
      File.WriteAllText(SavePath, JsonConvert.SerializeObject(this));
    }

    public static PlayerStats operator +(PlayerStats gstats, SessionStats stats)
    {
      gstats.score += stats.score;
      if (stats.score > gstats.highScore) gstats.highScore = stats.score;
      gstats.bulletsFired += stats.bulletsFired;
      gstats.bulletsHit += stats.bulletsHit;
      gstats.headshotsOrCritical += stats.headshotsOrCritical;
      if (stats.headshotComboMax > gstats.headshotComboMax) gstats.headshotComboMax = stats.headshotComboMax;
      gstats.contractsCompleted += stats.contractsCompleted;
      gstats.bombsDefused += stats.bombsDefused;
      gstats.bonusCatched += stats.bonusCatched;

      gstats.legshots += stats.legshots;
      return (gstats);
    }

    public override string AsString()
    {
      return ("Total Score: " + score + "\n"
            + "High score: " + highScore + "\n"
            + "Bullets fired: " + bulletsFired + "\n"
            + "Bullets hit: " + bulletsHit + "\n"
            + "Accuracy: " + getAccuracy() + "%\n"
            + "Headshots/Critical: " + headshotsOrCritical + "\n"
            + "Highest Headshot combo: " + headshotComboMax + "\n"
            + "Contracts completed: " + headshotComboMax + "\n"
            + "Bombs defused: " + bombsDefused + "\n"
            + "Bonus catched: " + bonusCatched + "\n"
            + "Cheats enabled: " + cheatsEnabled + "\n"
            );
    }

  }
}
