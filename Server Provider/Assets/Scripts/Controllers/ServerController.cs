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
        foreach (Server server in GameController.Instance.PlantableServerList)
        {
            server.Plant += Server_Plant;
            server.Planted += Server_Planted;
        }
    }

    private void Server_Planted(Server server)
    {
      //  Debug.Log("ServerController::Server_Plant " + server.Name);
        foreach (ItemContainer itemContainer in GameController.Instance.ItemContainerToGO.Keys)
        {
            Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
            itemContainer.OnClick -= ItemContainer_OnClick;
        }
        selectedServer = null;
        GameController.Instance.plantingServer = null;
    }

    private void Server_Plant(Server server)
    {
        selectedServer = server;
        Debug.Log("ServerController::Server_Plant " + server.Name);
        foreach (ItemContainer itemContainer in GameController.Instance.ItemContainerToGO.Keys)
        {
            if (itemContainer.hasServer == false)
            {
                Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>("Images\\" + selectedServer.spriteName);
                image.color = new Color(0, 0, 0, 0.1f);
                // GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().SetNativeSize();

                itemContainer.OnClick += ItemContainer_OnClick;
            }
        }
        GameController.Instance.plantingServer = selectedServer;
    }

    private void ItemContainer_OnClick(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        Debug.Log("ServerController::ItemContainer_OnClick ::PLANTED" + pointerEventData.position);

        GameObject serverGO = Instantiate(serverPrefab);
        serverGO.transform.SetParent(GameController.Instance.ItemContainerToGO[container].transform,false);
        serverGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\" + selectedServer.spriteName);

        //serverGO.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        serverGO.GetComponentInChildren<TextMeshProUGUI>().text = selectedServer.Name;

        Debug.Log("Update added");
        selectedServer.UpdateEvent += GameController.Instance.ServerUpdate;

        GameController.Instance.plantedServersToGOs[selectedServer] = serverGO;

        container.hasServer = true;
        //after server planted selected server gets set to null so be carefull where you do this
        selectedServer.PlantedServer();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
