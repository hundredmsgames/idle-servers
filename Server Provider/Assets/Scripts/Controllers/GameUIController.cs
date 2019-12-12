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
    public Animator archivedComputersAnimator;
    public Animator computerManagamentAnimatorController;
    public Image CloseUpgradeablePanelButtonImage;

    public TextMeshProUGUI currentshelfpriceText;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI currentLevel;
    public Image filledImage;

    public ScrollRect scrollRect;
    public GameObject dragEnabledScreen;
    public GameObject computerManagamentPanel;

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

        ArchivedComputersOpenCloseAnim(!scrollEnabled);
        UpgradesOpenCloseAnim(false);

        foreach (ComputerController computerController in GameController.Instance.computerControllers)
        {
            computerController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = scrollEnabled;
        }
    }

    public void ShowComputerManagamentPanel(GameObject computerController)
    {
        ComputerManagamentOpenCloseAnim(true);
        computerManagamentPanel.transform.position = computerController.transform.position;
        computerManagamentPanel.transform.Translate(0, 150, 0);
        CloseUpgradeablePanelButtonImage.raycastTarget = true;
    }

    public void HideComputerManagamentPanel()
    {
        ComputerManagamentOpenCloseAnim(false);
        CloseUpgradeablePanelButtonImage.raycastTarget = false;
    }

    public void UpgradesOpenCloseAnim(bool open)
    {
        UpgradesPanelAnimator.SetBool("panelOpenCloseState", open);
        if (open)
        {
            GameController.Instance.ShowHidePlantablePositions(false);
            HideComputerManagamentPanel();
        }
        else
        {
            //set CloseUpgradeablepanelButton raycast=false
            CloseUpgradeablePanelButtonImage.raycastTarget = false;
        }
    }

    public void ArchivedComputersOpenCloseAnim(bool open)
    {
        archivedComputersAnimator.SetBool("open", open);
    }

    public void ComputerManagamentOpenCloseAnim(bool open)
    {
        computerManagamentAnimatorController.SetBool("open", open);
    }
    
    public void PutComputerToArchiveButton_OnClick()
    {
        GameController.Instance.PutComputerToArchive();
    }
}
