using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server
{
    // Name of the server
    public string Name { get; set; }
    
    // Current level of the server.
    public int serverlevel = 1;

    // To make this server plantable game level should be
    // equal or above the requiredLevel.
    public int requiredLevel;

    // Produced money per second for this server.
    public int mps;

    // Required money to make this server upgradable.
    public int requiredMoneyForUpgrade;

    // Maximum level that server can reach.
    int maxLevel = 10;

    public delegate void LevelUpHandler(int level);
    public event LevelUpHandler LeveledUp;

    public delegate void ServerEventsHandler(Server server);
    public event ServerEventsHandler Plant;
    public event ServerEventsHandler Planted;
    public event ServerEventsHandler Upgraded;
    public event ServerEventsHandler UpdateEvent;

    public Server Copy()
    {
        Server copy = new Server() {
            mps = this.mps,
            serverlevel = this.serverlevel,
            maxLevel = this.maxLevel,
            Name = this.Name,
            requiredLevel = this.requiredLevel,
            requiredMoneyForUpgrade = this.requiredMoneyForUpgrade,
            
            // Events
            UpdateEvent = this.UpdateEvent,
            Upgraded = this.Upgraded,
            Planted = this.Planted,
            Plant = this.Plant,
            LeveledUp = this.LeveledUp
        };

        return copy;
    }

    // every second this server produces money
    public int ProduceMoney()
    {
        return mps;
    }

    public void LevelUp()
    {
        serverlevel++;
        if (serverlevel >= maxLevel)
        {
            // We need to notify the dictionary in the GameController that
            // we can setup a new server of this now.

            //setupNewServer = true;
        }
        else
        {
            LeveledUp(serverlevel);
        }
       
        //when we reach to the max then we can plant(setup) new server and reference to it
        //figure it out

        //recalculate mps(money per second)
    }

    public void UpgradeServer()
    {
        serverlevel++;
        mps += 1;
        requiredMoneyForUpgrade += (int)(requiredMoneyForUpgrade * 0.1f);
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
