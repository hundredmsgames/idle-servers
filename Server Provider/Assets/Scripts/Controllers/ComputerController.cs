using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ComputerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Computer computer;
    bool continuesTouchStarted;
    //time passed after we start touching the object
    float timePassed = 0;
    //maximum time for pressing object that we are on
    float maxTime = 1.3f;
    //animation will start to play afer 0.X seconds
    float animationStartTime = 0.3f;
    bool animationStarted = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        continuesTouchStarted = true;
        timePassed = 0;
        // Debug.Log("ComputerController::OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("ComputerController::OnPointerUp");
        computer.Update();
        animationStarted = false;
        continuesTouchStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (continuesTouchStarted)
        {
            timePassed += Time.deltaTime;
        }
        if (maxTime < timePassed)
        {

            continuesTouchStarted = false;
            timePassed = 0;

            ///this is the part we will do what ever we want

            Debug.Log("you are still touching"+ computer.Name);


        }
        if (animationStarted == false && animationStartTime < timePassed)
        {
            animationStarted = true;
            ///start the animation here
            Debug.Log("animation started");
        }
    }






}
