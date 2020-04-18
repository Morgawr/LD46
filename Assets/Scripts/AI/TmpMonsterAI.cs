using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpMonsterAI : EnemyAI 
{

    protected override void Start() {
        projectileFactory = GameObject.FindGameObjectsWithTag("ProjectileFactory")[0].GetComponent<ProjectileFactory>();
        RangedAttacks.Add(new AttackRoutine(BasicProjectileRoutine));
    }

    void BasicProjectileRoutine() {
        var toShoot = projectileFactory.SpawnProjectile(projectileFactory.BaseProjectile, Player.transform, this.transform);
        toShoot.Speed = 10;
        toShoot.Duration = 20;
        toShoot.gameObject.SetActive(true);
    }
}
