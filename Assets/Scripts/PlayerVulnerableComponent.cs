using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVulnerableComponent : AbstractVulnerableComponent
{

    public Player Player;
    public override void GetDamaged(int value) {
        // Do nothing because player has no health
        // TODO: add stamina values

        var staggerList = GameObject.FindGameObjectsWithTag("Stagger");
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        if (Player.CurrentStag > 0 && !Player.isFlickering)
        {
            staggerList[Player.CurrentStag - 1].SetActive(false);
            Player.CurrentStag--;
        }
    }
}
