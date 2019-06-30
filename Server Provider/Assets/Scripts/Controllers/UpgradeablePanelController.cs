using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeablePanelController : MonoBehaviour
{
    public Transform parentObjectTransform;
    public GameObject upgradeablePrefab;
    public Dictionary<Computer, GameObject> serverToUpgradeContainer;
    Computer[] servers;
    public static UpgradeablePanelController Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            return;
        
        Instance = this;
        serverToUpgradeContainer = new Dictionary<Computer, GameObject>();
        GameController.Instance.LeveledUp += Player_LeveledUp;

        foreach (ComputerInfo computerInfo in GameController.Instance.nameToComputerInfo.Values)
        {
            Computer proto = computerInfo.computerProto;

            GameObject upgradeableGO = Instantiate(upgradeablePrefab);
            upgradeableGO.transform.SetParent(parentObjectTransform, false);
            upgradeableGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = proto.Name;
            upgradeableGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\" + proto.Name);

            Button btn = upgradeableGO.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => { Plant(proto.Name); });
            if (proto.requiredLevel > GameController.Instance.level)
            {
                btn.interactable = false;
                btn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Required Level " + proto.requiredLevel;
            }

            serverToUpgradeContainer.Add(proto, upgradeableGO);

            //we need to set behaviour of this upgradeable or plantable server objects
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
        foreach (Computer server in serverToUpgradeContainer.Keys)
        {
            if (server.requiredLevel <= GameController.Instance.level)
            {
                Button button = serverToUpgradeContainer[server].GetComponentInChildren<Button>();
                if (button.interactable == false)
                    button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Plant";

                button.interactable = true;
            }
        }
    }

    //we may need upgradeable panel objects with the servers 
    //Dictionary<server,gameobject> so when we plant a server then we can find the object and set the texts or other things easyly
    public void Plant(string computerName)
    {

        //Şuanda prototype ismini aldık ve bunun oluşturulması ve gerekli yerelre eklenmesi lazım 
        if (DebugConfigs.DEBUG_LOG)
            Debug.Log(computerName + " is ready to plant");
        GameController.Instance.ComputerPlant(computerName);
        //copy.Planted += ComputerPlanted;
        //copy.Planted += GameController.Instance.ComputerPlanted;
        GameUIController.Instance.UpgradesOpenCloseAnim(false);
    }

    public void ComputerPlanted(Computer computer)
    {
        //find the copied server in the upgradeable panel
        foreach (Computer servers in serverToUpgradeContainer.Keys)
        {
            if (computer.Name == servers.Name)
            {
                Button button = serverToUpgradeContainer[servers].GetComponentInChildren<Button>();
                button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Extensions.Format(computer.requiredMoneyForUpgrade);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { UpgradeServerButtonClicked(serverToUpgradeContainer[servers], computer); });
            }
        }
        computer.Upgraded += ComputerUpgraded;
    }

    private void UpgradeServerButtonClicked(object sender, Computer server)
    {
        Debug.Log(server.Name + " UpgradeablePanelController::UpgradeServer");
        if (GameController.Instance.money - server.requiredMoneyForUpgrade < 0)
            return;

        GameController.Instance.money -= server.requiredMoneyForUpgrade;

        server.UpgradeServer();
    }

    private void ComputerUpgraded(Computer server)
    {
        Debug.Log("UpgradeablePanelController::ComputerUpgraded");
        foreach (Computer servers in serverToUpgradeContainer.Keys)
        {
            if (server.Name == servers.Name)
            {
                Button button = serverToUpgradeContainer[servers].GetComponentInChildren<Button>();
                button.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade \n" + Extensions.Format(server.requiredMoneyForUpgrade);
            }
        }

    }
}
