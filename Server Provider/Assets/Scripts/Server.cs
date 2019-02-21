using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server 
{
    int level;
    int requiredLevel;
    int mps;//money per second
    int ramSize;

    float cpuSpeed;
    bool active=false;
    string spriteName;
    int levelStart = 1;
    int maxLevel=10;
    bool setupNewServer = true;
    bool featurePointActive=true;
    int currentFeaturePoints = 3;

    public delegate void LevelUpHandler(int level);
    public event LevelUpHandler LeveledUp;

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
        ramSize+=2;
        //production level changes so recalculate mps(money per second)
    }
    public void AddCpuSpeed()
    {
        cpuSpeed += 0.1f;
        //production level changes so recalculate mps(money per second)
    }
  
    
}
