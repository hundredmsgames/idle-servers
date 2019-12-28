using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBoost : Booster
{
    float time;
    public FoodBoost() : base(BoosterType.Food)
    {
        coolDown = 76;
        usingTime = 10;
        Name = "Food and Water";
        Description = "Gives food and water.";
        Level = 1;
    }
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (CurrentUsingTime > 0)
        {
            time += deltaTime;
            if (time >= 0.1)
            {
                foreach (var item in GameController.Instance.planteditemsToGOs.Keys)
                {
                    GameController.Instance.ItemUpdate(item);

                }
                time = 0;
            }
        }


    }

    public override void Upgrade()
    {
        base.Upgrade();
        //if we want any logic for spesific booster, we can do it here
    }
    public override void Use()
    {
        base.Use();

        //does somethings when we use the booster with UI or game logic
    }
}
