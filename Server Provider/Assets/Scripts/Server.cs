using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server
{
    //deneme
    public string Name { get; set; }

    int level;
    int requiredLevel;
    int mps;//money per second
    int ramSize;

    float cpuSpeed;
    bool active = false;
    public string spriteName;
    int levelStart = 1;
    int maxLevel = 10;
    bool setupNewServer = true;
    bool featurePointActive = true;
    int currentFeaturePoints = 3;

    public bool plantable = false;
    public bool upgradeable = false;
    public delegate void LevelUpHandler(int level);
    public event LevelUpHandler LeveledUp;

    public delegate void ServerEventsHandler(Server server);
    public event ServerEventsHandler Plant;
    public event ServerEventsHandler Planted;
    public event ServerEventsHandler Upgraded;

    //every second this server produces money
    public int ProduceMoney()
    {
        return mps;
    }

    public void LevelUp()
    {
        level++;
        if (level >= maxLevel)
            setupNewServer = true;
        else
        {
            currentFeaturePoints += 3;
            featurePointActive = true;

            LeveledUp(level);
        }
        //when we reach to the max then we can plant(setup) new server and reference to it
        //figure it out

        //recalculate mps(money per second)
    }
    public void AddRam()
    {
        ramSize += 2;
        //production level changes so recalculate mps(money per second)
    }
    public void AddCpuSpeed()
    {
        cpuSpeed += 0.1f;
        //production level changes so recalculate mps(money per second)
    }
    
    public void UpgradeServer()
    {
        if (Upgraded != null)
            Upgraded(this);

    }
    public void PlantServer()
    {
        if (Plant != null)
            Plant(this);
    }
    public void PlantedServer()
    {
        if (Planted != null)
            Planted(this);
    }
    //we need to tell to upgradeable & plantable objects to what to do
    //Plant || Upgrade
    //does server need to know how to do this? how are we gonna do it? figure it out?

}
