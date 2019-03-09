using ControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    // Start is called before the first frame update
    //every row(shelf) has 4 tiles
    int serverCountInRow = 4;

    public List<Server> PlantableServerList { get; protected set; }

    public Dictionary<Server, GameObject> plantedServersToGOs;

    public Transform moneyTextContainer;

    public GameObject moneyTextPrefab;
    public GameObject placeholderPrefab;
    public GameObject itemcontainerPrefab;
    public Transform shelfGridTransform;

    List<Server[]> shelves;
    List<ItemPlaceholder[]> itemPlaceholders;
    public Dictionary<ItemContainer, GameObject> ItemContainerToGO;

    //Game Logic Variables
    int shelfCount = 1;
    public int shelfPrice = 1000;
    public int money = 9999;
    public float levelProgress = 0;
    public int level = 1;
    public Server plantingServer=null;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        shelves = new List<Server[]>();
        //instantiate plantable server list
        PlantableServerList = new List<Server>();
        //instantiate planted servers to game object dictionary
        //this will store all server models that planted and link them to the game objects in the game
        plantedServersToGOs = new Dictionary<Server, GameObject>();

        ItemContainerToGO = new Dictionary<ItemContainer, GameObject>();
        CreatePlantableServers();
    }

    private void CreatePlantableServers()
    {
        Server server;
        for (int i = 0; i < 8; i++)
        {
            server = new Server() { Name = "Server" + i, plantable = true, upgradeable = false, spriteName = "Computer" + i % 5, mps = 100 * (i + 1) };
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
            ItemContainerToGO.Add(itemContainer, itemcontainerGO);
        }
    }
    public void UnlockShelf()
    {
        if (money - shelfPrice < 0)
            return;
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
            if(plantingServer != null)
            {
                plantingServer.PlantServer();
            }

        }
        //
        money = money - shelfPrice;
        //update shelfprice
        shelfPrice += 1000;
        shelfCount++;

        // https://answers.unity.com/questions/1276433/get-layoutgroup-and-contentsizefitter-to-update-th.html
        // Content size fitter doesn't update content of grid layout. This is a workaround for it.
        LayoutRebuilder.ForceRebuildLayoutImmediate(shelfGridTransform as RectTransform);
    }

    float time = 0f;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 1f)
        {
            time = 0f;
            foreach (Server server in plantedServersToGOs.Keys)
            {
                server.Update();
            }
        }
    }

    public void ServerUpdate(Server server)
    {
        //earn money
        money += server.ProduceMoney();

        // create a money text
        GameObject moneyText = Instantiate(moneyTextPrefab);
        moneyText.transform.SetParent(moneyTextContainer, false);

        moneyText.transform.position = plantedServersToGOs[server].transform.position + Vector3.up * 20f;

        //level progress
        //FIXME : 0.01 is hard coded turn it to a variable and change the value with power ups
        levelProgress += 0.01f;
        if (levelProgress >= 1)
        {
            level++;
            levelProgress = 0;
        }
    }


}
