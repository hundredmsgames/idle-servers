using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalVaccineBoost : Booster
{
    public AnimalVaccineBoost() : base(BoosterType.AnimalVaccine)
    {
        coolDown = 60;
        usingTime = 0;
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
        GameController.Instance.money += 1000;
        //does somethings when we use the booster with UI or game logic
    }
}
