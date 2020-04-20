using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailBoss : EnemyAI {

    protected override void Start() {
        base.Start();
    }

    // We reuse this to dash forward
    protected override void ApproachPlayer() {
        this.body.AddForce(new Vector2(500, 0), ForceMode2D.Impulse);
    }

    protected override void RunAI() {
        ResolveApproach();
    }

    protected override void OnDeath() {
        var globalPlayer = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().IsInBossBattle = false;
        base.OnDeath();
    }
}
