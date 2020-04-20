using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailBoss : EnemyAI {

    public bool isEnraged;

    protected override void Start() {
        base.Start();
    }

    protected override void Patrol() {
        if(patroller.ShouldGetNextPoint(this.transform)) {
            patroller.TriggerNextPoint();
        }
        if(isEnraged) {
            FollowPoint(patroller.GetCurrentPoint(), AggroSpeed);
        } else {
            FollowPoint(patroller.GetCurrentPoint(), PatrolSpeed);
        }
    }

    // We reuse this to dash forward
    protected override void ApproachPlayer() {
        Patrol();
    }

    protected override void RunAI() {
        if(health.HP < health.MaxHP / 2 && !isEnraged) 
            isEnraged = true;
        ResolveApproach();
    }

    protected override void OnDeath() {
        var globalPlayer = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().IsInBossBattle = false;
        base.OnDeath();
    }
}
