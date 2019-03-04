using ControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameContoller : MonoBehaviour
{
    public static GameContoller Instance;
    // Start is called before the first frame update
    //every row(shelf) has 4 tiles
    int serverCountInRow = 4;

    public List<Server> PlantableServerList { get; protected set; }

    public GameObject placeholderPrefab;
    public GameObject itemcontainerPrefab;
    public Transform shelfGridTransform;

    List<Server[]> shelves;
    List<ItemPlaceholder[]> itemPlaceholders;
    public Dictionary<ItemContainer,GameObject> ItemContainerToGO;

    //Game Logic Variables
    int shelfCount = 1;
    public int shelfPrice = 1000;
    public int money = 9999;


    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        shelves = new List<Server[]>();
        PlantableServerList = new List<Server>();
        ItemContainerToGO = new Dictionary<ItemContainer, GameObject>();
        CreatePlantableServers();
    }

    private void CreatePlantableServers()
    {
        Server server;
        for (int i = 0; i < 8; i++)
        {
             server = new Server() { Name = "Server"+i, plantable = true, upgradeable = false };
            PlantableServerList.Add(server);
        }

    }

    void Start()
    {
        ItemPlaceholder[] placeholders = new ItemPlaceholder[serverCountInRow];
        for (int i = 0; i < serverCountInRow; i++)
        {

            GameObject placeHolderGO = (GameObject)Instantiate(placeholderPrefab);
            placeHolderGO.transform.SetParent(shelfGridTransform, false);
            ItemPlaceholder placeholder = placeHolderGO.GetComponent<ItemPlaceholder>();
            placeholders[i] = placeholder;
            GameObject itemcontainerGO = (GameObject)Instantiate(itemcontainerPrefab);
            itemcontainerGO.transform.SetParent(placeHolderGO.transform, false);
            ItemContainer itemContainer = itemcontainerGO.GetComponent<ItemContainer>();
            itemContainer.CanDrag = false;
            ItemContainerToGO.Add(itemContainer,itemcontainerGO);
        }
    }
    public void UnlockShelf()
    {
        ItemPlaceholder[] placeholders = new ItemPlaceholder[serverCountInRow];
        for (int i = 0; i < serverCountInRow; i++)
        {

            GameObject placeHolderGO = (GameObject)Instantiate(placeholderPrefab);
            placeHolderGO.transform.SetParent(shelfGridTransform, false);
            ItemPlaceholder placeholder = placeHolderGO.GetComponent<ItemPlaceholder>();
            placeholders[i] = placeholder;
            GameObject itemcontainerGO = (GameObject)Instantiate(itemcontainerPrefab);
            itemcontainerGO.transform.SetParent(placeHolderGO.transform, false);
            ItemContainer itemContainer = itemcontainerGO.GetComponent<ItemContainer>();
            itemContainer.CanDrag = false;
            ItemContainerToGO.Add(itemContainer, itemcontainerGO);
            
        }
        //
        money = money - shelfPrice;
        //update shelfprice
        shelfCount++;

        // https://answers.unity.com/questions/1276433/get-layoutgroup-and-contentsizefitter-to-update-th.html
        // Content size fitter doesn't update content of grid layout. This is a workaround for it.
        LayoutRebuilder.ForceRebuildLayoutImmediate(shelfGridTransform as RectTransform);
    }


    // Update is called once per frame
    void Update()
    {

    }


}
