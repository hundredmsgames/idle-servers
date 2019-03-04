using ControlToolkit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerController : MonoBehaviour
{
    public GameObject serverPrefab;
    // Start is called before the first frame update
     Server selectedServer;
    void Start()
    {
        foreach (Server server in GameContoller.Instance.PlantableServerList)
        {
            server.Plant += Server_Plant;
            server.Planted += Server_Planted;
        }
    }

    private void Server_Planted(Server server)
    {
      //  Debug.Log("ServerController::Server_Plant " + server.Name);
        foreach (ItemContainer itemContainer in GameContoller.Instance.ItemContainerToGO.Keys)
        {
            GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            itemContainer.OnClick -= ItemContainer_OnClick;
        }
        selectedServer = null;
    }

    private void Server_Plant(Server server)
    {
        selectedServer = server;
        Debug.Log("ServerController::Server_Plant " + server.Name);
        foreach (ItemContainer itemContainer in GameContoller.Instance.ItemContainerToGO.Keys)
        {
            GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().color = new Color(0, 0, 0, 0.3f);
            itemContainer.OnClick += ItemContainer_OnClick;
        }
    }

    private void ItemContainer_OnClick(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        Debug.Log("ServerController::ItemContainer_OnClick ::PLANTED" + pointerEventData.position);

        GameObject serverGO = Instantiate(serverPrefab);
        serverGO.transform.SetParent(GameContoller.Instance.ItemContainerToGO[container].transform,false);
        //serverGO.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        serverGO.GetComponentInChildren<TextMeshProUGUI>().text = selectedServer.Name;

        selectedServer.PlantedServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
