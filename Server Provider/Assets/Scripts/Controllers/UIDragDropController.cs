using ControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDragDropController : MonoBehaviour
{
    private int m_index;

    public ScrollRect scrollRect;
    public GameObject dragEnabledScreen;

    [SerializeField] ArrangeItemsControl ArrangeItemsControl;
    List<ItemDataBindingEventArgs> allItems;
    // Start is called before the first frame update
    void Start()
    {
        ArrangeItemsControl.ItemDataBinding += OnItemDataBinding;
        ArrangeItemsControl.ItemsArranged += OnItemsArranged;
        ArrangeItemsControl.ItemAdd += OnItemsAdd;
        ArrangeItemsControl.ItemRemoving += OnItemRemoving;
        ArrangeItemsControl.ItemRemoved += OnItemRemoved;
        allItems = new List<ItemDataBindingEventArgs>();
        
    }

    private void OnItemRemoved(object sender, ItemRemovedEventArgs e)
    {

    }

    private void OnItemRemoving(object sender, CancelEventArgs e)
    {

    }

    private void OnItemsAdd(object sender, ItemAddEventArgs e)
    {

    }

    private void OnItemsArranged(object sender, EventArgs e)
    {
        
    }

    private void OnItemDataBinding(object sender, ItemDataBindingEventArgs e)
    {
        Computer dataItem = e.Item as Computer;
        if (dataItem != null)
        {
            TextMeshProUGUI tmp = e.ItemPresenter.GetComponentInChildren<TextMeshProUGUI>();
            
            tmp.text = dataItem.Name;

            Image image = e.ItemPresenter.GetComponentInChildren<Image>();
            e.CanDrag = false;
            Sprite sprite = Resources.Load<Sprite>("server");
            image.sprite = sprite;

            allItems.Add(e);


        }
    }
    public  void DragEnableDisable()
    {
       // Debug.Log("basıldı");
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




        AnimationsController.Instance.RecycleBinOpenCloseAnim(!scrollEnabled);
        AnimationsController.Instance.UpgradesOpenCloseAnim(false);

        foreach (ComputerController computerController in GameController.Instance.computerControllers)
        {
            computerController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = scrollEnabled;
        }
      
    }
}
