using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeablePanelController : MonoBehaviour
{
    public Transform parentObjectTransform;
    public GameObject upgradeablePrefab;
    public Dictionary<Item, GameObject> itemToUpgradeContainer;
    Item[] items;
    public static UpgradeablePanelController Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            return;
        //TODO:hghjgj
        Instance = this;
        itemToUpgradeContainer = new Dictionary<Item, GameObject>();
        GameController.Instance.LeveledUp += Player_LeveledUp;

        foreach (ItemInfo itemInfo in ItemPrototype.GetItemInfos())
        {
            Item proto = itemInfo.item;

            GameObject upgradeableGO = Instantiate(upgradeablePrefab);
            upgradeableGO.transform.SetParent(parentObjectTransform, false);
            upgradeableGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = proto.Name;
            upgradeableGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\Animals\\" + proto.Name);
            // Debug.Log(proto.Name);

            Button btn = upgradeableGO.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => { Plant(proto.Name); });
            if (proto.requiredLevel > GameController.Instance.level)
            {
                btn.interactable = false;
                btn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Required Level " + proto.requiredLevel;
            }

            itemToUpgradeContainer.Add(proto, upgradeableGO);

            //we need to set behaviour of this upgradeable or plantable Item objects
            //for example is this object active?
            //is this upgradeable or plantable
            //yerleştirilmiş ve son seviyeye kadar geliştirilmiş objeler nasıl davranacak?
            //son seviyeye geldiği zaman yerleştirme aktif olacak yani artık buton başka bir obje için çalışacak diğeri ile olan bağı kopacak
            //bu durumda buttonlar hangi objeye bağlı olduğunu bilecek
            //plant yaparken bunu bildirebiliriz
            //bunun için mantıklı bir yöntem düşün

        }
    }

    private void Player_LeveledUp(int level)
    {
        foreach (Item item in itemToUpgradeContainer.Keys)
        {
            if (item.requiredLevel <= GameController.Instance.level)
            {
                Button button = itemToUpgradeContainer[item].GetComponentInChildren<Button>();
                if (button.interactable == false)
                    button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Plant";

                button.interactable = true;
            }
        }
    }

    //we may need upgradeable panel objects with the Items 
    //Dictionary<Item,gameobject> so when we plant a Item then we can find the object and set the texts or other things easyly
    public void Plant(string ItemName)
    {

        //Şuanda prototype ismini aldık ve bunun oluşturulması ve gerekli yerelre eklenmesi lazım 
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log(ItemName + " is ready to plant");
        GameController.Instance.itemPlant(ItemName);
        //copy.Planted += ItemPlanted;
        //copy.Planted += GameController.Instance.ItemPlanted;
        GameUIController.Instance.UpgradesOpenCloseAnim(false);
    }

    public void ItemPlanted(Item item)
    {
        //find the copied Item in the upgradeable panel
        foreach (Item itemUpgradeable in itemToUpgradeContainer.Keys)
        {
            if (item.Name == itemUpgradeable.Name)
            {
                Button button = itemToUpgradeContainer[itemUpgradeable].GetComponentInChildren<Button>();
                button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Extensions.Format(item.requiredMoneyForUpgrade);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { UpgradeItemButtonClicked(itemToUpgradeContainer[itemUpgradeable], item as Item); });
            }
        }
        item.Upgraded += ItemUpgraded;
    }

    private void UpgradeItemButtonClicked(object sender, Item item)
    {
        Debug.Log(item.Name + " UpgradeablePanelController::UpgradeItem");
        if (GameController.Instance.money - item.requiredMoneyForUpgrade < 0)
            return;

        GameController.Instance.money -= item.requiredMoneyForUpgrade;

        item.Upgrade();
    }

    private void ItemUpgraded(Item item)
    {
        Debug.Log("UpgradeablePanelController::ItemUpgraded");
        foreach (Item itemUpgradeable in itemToUpgradeContainer.Keys)
        {
            if (item.Name == itemUpgradeable.Name)
            {
                Button button = itemToUpgradeContainer[itemUpgradeable].GetComponentInChildren<Button>();
                button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade \n" + Extensions.Format(item.requiredMoneyForUpgrade);
            }
        }

    }
}
