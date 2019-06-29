using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ControlToolkit;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI currentshelfpriceText;
    public TextMeshProUGUI currentMoneyText;
    public Image filledImage;
    public TextMeshProUGUI currentLevel;

    public ScrollRect scrollRect;
    public GameObject dragEnabledScreen;

    // Update is called once per frame
    void Update()
    {
        currentshelfpriceText.text ="Unluck " + Extensions.Format(GameController.Instance.shelfPrice);
        currentMoneyText.text = Extensions.Format(GameController.Instance.money);
        filledImage.fillAmount = GameController.Instance.levelProgress;
        currentLevel.text ="Level "+ GameController.Instance.level.ToString() ;
    }

    public void UnlockShelf()
    {
        GameController.Instance.UnlockShelf();
    }

    public void DragEnableDisable()
    {
        bool scrollEnabled = false;
        foreach (ItemContainer item in GameController.Instance.ItemContainerToGO.Keys)
        {
            //if item drag is enabled disable it and pass the info to scrollEnabled flag
            item.CanDrag = !item.CanDrag;
            //if we can not drag drop items so scroll is disabled (set to false)
            scrollEnabled = !item.CanDrag;
            //Debug.Log(item.CanDrag);
        }

        scrollRect.vertical = scrollEnabled;

        //FIXME: This code has to change this is for debugging
        if (scrollRect.vertical == false)
            dragEnabledScreen.GetComponent<Image>().color = Color.green;
        else
            dragEnabledScreen.GetComponent<Image>().color = Color.white;

        AnimationsController.Instance.RecycleBinOpenCloseAnim(!scrollEnabled);
        AnimationsController.Instance.UpgradesOpenCloseAnim(false);

        foreach (ComputerController computerController in GameController.Instance.computerControllers)
        {
            computerController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = scrollEnabled;
        }
    }
}
