using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI currentshelfpriceText;
    public TextMeshProUGUI currentMoneyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentshelfpriceText.text = GameContoller.Instance.shelfPrice.ToString();
        currentMoneyText.text = GameContoller.Instance.money.ToString();
    }
    public void UnlockShelf()
    {
        GameContoller.Instance.UnlockShelf();
        
    }
}
