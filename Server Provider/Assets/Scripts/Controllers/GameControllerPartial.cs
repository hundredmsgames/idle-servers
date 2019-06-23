using ControlToolkit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameController : MonoBehaviour
{
    public GameObject computerPrefab;
    public Computer selectedComputer;

    void Start()
	{
        foreach (ComputerInfo computerInfo in nameToComputerInfo.Values)
        {
            computerInfo.computerProto.Plant += ComputerPlant;
        }
    }

    public void ComputerPlanted(Computer computer)
    {
        Debug.Log("GameController::Computer_Planted::" + computer.Name);
        foreach (ItemContainer itemContainer in GameController.Instance.ItemContainerToGO.Keys)
        {
            Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
            itemContainer.OnClick -= ItemContainer_OnClick;
            if (itemContainer.hasServer)
                itemContainer.OnClick += ClickOnComputer;
        }
        selectedComputer = null;
    }

    private void ClickOnComputer(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        GameController.Instance.ItemContainerToComputer[container].Update();
    }

    private void ComputerPlant(Computer computer)
    {
		Debug.Log("GameController::Computer_Plant " + computer.Name);

        selectedComputer = computer;
        foreach (ItemContainer itemContainer in ItemContainerToGO.Keys)
        {
            if (itemContainer.hasServer == false)
            {
                Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>("Images\\" + selectedComputer.Name);
                image.color = new Color(0, 0, 0, 0.1f);
                itemContainer.OnClick += ItemContainer_OnClick;

                // GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().SetNativeSize();
            }
            else
            {
                //if item container has server then it means it has a click event on it we dont wanna update server while planting
                itemContainer.OnClick -= ClickOnComputer;
            }
        }
    }

    private void ItemContainer_OnClick(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        Debug.Log("ServerController::ItemContainer_OnClick::PLANTED" + pointerEventData.position);

        GameObject serverGO = Instantiate(computerPrefab);
        serverGO.transform.SetParent(ItemContainerToGO[container].transform, false);
        serverGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\" + selectedComputer.Name);
        serverGO.GetComponentInChildren<TextMeshProUGUI>().text = selectedComputer.Name;
        //serverGO.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);

        Debug.Log("Update event added to server.");
        selectedComputer.UpdateEvent += GameController.Instance.ServerUpdate;

        GameController.Instance.plantedServersToGOs[selectedComputer] = serverGO;
        GameController.Instance.ItemContainerToComputer[container] = selectedComputer;
        container.hasServer = true;

        // After server planted selected server gets set to null so be carefull where you do this
        selectedComputer.PlantedServer();
    }
}
