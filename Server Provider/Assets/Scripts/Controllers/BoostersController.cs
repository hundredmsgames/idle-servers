using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersController : MonoBehaviour
{
    public static BoostersController Instance;
    //create boosters with the info we saved
    //and start the logic
    // Start is called before the first frame update
    Dictionary<BoosterType, Booster> boosterType2Booster;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        Booster booster;

        boosterType2Booster = new Dictionary<BoosterType, Booster>();
        booster = BoosterFactory.getBooster(BoosterType.Food);
        boosterType2Booster.Add(booster.BoosterType, booster);
        booster = BoosterFactory.getBooster(BoosterType.Petting);
        boosterType2Booster.Add(booster.BoosterType, booster);
        booster = BoosterFactory.getBooster(BoosterType.AnimalVaccine);
        boosterType2Booster.Add(booster.BoosterType, booster);

    }

    // Update is called once per frame
    void Update()
    {
        //we can calculate cooldowns here or any other logic
        foreach (var booster in boosterType2Booster.Values)
        {
            booster.Update(Time.deltaTime);
        }

    }

    public Booster GetBoosterOfType(BoosterType boosterType)
    {
        if (boosterType2Booster.ContainsKey(boosterType))
            return boosterType2Booster[boosterType];
        else
            return null;
    }


}
