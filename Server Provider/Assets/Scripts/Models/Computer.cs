using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Item
{

    public override void Upgrade()
    {
        currLevel++;
        mps += 1;
        requiredMoneyForUpgrade += (int)(requiredMoneyForUpgrade * 0.1f);

        if (currLevel > maxLevel)
        {
            // We need to notify the dictionary in the GameController that
            // we can setup a new server of this now.

            //setupNewServer = true;
        }

        base.Upgrade();
    }
    public override int Produce()
    {
        return mps;
    }

    //we need to tell to upgradeable & plantable objects to what to do
    //Plant || Upgrade
    //does server need to know how to do this? how are we gonna do it? figure it out?
}
