using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    public static AnimationsController Instance;
    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    void Start()
    {

    }

    public void PlayItemAnimatons(ItemController itemController)
    {
        itemController.gameObject.GetComponent<Animator>().SetTrigger("blob");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
