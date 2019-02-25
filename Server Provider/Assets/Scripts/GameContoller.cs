using ControlToolkit;
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

    public GameObject placeholderPrefab;
    public Transform shelfGridTransform;

    List<Server[]> shelves;
    List<ItemPlaceholder[]> itemPlaceholders;
    public List<ItemContainer> ItemContainers;

    //Game Logic Variables
    int shelfCount=1;
    int shelfPrice = 1000;
    int money = 1000;


    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        shelves = new List<Server[]>();
    }
    void Start()
    {
        
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
