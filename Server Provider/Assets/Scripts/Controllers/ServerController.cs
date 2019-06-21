using ControlToolkit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerController : MonoBehaviour
{
    public static ServerController Instance;
    public GameObject serverPrefab;
    Server selectedServer;
    Dictionary<ItemContainer, GameObject> itemcontainerToGO;
    
	private void OnEnable()
    {
        if (Instance != null)
			return;

		Instance = this;
	}

    void Start()
	{
		Debug.Log("ServerController Start runs");

        itemcontainerToGO = GameController.Instance.ItemContainerToGO;
        foreach (Server server in GameController.Instance.PlantableServerList)
        {
            server.Plant += Server_Plant;
            //server.Planted += Server_Planted;
        }
    }

    public void Server_Planted(Server server)
    {
        //  Debug.Log("ServerController::Server_Plant " + server.Name);
        foreach (ItemContainer itemContainer in GameController.Instance.ItemContainerToGO.Keys)
        {
            Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
            itemContainer.OnClick -= ItemContainer_OnClick;
            if (itemContainer.hasServer)
                itemContainer.OnClick += ClickOnServer;
        }
        selectedServer = null;
        GameController.Instance.plantingServer = null;
    }

    private void ClickOnServer(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        GameController.Instance.ItemContainerToServer[container].Update();
    }

    private void Server_Plant(Server server)
    {
		Debug.Log("ServerController::Server_Plant " + server.Name);
		Debug.Log(GameController.Instance.ItemContainerToGO.Count);
		foreach (ItemContainer itemContainer in itemcontainerToGO.Keys)
		{
			Debug.Log(GameController.Instance.ItemContainerToGO[itemContainer] == null);
			Debug.Log(itemContainer.GetHashCode());
		}

        selectedServer = server;
        foreach (ItemContainer itemContainer in itemcontainerToGO.Keys)
        {
            if (itemContainer.hasServer == false)
            {
                Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>("Images\\" + selectedServer.spriteName);
                image.color = new Color(0, 0, 0, 0.1f);
                // GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().SetNativeSize();
                if (itemContainer.IsClickable == false)
                    itemContainer.OnClick += ItemContainer_OnClick;
                
            }
            else
            {
                //if item container has server then it means it has a click event on it we dont wanna update server while planting
                itemContainer.OnClick -= ClickOnServer;
            }
        }
        GameController.Instance.plantingServer = selectedServer;
    }

    private void ItemContainer_OnClick(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        Debug.Log("ServerController::ItemContainer_OnClick ::PLANTED" + pointerEventData.position);

        GameObject serverGO = Instantiate(serverPrefab);
        serverGO.transform.SetParent(itemcontainerToGO[container].transform, false);
        serverGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\" + selectedServer.spriteName);

        //serverGO.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        serverGO.GetComponentInChildren<TextMeshProUGUI>().text = selectedServer.Name;

        Debug.Log("Update added");
        selectedServer.UpdateEvent += GameController.Instance.ServerUpdate;

        GameController.Instance.plantedServersToGOs[selectedServer] = serverGO;

        container.hasServer = true;
        GameController.Instance.ItemContainerToServer[container] = selectedServer;
        //after server planted selected server gets set to null so be carefull where you do this
        selectedServer.PlantedServer();

    }
}
