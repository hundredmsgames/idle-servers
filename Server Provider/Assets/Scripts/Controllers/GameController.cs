﻿using ControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameController : MonoBehaviour
{
    public static GameController Instance;

    // We can use one dictionary here.
    public Dictionary<ItemContainer, GameObject> ItemContainerToGO;
    public Dictionary<ItemContainer, Item> ItemContainerToitem;
    public Dictionary<Item, GameObject> planteditemsToGOs;


    public GameObject moneyTextPrefab;
    public GameObject placeholderPrefab;
    public GameObject itemcontainerPrefab;
    public Transform shelfGridTransform;
    public Transform moneyTextContainer;

    public List<ItemController> ItemControllers;

    List<Item[]> shelves;
    List<ItemPlaceholder[]> itemPlaceholders;


    public event Action<int> LeveledUp;


    // Start is called before the first frame update
    // every row(shelf) has 4 tiles
    int ItemCountInRow = 4;

    //Game Logic Variables
    int shelfCount = 1;
    public int shelfPrice = 1000;
    public int shelfRequiredLevel = 20;
    public int money = 0;
    public float levelProgress = 0;
    public int level = 1;

    //whenever we use a power up we can set this multiplier to any power of 10
    float levelProgressMultiplier = 2;
    float levelProgressPerSecond = 0.003f;


    private void OnEnable()
    {
        if (Instance != null)
            return;

        Instance = this;
        shelves = new List<Item[]>();

        // instantiate planted Items to game object dictionary
        // this will store all Item models that planted and link them to the game objects in the game
        planteditemsToGOs = new Dictionary<Item, GameObject>();
        ItemContainerToGO = new Dictionary<ItemContainer, GameObject>();
        ItemContainerToitem = new Dictionary<ItemContainer, Item>();

        ItemControllers = new List<ItemController>();
        ItemPrototype.InitializeObjects();
        CleanShelf();
        CreateShelf();
        CreateShelf();
    }


    // Maybe this method deserves a better name idk.
    private void FillItemInfo()
    {
        // Need to read canSetup and numberOfItem info of every Item.
    }

    // Clean shelves before executing.
    void CleanShelf()
    {
        if (DebugConfigs.RESET_UI)
        {
            foreach (Transform placeholder in shelfGridTransform)
            {
                ItemContainer container = placeholder.GetComponentInChildren<ItemContainer>();
                if (container != null)
                {
                    Destroy(container.gameObject);
                }
            }
        }
    }

    void CreateShelf()
    {
        for (int i = 0; i < ItemCountInRow; i++)
        {
            GameObject placeHolderGO = (GameObject)Instantiate(placeholderPrefab);
            placeHolderGO.transform.SetParent(shelfGridTransform, false);
            ItemPlaceholder placeholder = placeHolderGO.GetComponent<ItemPlaceholder>();
            GameObject itemcontainerGO = (GameObject)Instantiate(itemcontainerPrefab);
            itemcontainerGO.transform.SetParent(placeHolderGO.transform, false);
            ItemContainer itemContainer = itemcontainerGO.GetComponent<ItemContainer>();
            itemContainer.CanDrag = false;
            ItemContainerToGO.Add(itemContainer, itemcontainerGO);
            ItemContainerToitem.Add(itemContainer, null);
        }
    }

    public void UnlockShelf()
    {
        // If we do not have enough money just quit this method.
        // Note: It is a good place to show tips like "You don't have enough money".
        if (money <= shelfPrice)
            return;

        // Create the new shelf, placeholders, item containers etc.
        CreateShelf();

        // If we unlock the shelf while we are planting a Item,
        // we should call PlantItem again thus new shelf can be updated.  
        if (selecteditem != null)
        {

            //HACK:do we need to handle this situtation like this?
            //do not permit to unluck shelfs while planing Items maybe???
            ShowHidePlantablePositions(true);
        }

        // We have enough money so just reduce the price from our money.
        money = money - shelfPrice;

        // Update shelf price
        //TODO : this need to be a variable
        shelfPrice += 1000;
        shelfRequiredLevel += 20;
        shelfCount++;

        // https://answers.unity.com/questions/1276433/get-layoutgroup-and-contentsizefitter-to-update-th.html
        // Content size fitter doesn't update content of grid layout. This is a workaround for it.
        LayoutRebuilder.ForceRebuildLayoutImmediate(shelfGridTransform as RectTransform);
    }

    float time = 0f;
    public void UpdateLevel()
    {
        levelProgress += levelProgressPerSecond * levelProgressMultiplier;

        if (levelProgress >= 1)
        {
            level++;
            LeveledUp?.Invoke(level);
            // levelProgressMultiplier = 50 / level;
            levelProgress = 0;
        }
    }
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 1f)
        {
            time = 0f;

            if (planteditemsToGOs.Count == 0)
                return;

            foreach (Item Item in planteditemsToGOs.Keys)
            {
                Item.Update();

            }

            UpdateLevel();
        }


    }

    public void ItemUpdate(Item item)
    {
        int amount = item.Produce();
        //earn money
        money += amount;

        // create a money text game object
        GameObject moneyTextGO = Instantiate(moneyTextPrefab);
        moneyTextGO.transform.SetParent(moneyTextContainer, false);

        moneyTextGO.transform.position = planteditemsToGOs[item as Item].transform.position + Vector3.up * 40f;

        moneyTextGO.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();

        //level progress
        //FIXME : 0.01 is hard coded turn it to a variable and change the value with power ups
        UpdateLevel();

    }

    void OnApplicationPause()
    {
        Debug.Log("OnApplicationPause()");
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit()");
    }

}
