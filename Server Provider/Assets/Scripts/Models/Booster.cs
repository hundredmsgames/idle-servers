using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Booster
{
    public static event Action<BoosterType, BoosterState> OnStateChanged = delegate { };
    //current level of booster
    private int level;
    //the amount of money we need to be able to upgrade
    private int nextUpgradePrice;
    //booster name for displaying
    private string name;
    //every boost has a timer for using
    protected float usingTime = 5;
    //every booster has its own cool down we can wait till it ends or we can watch an advertisement?
    protected float coolDown = 5;
    //every booster has a description for what it does
    private string description;
    private float currentCoolDown;
    private float currentUsingTime;

    private BoosterState state = BoosterState.Ready;
    private BoosterType boosterType;
    public int Level { get => level; protected set => level = value; }
    public string Name { get => name; protected set => name = value; }
    public string Description { get => description; protected set => description = value; }
    public BoosterState State { get => state; protected set { state = value; OnStateChanged(boosterType, state); } }
    public BoosterType BoosterType { get => boosterType; protected set { boosterType = value; } }

    public float CurrentCoolDown { get => currentCoolDown; set => currentCoolDown = value; }
    public float CurrentUsingTime { get => currentUsingTime; set => currentUsingTime = value; }

    public Booster(BoosterType _boosterType)
    {
        boosterType = _boosterType;
    }
    public virtual void Upgrade()
    {
        //update level
        level++;
        //calculate next upgrade price

        //calculate next time for using
    }
    public virtual void Use()
    {
        if (state != BoosterState.Ready)
        {
            Debug.LogError("How did it happen??? There should be no way of awaliable of using this");
            return;
        }
        State = BoosterState.Using;
        currentCoolDown = coolDown;
        currentUsingTime = usingTime;
    }

    //TODO: buraya bir daha bak
    public virtual void Update(float deltaTime)
    {
        if (state == BoosterState.Ready)
        {
            return;
        }
        else
        {
            if (currentCoolDown <= 0)
            {
                currentCoolDown = 0;
                State = BoosterState.Ready;
                return;
            }
            else
                currentCoolDown -= deltaTime;

            if (state != BoosterState.CoolDown)
            {
                if (currentUsingTime <= 0)
                {
                    currentUsingTime = 0;
                    State = BoosterState.CoolDown;
                }
                else
                {
                    currentUsingTime -= deltaTime;
                }
            }
        }

    }
    /// <summary>
    /// returns the value between 0-1
    /// </summary>
    /// <returns></returns>
    public float GetCoolDownAsPercent()
    {
        return (coolDown - currentCoolDown) / coolDown;
    }
    /// <summary>
    /// returns the value between 0-1
    /// </summary>
    /// <returns></returns>
    public float GetUsingTimeAsPercent()
    {
        return (usingTime - currentUsingTime) / usingTime;
    }

}
