using ControlToolkit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameController : MonoBehaviour
{
    public GameObject computerPrefab;

    // This is the selected computer to be planted.
    public string selectedComputerName;

    // This is the computer that we want to put into archive
    public Computer computerToBeArchived;

    public void ComputerPlanted(Computer computer)
    {
        if(DebugConfigs.DEBUG_LOG)
            Debug.Log("GameController::Computer_Planted::" + computer.Name);

        ShowHidePlantablePositions(false);
        selectedComputerName = null;
    }

    public void ComputerPlant(string computerName)
    {
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("GameController::Computer_Plant " + computerName);

        selectedComputerName = computerName;
        ShowHidePlantablePositions(true);
    }

    public void ShowHidePlantablePositions(bool show)
    {
        if (show == true)
        {
            foreach (ItemContainer itemContainer in ItemContainerToGO.Keys)
            {
                if (itemContainer.hasServer == false)
                {
                    Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
                    image.sprite = Resources.Load<Sprite>("Images\\" + selectedComputerName);
                    image.color = new Color(0, 0, 0, 0.1f);
                    itemContainer.OnClick += ItemContainer_OnClick;

                    // GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().SetNativeSize();
                }
                else
                {
                    //if item container has server then it means it has a click event on it we dont wanna update server while planting
                   // itemContainer.OnClick -= ClickOnComputer;
                }
            }
        }
        else
        {
            //hide plantable positions
            foreach (ItemContainer itemContainer in GameController.Instance.ItemContainerToGO.Keys)
            {
                Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
                image.sprite = null;
                image.color = new Color(0, 0, 0, 0);
                itemContainer.OnClick -= ItemContainer_OnClick;
                //if (itemContainer.hasServer)
                //    itemContainer.OnClick += ClickOnComputer;
            }
        }
    }

    private void ItemContainer_OnClick(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("ServerController::ItemContainer_OnClick::PLANTED" + pointerEventData.position);

        GameObject serverGO = Instantiate(computerPrefab);
        serverGO.transform.SetParent(ItemContainerToGO[container].transform, false);
        serverGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\" + selectedComputerName);
        serverGO.GetComponentInChildren<TextMeshProUGUI>().text = selectedComputerName;
        //serverGO.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("Update event added to server.");

        Computer newComputer = nameToComputerInfo[selectedComputerName].computerProto.Copy();
        newComputer.Planted += ComputerPlanted;
        newComputer.Planted += UpgradeablePanelController.Instance.ComputerPlanted;
        newComputer.UpdateEvent += Instance.ServerUpdate;
        //planted eventları
        GameController.Instance.plantedServersToGOs[newComputer] = serverGO;
        GameController.Instance.ItemContainerToComputer[container] = newComputer;
        container.hasServer = true;
        ComputerController computerController = serverGO.GetComponent<ComputerController>();
        computerController.computer = newComputer;
        computerControllers.Add(computerController);

        // After server planted selected server gets set to null so be carefull where you do this
        newComputer.ComputerPlanted();
        
    }

    public void PutComputerToArchive()
    {
        Debug.Log(computerToBeArchived.Name);

    }
}
