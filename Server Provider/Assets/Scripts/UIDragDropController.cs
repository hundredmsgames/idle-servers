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
        Server dataItem = e.Item as Server;
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
    public  void DragEnable()
    {
        bool scrollEnabledisable = false;
        foreach (ItemContainer item in GameContoller.Instance.ItemContainers)
        {
            item.CanDrag = !item.CanDrag;
            scrollEnabledisable = !item.CanDrag;
            Debug.Log(item.CanDrag);
        }
        scrollRect.vertical = scrollEnabledisable;
        if (scrollRect.vertical == false)
            dragEnabledScreen.GetComponent<Image>().color = Color.green;
        else
            dragEnabledScreen.GetComponent<Image>().color = Color.white;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
