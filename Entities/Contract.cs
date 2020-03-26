using System;
using System.Collections.Generic;
using System.Text;

namespace Target.Entities
{

  class Contract
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
            GameMain._player.setScore(amount);
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
    }

    public string GetStatus()
    {
      return ("Active contract: " + type.ToString() + " " + amount + "/" + target);
    }

    public void Update(HitType hit)
    {
      if (type == Type.Headshot && hit == HitType.Headshot) amount++;
      GameMain.hud.updateContract(GetStatus());
      if (amount >= target) Complete();
    }

    public void Complete()
    {
      inactive = true;
      reward.Claim();
      GameMain.hud.updateContract("");
      GameMain.hud.setAction("Contract complete");
    }

    public void Fail()
    {
      inactive = true;
      GameMain.hud.updateContract("");
      GameMain.hud.setAction("Contract failed");
    }
  }
}
