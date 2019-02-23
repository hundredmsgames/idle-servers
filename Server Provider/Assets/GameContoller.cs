using ControlToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContoller : MonoBehaviour
{
    public static GameContoller Instance;
    // Start is called before the first frame update
   public List<ItemContainer> ItemContainers;
    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        ItemContainers = new List<ItemContainer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
