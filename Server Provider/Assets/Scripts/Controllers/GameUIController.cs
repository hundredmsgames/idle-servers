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
        currentshelfpriceText.text ="Unluck " + Extensions.Format(GameController.Instance.shelfPrice);
        currentMoneyText.text = Extensions.Format(GameController.Instance.money);
        filledImage.fillAmount = GameController.Instance.levelProgress;
        currentLevel.text ="Level "+ GameController.Instance.level.ToString() ;
    }

    public void UnlockShelf()
    {
        GameController.Instance.UnlockShelf();
        
    }
}
