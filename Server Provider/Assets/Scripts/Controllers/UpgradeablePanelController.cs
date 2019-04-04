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
    AnimationsController animationsController;
    public Dictionary<Server, Button> servers2buttonsInUpgradeablePanels;
    Server[] servers;

    // Start is called before the first frame update
    void Start()
    {
        servers2buttonsInUpgradeablePanels = new Dictionary<Server, Button>();
        animationsController = AnimationsController.Instance;
        servers = GameController.Instance.PlantableServerList.ToArray();
        GameController.Instance.LeveledUp += Player_LeveledUp;
        foreach (Server server in servers)
        {
            GameObject upgradeableGO = Instantiate(upgradeablePrefab);
            upgradeableGO.transform.SetParent(parentObjectTransform, false);

            upgradeableGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = server.Name;
            upgradeableGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\" + server.spriteName);

            Button btn = upgradeableGO.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => { Plant(upgradeableGO, server); });
            if (server.requiredLevel > GameController.Instance.level)
            {
                btn.interactable = false;
                btn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Required Level " + server.requiredLevel;
            }

            servers2buttonsInUpgradeablePanels.Add(server, btn);
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
        foreach (Server server in servers2buttonsInUpgradeablePanels.Keys)
        {
            if (server.requiredLevel <= GameController.Instance.level)
            {
                if (servers2buttonsInUpgradeablePanels[server].interactable == false)
                    servers2buttonsInUpgradeablePanels[server].transform.GetComponentInChildren<TextMeshProUGUI>().text = "Plant";

                servers2buttonsInUpgradeablePanels[server].interactable = true;


            }
        }
    }

    //we may need upgradeable panel objects with the servers 
    //Dictionary<server,gameobject> so when we plant a server then we can find the object and set the texts or other things easyly
    public void Plant(object sender, Server server)
    {
        Debug.Log(server.Name + "plant active");
        
        GameObject upgradeableGO = (GameObject)sender;

        Server copy = server.Copy();
        copy.plantable = false;
        copy.upgradeable = true;
        //this doesnt work yet
        //we will add events to server about how to behave when this happen 
        //then it will work
        copy.PlantServer();

        copy.Planted += Copy_Planted;
        copy.Planted += ServerController.Instance.Server_Planted;
        animationsController.UpgradesOpenCloseAnim(false);
    }

    private void Copy_Planted(Server server)
    {
        //find the copied server in the upgradeable panel
        foreach (Server servers in servers2buttonsInUpgradeablePanels.Keys)
        {
            if (server.Name == servers.Name)
            {
                Transform buttonTransform = servers2buttonsInUpgradeablePanels[servers].gameObject.transform;
                buttonTransform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade\n" + Extensions.Format(server.requiredMoneyForUpgrade);
                Button btn = buttonTransform.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => { UpgradeServerButtonClicked(servers2buttonsInUpgradeablePanels[servers], server); });
            }
        }
        server.Upgraded += Server_Upgraded;
    }

    private void UpgradeServerButtonClicked(object sender, Server server)
    {
        Debug.Log(server.Name + " UpgradeablePanelController::UpgradeServer");
        if (GameController.Instance.money - server.requiredMoneyForUpgrade < 0)
            return;

        GameController.Instance.money -= server.requiredMoneyForUpgrade;
        
        server.UpgradeServer();   
    }

    private void Server_Upgraded(Server server)
    {
        Debug.Log("UpgradeablePanelController::Server_Upgraded");
        foreach (Server servers in servers2buttonsInUpgradeablePanels.Keys)
        {
            if (server.Name == servers.Name)
            {
                Transform buttonTransform = servers2buttonsInUpgradeablePanels[servers].gameObject.transform;
                buttonTransform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade \n" + Extensions.Format(server.requiredMoneyForUpgrade);
            }
        }
        
    }
}
