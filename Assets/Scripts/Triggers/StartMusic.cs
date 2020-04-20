using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : ProximityTriggerCollider.TriggerBehaviour
{
    public override void TriggerAction() {
        var Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        Player.MusicManager.StartNormalTracks();
    }
}
