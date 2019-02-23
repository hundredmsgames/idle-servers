using ControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDragDropController : MonoBehaviour
{
    private int m_index;

    [SerializeField] ArrangeItemsControl ArrangeItemsControl;
    // Start is called before the first frame update
    void Start()
    {
        ArrangeItemsControl.ItemDataBinding += OnItemDataBinding;
        ArrangeItemsControl.ItemsArranged += OnItemsArranged;
        ArrangeItemsControl.ItemAdd += OnItemsAdd;
        ArrangeItemsControl.ItemRemoving += OnItemRemoving;
        ArrangeItemsControl.ItemRemoved += OnItemRemoved;


        IList items = new ArrayList();
        for (int i = 0; i < 38; i++)
            items.Add(new Server());




        ArrangeItemsControl.Items = items;
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
            Image image = e.ItemPresenter.GetComponentInChildren<Image>();

            Sprite sprite = Resources.Load<Sprite>("server");
            image.sprite = sprite;


        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
