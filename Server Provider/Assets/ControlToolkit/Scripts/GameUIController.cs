using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI currentshelfpriceText;
    public TextMeshProUGUI currentMoneyText;
    public Image filledImage;
    public TextMeshProUGUI currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentshelfpriceText.text ="Unluck " + SIPrefix.GetInfo(GameController.Instance.shelfPrice,1).AmountWithPrefix;
        currentMoneyText.text = SIPrefix.GetInfo(GameController.Instance.money, 1).AmountWithPrefix ;
        filledImage.fillAmount = GameController.Instance.levelProgress;
        currentLevel.text ="Level "+ GameController.Instance.level.ToString() ;
    }
    public void UnlockShelf()
    {
        GameController.Instance.UnlockShelf();
        
    }
}
