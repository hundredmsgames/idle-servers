using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettingBooster : Booster
{
    bool finished = true;
    int boostAmount = 30;
    public PettingBooster() : base(BoosterType.Petting)
    {
        coolDown = 60;
        usingTime = 10;
        Name = "Petting";
        Description = string.Format("Pets all the animals for {0} secs", usingTime);
        Level = 1;
    }

    public override void Use()
    {
        base.Use();
        //what does this thing when we use it
        //effects some models maybe?
        foreach (var item in GameController.Instance.planteditemsToGOs.Keys)
        {
            item.ApplyBoost(boostAmount);

        }
        finished = false;
    }
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (finished == true)
            return;
        if (CurrentUsingTime <= 0)
        {
            foreach (var item in GameController.Instance.planteditemsToGOs.Keys)
            {
                item.ApplyBoost(-boostAmount);

            }
            finished = true;
        }


    }

    public override void Upgrade()
    {
        base.Upgrade();
        //apply logic here 
    }
}
