using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVulnerableComponent : AbstractVulnerableComponent
{
    Player player;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

    public override void GetDamaged(int value) {
        // Do nothing because player has no health
        // TODO: add stamina values
        Debug.Log(value);
        if(player.IsInBossBattle) {
            Debug.Log(value);
            player.CurrentLife -= value;
        }
    }
}
