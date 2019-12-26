using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoosterFactory
{
    public static Booster getBooster(BoosterType boosterType)
    {

        switch (boosterType)
        {
            case BoosterType.Food:
                return new FoodBoost();

            case BoosterType.Petting:
                return new PettingBooster();

            case BoosterType.AnimalVaccine:
                return new AnimalVaccineBoost();
            case BoosterType.Boost1:
                break;
            default:
                break;
        }

        return null;
    }
}
