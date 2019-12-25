using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBoost : Booster
{
    public override void Upgrade()
    {
        base.Upgrade();
        //if we want any logic for spesific booster, we can do it here
    }
    public override void Use()
    {
        //does somethings when we use the booster with UI or game logic
    }
}
