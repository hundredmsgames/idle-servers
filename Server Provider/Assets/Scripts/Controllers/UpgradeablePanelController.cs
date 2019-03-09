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

    Server[] servers;

    // Start is called before the first frame update
    void Start()
    {

        animationsController = AnimationsController.Instance;
        servers = GameContoller.Instance.PlantableServerList.ToArray();
        foreach (Server server in servers)
        {
            GameObject upgradeableGO = Instantiate(upgradeablePrefab);
            upgradeableGO.transform.SetParent(parentObjectTransform, false);

            upgradeableGO.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = server.Name;
            upgradeableGO.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images\\"+server.spriteName);
            upgradeableGO.GetComponentInChildren<Button>().onClick.AddListener(()=>{ Plant(server); });
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
    //we may need upgradeable panel objects with the servers 
    //Dictionary<server,gameobject> so when we plant a server then we can find the object and set the texts or other things easyly
    public void Plant(Server server)
    {
        Debug.Log(server.Name + "plant active");

        Server copy = server.Copy();
        copy.plantable = false;
        copy.upgradeable = true;
        //this doesnt work yet
        //we will add events to server about how to behave when this happen 
        //then it will work
        copy.PlantServer();
        animationsController.UpgradesOpenCloseAnim(false);
        GameContoller.Instance.plantedServersToGOs.Add(copy, null);
    }
    public void Upgrade(Server server)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
