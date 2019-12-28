using ControlToolkit;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameController : MonoBehaviour
{
    public GameObject itemPrefab;

    // This is the selected Item to be planted.
    public string selecteditem;

    // This is the Item that we want to put into archive
    public Item itemToBeArchived;

    public void itemPlanted(Item item)
    {
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("GameController::Item_Planted::" + item.Name);

        ShowHidePlantablePositions(false);
        selecteditem = null;
    }

    public void itemPlant(string itemName)
    {
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("GameController::Item_Plant " + itemName);

        selecteditem = itemName;
        ShowHidePlantablePositions(true);
    }

    public void ShowHidePlantablePositions(bool show)
    {
        if (show == true)
        {
            foreach (ItemContainer itemContainer in ItemContainerToGO.Keys)
            {
                if (itemContainer.hasItem == false)
                {
                    Image image = GameController.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>();
                    image.sprite = Resources.Load<Sprite>("Images\\Animals\\" + selecteditem);
                    image.color = new Color(0, 0, 0, 0.1f);
                    itemContainer.OnClick += ItemContainer_OnClick;

                    // GameContoller.Instance.ItemContainerToGO[itemContainer].GetComponent<Image>().SetNativeSize();
                }
                else
                {
                    //if item container has Item then it means it has a click event on it we dont wanna update Item while planting
                    // itemContainer.OnClick -= ClickOnItem;
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
                //if (itemContainer.hasItem)
                //    itemContainer.OnClick += ClickOnItem;
            }
        }
    }

    private void ItemContainer_OnClick(ItemContainer container, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("ItemController::ItemContainer_OnClick::PLANTED" + pointerEventData.position);

        GameObject ItemGO = Instantiate(itemPrefab);
        ItemGO.transform.SetParent(ItemContainerToGO[container].transform, false);
        ItemGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\Animals\\" + selecteditem);
        ItemGO.GetComponentInChildren<TextMeshProUGUI>().text = selecteditem;
        //ItemGO.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log("Update event added to Item.");

        ItemController ItemController = ItemGO.GetComponent<ItemController>();
        Item item = ItemPrototype.GetItemInfo(selecteditem).item.Clone() as Item;
        item.Planted += itemPlanted;
        item.Planted += UpgradeablePanelController.Instance.ItemPlanted;
        item.UpdateEvent += ItemUpdate;

        //planted eventları
        GameController.Instance.planteditemsToGOs[item] = ItemGO;
        GameController.Instance.ItemContainerToitem[container] = item;
        container.hasItem = true;
        ItemController.item = item as Item;
        ItemControllers.Add(ItemController);
        // After Item planted selected Item gets set to null so be carefull where you do this
        item.ItemPlanted();

    }

    public void PutItemToArchive()
    {
        Debug.Log(itemToBeArchived.Name);

    }
}
