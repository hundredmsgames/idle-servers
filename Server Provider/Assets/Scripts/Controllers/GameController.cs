using ControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

	// We can use one dictionary here.
    public Dictionary<ItemContainer, GameObject> ItemContainerToGO;
    public Dictionary<ItemContainer, Server> ItemContainerToServer;

    public delegate void LevelHandler(int level);
    public event LevelHandler LeveledUp;

    //Game Logic Variables
    int shelfCount = 1;
    public int shelfPrice = 1000;
    public int money = 0;
    public float levelProgress = 0;
    public int level = 1;
    public Server plantingServer = null;
    //whenever we use a power up we can set this multiplier to any power of 10
    float levelProgressMultiplier=3;
    float levelProgressPerSecond = 0.01f;


    private void OnEnable()
    {
        if (Instance != null)
			return;
        
		Instance = this;

        shelves = new List<Server[]>();

        //instantiate plantable server list
        PlantableServerList = new List<Server>();
        ItemContainerToServer = new Dictionary<ItemContainer, Server>();
        
		//instantiate planted servers to game object dictionary
        //this will store all server models that planted and link them to the game objects in the game
        plantedServersToGOs = new Dictionary<Server, GameObject>();

        ItemContainerToGO = new Dictionary<ItemContainer, GameObject>();
        CreatePlantableServers();

    }

	void Start()
	{
		// We call it here beacuse DataBind method in ArrangeItemsControl,
		// should be called before this method. DataBind destroys all gameobjects in ItemContainer.
		CreateShelf();
	}

    private void CreatePlantableServers()
    {
        Server server;
        for (int i = 0; i < 8; i++)
        {
            server = new Server() {
                Name = "Server" + i,
                plantable = true,
                upgradeable = false,
                spriteName = "Computer" + i % 5,
                mps = i + 1,
                requiredLevel = (2 * i + 1),
                requiredMoneyForUpgrade = 30
            };

            PlantableServerList.Add(server);
        }
    }

    void CreateShelf()
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
            ItemContainerToServer.Add(itemContainer, null);
        }
    }

    public void UnlockShelf()
    {
        if (money - shelfPrice < 0)
            return;


        CreateShelf();
        if (plantingServer != null)
        {
            plantingServer.PlantServer();
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

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 1f)
        {
            time = 0f;

            if (plantedServersToGOs.Count == 0)
                return;

            foreach (Server server in plantedServersToGOs.Keys)
            {
                server.Update();
            }            

            levelProgress += levelProgressPerSecond * levelProgressMultiplier;

            if (levelProgress >= 1)
            {
                level++;
                LeveledUp?.Invoke(level);
                levelProgress = 0;
            }
        }
    }

    public void ServerUpdate(Server server)
    {
        //earn money
        money += server.ProduceMoney();

        // create a money text game object
        GameObject moneyTextGO = Instantiate(moneyTextPrefab);
        moneyTextGO.transform.SetParent(moneyTextContainer, false);

        moneyTextGO.transform.position = plantedServersToGOs[server].transform.position + Vector3.up * 40f;

        moneyTextGO.GetComponentInChildren<TextMeshProUGUI>().text = server.mps.ToString();

        //level progress
        //FIXME : 0.01 is hard coded turn it to a variable and change the value with power ups
       
       
    }


}
