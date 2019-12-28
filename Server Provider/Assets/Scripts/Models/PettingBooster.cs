using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettingBooster : Booster
{
    public PettingBooster() : base(BoosterType.Petting)
    {
        coolDown = 53;
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
    }
    public override void Upgrade()
    {
        base.Upgrade();
        //apply logic here 
    }
}
