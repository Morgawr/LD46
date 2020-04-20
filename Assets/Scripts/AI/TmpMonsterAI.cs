using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpMonsterAI : EnemyAI 
{

    protected override void Start() {
        base.Start();
        projectileFactory = GameObject.FindGameObjectsWithTag("ProjectileFactory")[0].GetComponent<ProjectileFactory>();
        RangedAttacks.Add(new Delegates.EmptyDel(BasicProjectileRoutine));
    }

    void BasicProjectileRoutine() {
        var toShoot = projectileFactory.SpawnProjectile(projectileFactory.BaseProjectile, Player.transform.position, this.transform);
        toShoot.Speed = 10;
        toShoot.Duration = 20;
        toShoot.gameObject.SetActive(true);
    }

    protected override void Patrol() {
        base.Patrol();
        GetComponent<Animator>().SetTrigger("Walking");
    }

    protected override void ApproachPlayer() {
        base.ApproachPlayer();
        GetComponent<Animator>().SetTrigger("Walking");
    }

    protected override void DoNothing() {
        base.DoNothing();
        GetComponent<Animator>().SetTrigger("Idle");
    }

}
