using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item item;
    bool continuesTouchStarted;
    //time passed after we start touching the object
    float timePassed = 0;
    //maximum time for pressing object that we are on
    float maxTime = 1f;
    //animation will start to play afer 0.X seconds
    float animationStartTime = 0.3f;
    bool animationStarted = false;

    // Update is called once per frame
    void Update()
    {
        if (!continuesTouchStarted)
            return;

        timePassed += Time.deltaTime;
        if (maxTime < timePassed)
        {
            Debug.Log("you are still touching" + item.Name);

            continuesTouchStarted = false;
            timePassed = 0;

            ///this is the part we will do what ever we want
            GameUIController.Instance.ShowItemManagamentPanel(this.gameObject);
            GameController.Instance.itemToBeArchived = this.item;
        }

        if (animationStarted == false && animationStartTime < timePassed)
        {
            animationStarted = true;
            ///start the animation here
            Debug.Log("animation started");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        continuesTouchStarted = true;
        timePassed = 0;
        // Debug.Log("ItemController::OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("ItemController::OnPointerUp");
        item.Update();
        animationStarted = false;
        continuesTouchStarted = false;
    }




}
