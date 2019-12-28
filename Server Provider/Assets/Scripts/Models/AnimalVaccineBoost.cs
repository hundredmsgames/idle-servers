using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalVaccineBoost : Booster
{
    public AnimalVaccineBoost() : base(BoosterType.AnimalVaccine)
    {
        coolDown = 20;
        usingTime = 5;
        Name = "Animal Vaccine";
        Description = "Vaccine the animals for instant 500 hearts.";
        Level = 1;
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
