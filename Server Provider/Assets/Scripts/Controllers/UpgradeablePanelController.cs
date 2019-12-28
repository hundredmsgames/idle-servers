using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeablePanelController : MonoBehaviour
{

    /// <summary>
    /// biz her plantable olanın plant edilip edilmediğini bilmek istiyor muyuz?
    /// her item için bir limit olmalı  mı 🤔 
    /// yapılacaklar
    /// upgradeable panel içinde item leveli görüntülenecek
    /// max levele ulaşan item için gerekli adımlar işlenecek.
    /// 1) itemNameToActiveItem içinde item null yapılacak (null yapılınca button otomatik olarak plant işlemine dönecek )
    /// 
    /// </summary>





    public Transform parentObjectTransform;
    public GameObject upgradeablePrefab;
    public Dictionary<string, GameObject> itemNameToUpgradeContainer;
    public Dictionary<string, Item> itemNameToActiveItem;


    public static UpgradeablePanelController Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            return;
        //TODO:hghjgj
        Instance = this;
        itemNameToUpgradeContainer = new Dictionary<string, GameObject>();
        itemNameToActiveItem = new Dictionary<string, Item>();

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

            itemNameToUpgradeContainer.Add(proto.Name, upgradeableGO);
            itemNameToActiveItem.Add(proto.Name, null);


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
    private void Update()
    {
        foreach (string itemName in itemNameToActiveItem.Keys)
        {
            Button button = itemNameToUpgradeContainer[itemName].GetComponentInChildren<Button>();
            if (ItemPrototype.GetItemInfo(itemName).item.requiredLevel > GameController.Instance.level)
            {
                button.interactable = false;
                return;
            }
            Item item = itemNameToActiveItem[itemName];
            //this means we can plant this but we did not do that yet
            if (item == null)
            {
                if (ItemPrototype.GetItemInfo(itemName).item.requiredMoneyForPlant > GameController.Instance.money)
                    button.interactable = false;
                else
                    button.interactable = true;
            }
            //this means we planted an item 
            else
            {
                if (item.requiredMoneyForUpgrade > GameController.Instance.money)
                {
                    button.interactable = false;
                }
                else
                {
                    button.interactable = true;
                }
            }

        }
    }
    private void Player_LeveledUp(int level)
    {
        foreach (string itemName in itemNameToUpgradeContainer.Keys)
        {
            if (ItemPrototype.GetItemInfo(itemName).item.requiredLevel <= GameController.Instance.level)
            {
                Button button = itemNameToUpgradeContainer[itemName].GetComponentInChildren<Button>();
                if (itemNameToActiveItem[itemName] == null)
                {
                    int reqMoney = ItemPrototype.GetItemInfo(itemName).item.requiredMoneyForPlant;
                    string plantInfo = "Plant\n" + (reqMoney > 0 ? Extensions.Format(reqMoney) : "");
                    button.transform.GetComponentInChildren<TextMeshProUGUI>().text = plantInfo;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => { Plant(itemName); });

                }
                else
                    continue;
                Debug.Log(itemName + ":" + (itemNameToActiveItem[itemName] == null));
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

        Button button = itemNameToUpgradeContainer[item.Name].GetComponentInChildren<Button>();
        button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Extensions.Format(item.requiredMoneyForUpgrade);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => { UpgradeItemButtonClicked(itemNameToUpgradeContainer[item.Name], item); });

        item.Upgraded += ItemUpgraded;

        GameController.Instance.money -= item.requiredMoneyForPlant;
        //for second time to plant an item we will set the price to 0 in the proto
        //ikinci kez aynı nesneyi plant etmek için bir ücret istemiyoruz(tartışmaya açık)
        ItemPrototype.GetItemInfo(item.Name).item.requiredMoneyForPlant = 0;

        //itemler startta isimleri ile ekleniyorlar
        if (itemNameToActiveItem.ContainsKey(item.Name))
        {
            //her seferinde burada referans değişmeli. Biz bir item ekledik ve sonra onu son seviye yaptık.
            //button tekrar plant olacak ve yeni bir item plant edeceğiz artık takip etmemiz gereken item o olacak
            itemNameToActiveItem[item.Name] = item;
        }
        else
            Debug.LogError("How did this happen?? This item should not be exist!!!");
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

        Button button = itemNameToUpgradeContainer[item.Name].GetComponentInChildren<Button>();
        button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade \n" + Extensions.Format(item.requiredMoneyForUpgrade);



    }
}
