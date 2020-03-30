using System;
using System.Collections.Generic;
using System.Text;
using TargetGame.Settings;

namespace TargetGame.Entities
{

  public class Contract
  {
    public class Reward
    {
      public enum Type
      {
        ExpandMagazine,
        ReduceReloadDuration
      }

      //Class
      public Type type;

      public Reward()
      {
        type = (Type)_randomGenerator.Next(0, Enum.GetValues(typeof(Type)).Length);
      }

      public string GetDescription()
      {
        switch (type)
        {
          case Type.ExpandMagazine:
            return ("Expand magazine");
          case Type.ReduceReloadDuration:
            return ("Reduce reload duration");
          default:
            break;
        }
        return ("None");
      }

      public void Claim()
      {
        switch(type)
        {
          case Type.ExpandMagazine:
            GameMain.player.getWeapon().addMagazineSize(1);
            break;
          case Type.ReduceReloadDuration:
            GameMain.player.getWeapon().addReloadDuration(100);
            break;
          default:
            break;
        }
      }
    }

    public enum Type
    {
      HeadshotCombo,
      CriticalCombo
    }

    //Class
    private static Random _randomGenerator = new Random();

    private Type type;
    private int amount;
    private int target;
    public Reward reward;
    public bool inactive;

    public Contract()
    {
      inactive = false;
      type = (Type)_randomGenerator.Next(0, Enum.GetValues(typeof(Type)).Length);
      amount = 0;
      target = 5;
      reward = new Reward();
      GameMain.hud.contractsPanel.AddIndicator(this);
    }

    public string GetMission()
    {
      string description = "Deal only ";
      switch (type)
      {
        case Type.HeadshotCombo:
          description += "headshot on soldiers";
          break;
        case Type.CriticalCombo:
          description += "critical on zeppelin";
          break;
        default:
          break;
      }
      return (description + " (" + amount + "/" + target + ")");
    }

    public void Update(HitType hit)
    {
      if (hit == HitType.Headshot)
      {
        if (type == Type.HeadshotCombo) amount++;
      }
      else if (hit == HitType.Critical)
      {
        if (type == Type.CriticalCombo) amount++;
      }
      else if (hit != HitType.Catch) Fail(); //If its not an objective or combo, fail contract
      if (amount >= target) Complete();
    }

    public void Complete()
    {
      inactive = true;
      reward.Claim();
      GameMain.hud.contractsPanel.RemoveIndicator(this);
      GameMain.hud.setAction("Contract complete");
      GameMain.player.addContractCompleted();
      Resources.cash.Play(Options.Config.SoundVolume, 0f, 0f);
    }

    public void Fail()
    {
      inactive = true;
      GameMain.hud.contractsPanel.RemoveIndicator(this);
      GameMain.hud.setAction("Contract failed");
      Resources.fail.Play(Options.Config.SoundVolume, 0f, 0f);
    }
  }
}
