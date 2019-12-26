﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBoost : Booster
{
    public FoodBoost() : base(BoosterType.Food)
    {
        coolDown = 76;
        usingTime = 10;
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
