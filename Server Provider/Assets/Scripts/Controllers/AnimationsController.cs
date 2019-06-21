using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    public Animator animator;
    static public AnimationsController Instance;
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

    public void UpgradesOpenCloseAnim(bool openclose)
    {
        animator.SetBool("panelOpenCloseState", openclose);
    }
}
