using System;
using System.Collections.Generic;
using System.Text;

namespace Target.Entities
{

  public class Contract
  {
    class Reward
    {
      public enum Type
      {
        Score
      }

      //Class

      public Type type;
      private int amount;

      public Reward()
      {
        type = Type.Score;
        amount = 500;
      }

      public void Claim()
      {
        switch(type)
        {
          case Type.Score:
            GameMain._player.addScore(amount);
            break;
          default:
            break;
        }
      }
    }

    public enum Type
    {
      Headshot
    }

    //Class

    private Type type;
    private int amount;
    private int target;
    private Reward reward;
    public bool inactive;

    public Contract()
    {
      inactive = false;
      type = Type.Headshot;
      amount = 0;
      target = 5;
      reward = new Reward();
      GameMain.hud.contractsPanel.AddIndicator(this);
    }

    public string GetStatus()
    {
      return (type.ToString() + " " + amount + "/" + target);
    }

    public void Update(HitType hit)
    {
      if (type == Type.Headshot && hit == HitType.Headshot) amount++;
      if (amount >= target) Complete();
    }

    public void Complete()
    {
      inactive = true;
      reward.Claim();
      GameMain.hud.contractsPanel.RemoveIndicator(this);
      GameMain.hud.setAction("Contract complete");
      GameMain._player.addContractCompleted();
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
