using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ICloneable
{
    // Name of the server
    public string Name { get; set; }

    // Current level of the server.
    public int currLevel = 1;

    // To make this server plantable game level should be
    // equal or above the requiredLevel.
    public int requiredLevel;

    // Produced money per second for this server.
    public int mps;

    // Required money to make this server upgradable.
    public int requiredMoneyForUpgrade;

    public int requiredMoneyForPlant;

    // (Maximum level that item can reach) - 1.
    protected int maxLevel = 4;




    public delegate void ItemEventsHandler(Item item);

    public event ItemEventsHandler Planted;
    public event ItemEventsHandler Upgraded;
    public event ItemEventsHandler UpdateEvent;





    // every second this item produces somthing
    public abstract int Produce();
    public virtual void Upgrade()
    {
        Upgraded?.Invoke(this);
    }

    public virtual void ItemPlanted()
    {
        Planted?.Invoke(this);
    }

    public virtual void Update()
    {
        UpdateEvent?.Invoke(this);
    }

    public void ApplyBoost(int boostAmount)
    {
        mps += boostAmount;
    }
    //we need to tell to upgradeable & plantable objects to what to do
    //Plant || Upgrade
    //does server need to know how to do this? how are we gonna do it? figure it out?
    public object Clone()
    {
        return MemberwiseClone();
    }
}
