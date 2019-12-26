using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettingBooster : Booster
{
    public PettingBooster() : base(BoosterType.Petting)
    {
        coolDown = 3;
        usingTime = 0;
    }

    public override void Use()
    {
        base.Use();
        //what does this thing when we use it
        //effects some models maybe?
    }
    public override void Upgrade()
    {
        base.Upgrade();
        //apply logic here 
    }
}
