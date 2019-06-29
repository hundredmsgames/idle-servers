using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationsController : MonoBehaviour
{
    public Animator UpgradesPanelAnimator;
    public Animator RecycleBinAnimator;
    static public AnimationsController Instance;
    public Image CloseUpgradeablePanelButtonImage;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (Instance != null)
            return;
		
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpgradesOpenCloseAnim(bool open)
    {
        UpgradesPanelAnimator.SetBool("panelOpenCloseState", open);
        if (open)
        {
            GameController.Instance.selectedComputerName = null;
            GameController.Instance.ShowHidePlantablePositions(false);
        }
        else
        {
            //set CloseUpgradeablepanelButton raycast=false
            CloseUpgradeablePanelButtonImage.raycastTarget = false;
        }
    }

    public void RecycleBinOpenCloseAnim(bool open)
    {
        RecycleBinAnimator.SetBool("open", open);
        
    }
}
