using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ControlToolkit;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    public Animator UpgradesPanelAnimator;
    public Animator archivedItemsAnimator;
    public Animator ItemManagamentAnimatorController;
    public Image CloseUpgradeablePanelButtonImage;

    public TextMeshProUGUI currentshelfpriceText;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI currentLevel;
    public Image filledImage;

    public ScrollRect scrollRect;
    public GameObject dragEnabledScreen;
    public GameObject ItemManagamentPanel;

    private void Start()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    void Update()
    {
        // FIXME: Probably we shouldn't update them in the update method.
        // They could be triggered by an event maybe
        if (GameController.Instance.shelfRequiredLevel > GameController.Instance.level)
        {
            currentshelfpriceText.text = "Requiered Level " + GameController.Instance.shelfRequiredLevel;
            currentshelfpriceText.gameObject.transform.parent.GetComponent<Button>().interactable = false;
            currentshelfpriceText.gameObject.transform.parent.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            currentshelfpriceText.gameObject.transform.parent.GetChild(1).gameObject.SetActive(true);
            currentshelfpriceText.gameObject.transform.parent.GetComponent<Button>().interactable = true;
            currentshelfpriceText.text = "Unluck " + Extensions.Format(GameController.Instance.shelfPrice);
        }
        currentMoneyText.text = Extensions.Format(GameController.Instance.money);
        filledImage.fillAmount = GameController.Instance.levelProgress;
        currentLevel.text = "Level " + GameController.Instance.level.ToString();
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

        ArchivedItemsOpenCloseAnim(!scrollEnabled);
        UpgradesOpenCloseAnim(false);

        foreach (ItemController ItemController in GameController.Instance.ItemControllers)
        {
            ItemController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = scrollEnabled;
        }
    }

    public void ShowItemManagamentPanel(GameObject ItemController)
    {
        ItemManagamentOpenCloseAnim(true);
        ItemManagamentPanel.transform.position = ItemController.transform.position;
        ItemManagamentPanel.transform.Translate(0, 150, 0);
        CloseUpgradeablePanelButtonImage.raycastTarget = true;
    }

    public void HideItemManagamentPanel()
    {
        ItemManagamentOpenCloseAnim(false);
        CloseUpgradeablePanelButtonImage.raycastTarget = false;
    }

    public void UpgradesOpenCloseAnim(bool open)
    {
        UpgradesPanelAnimator.SetBool("panelOpenCloseState", open);
        if (open)
        {
            GameController.Instance.ShowHidePlantablePositions(false);
            HideItemManagamentPanel();
        }
        else
        {
            //set CloseUpgradeablepanelButton raycast=false
            CloseUpgradeablePanelButtonImage.raycastTarget = false;
        }
    }

    public void ArchivedItemsOpenCloseAnim(bool open)
    {
        archivedItemsAnimator.SetBool("open", open);
    }

    public void ItemManagamentOpenCloseAnim(bool open)
    {
        ItemManagamentAnimatorController.SetBool("open", open);
    }

    public void PutItemToArchiveButton_OnClick()
    {
        GameController.Instance.PutItemToArchive();
    }
}
