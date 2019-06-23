using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer
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

    // Maximum level that server can reach.
    int maxLevel = 10;

    public delegate void ComputerEventsHandler(Computer server);
    public event ComputerEventsHandler Plant;
    public event ComputerEventsHandler Planted;
    public event ComputerEventsHandler Upgraded;
    public event ComputerEventsHandler UpdateEvent;

    public Computer Copy()
    {
        Computer copy = new Computer() {
            Name = this.Name,
            mps = this.mps,
            currLevel = this.currLevel,
            maxLevel = this.maxLevel,
            requiredLevel = this.requiredLevel,
            requiredMoneyForUpgrade = this.requiredMoneyForUpgrade,
            
            // Events
            UpdateEvent = this.UpdateEvent,
            Upgraded = this.Upgraded,
            Planted = this.Planted,
            Plant = this.Plant
        };

        return copy;
    }

    // every second this server produces money
    public int ProduceMoney()
    {
        return mps;
    }

    public void UpgradeServer()
    {
        currLevel++;
        mps += 1;
        requiredMoneyForUpgrade += (int)(requiredMoneyForUpgrade * 0.1f);

        if (currLevel >= maxLevel)
        {
            // We need to notify the dictionary in the GameController that
            // we can setup a new server of this now.

            //setupNewServer = true;
        }

        Upgraded?.Invoke(this);
    }

    public void PlantServer()
    {
        Plant?.Invoke(this);
    }

    public void PlantedServer()
    {
        Planted?.Invoke(this);
    }

    public void Update()
    {
        UpdateEvent?.Invoke(this);
    }

    //we need to tell to upgradeable & plantable objects to what to do
    //Plant || Upgrade
    //does server need to know how to do this? how are we gonna do it? figure it out?
}
