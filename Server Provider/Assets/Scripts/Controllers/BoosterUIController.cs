using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUIController : MonoBehaviour
{
    [SerializeField]
    GameObject[] boosterUIObjects;
    [SerializeField]
    GameObject[] boosterUpgadeableUIObjects;

    Dictionary<BoosterType, GameObject> boosterType2UI;


    // Start is called before the first frame update
    void Start()
    {
        boosterType2UI = new Dictionary<BoosterType, GameObject>();
        boosterType2UI.Add(BoosterType.Food, boosterUIObjects[0]);
        boosterType2UI.Add(BoosterType.Petting, boosterUIObjects[1]);
        boosterType2UI.Add(BoosterType.AnimalVaccine, boosterUIObjects[2]);
        Booster.OnStateChanged += Booster_OnStateChanged;
    }
    public void SetConstantInfos()
    {

    }
    private void Booster_OnStateChanged(BoosterType boosterType, BoosterState boosterState)
    {
        GameObject boosterObject = boosterType2UI[boosterType];
        GameObject readyUI = boosterObject.transform.GetChild(0).gameObject;
        GameObject CoolDownUI = boosterObject.transform.GetChild(1).gameObject;

        //if booster state is ready then update the UI
        if (boosterState == BoosterState.Ready)
        {
            readyUI.SetActive(true);
            CoolDownUI.SetActive(false);

        }
        //if booster state is not ready then we start the cooldown and set the UI based on that 
        if (boosterState == BoosterState.CoolDown || boosterState == BoosterState.Using)
        {
            readyUI.SetActive(false);
            CoolDownUI.SetActive(true);

        }
        //      Debug.Log("statechanged" + boosterType + ": " + boosterState);
        //if boosterstate is using state then we are going to do some stuff on the UI
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: We dont have to check every single update that we are ready or not and set the UI 
        //maybe we can add an event on the booster like on state changed and upadte the with that
        foreach (var boosterType in boosterType2UI.Keys)
        {
            Booster booster = BoostersController.Instance.GetBoosterOfType(boosterType);

            if (booster.State != BoosterState.Ready)
            {
                GameObject cooldownObject = boosterType2UI[boosterType].transform.GetChild(1).gameObject;
                cooldownObject.GetComponentInChildren<TextMeshProUGUI>().text = ((int)booster.CurrentCoolDown).TimerFormat();
                cooldownObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = booster.GetCoolDownAsPercent();
            }
        }

    }
    public void UseBooster(int boosterType)
    {
        Booster booster = BoostersController.Instance.GetBoosterOfType((BoosterType)Enum.Parse(typeof(BoosterType), Enum.GetNames(typeof(BoosterType))[boosterType]));
        if (booster.State == BoosterState.Ready)
            booster.Use();
    }
}
