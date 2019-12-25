using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Booster
{
    //current level of booster
    protected int level;
    //the amount of money we need to be able to upgrade
    protected int nextUpgradePrice;
    //booster name for displaying
    protected string name;
    //every boost has a timer for using
    protected float usingTime;
    //every booster has its own cool down we can wait till it ends or we can watch an advertisement?
    protected float coolDown;
    //every booster has a description for what it does
    protected string description;

    public virtual void Upgrade()
    {
        //update level
        level++;
        //calculate next upgrade price

        //calculate next time for using
    }
    public abstract void Use();

}
