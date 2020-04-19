using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpBossAI : EnemyAI {

    protected override void Start() {
        base.Start();
        projectileFactory = GameObject.FindGameObjectsWithTag("ProjectileFactory")[0].GetComponent<ProjectileFactory>();
        RangedAttacks.Add(new Delegates.EmptyDel(ShootProjectiles));
        RangedAttacks.Add(new Delegates.EmptyDel(ShootLargerProjectile));
    }

    // TODO: Add this to ranged attack list
    void ShootProjectiles() {
        List<ProjectileComponent> projectiles = new List<ProjectileComponent>();

        int spread = 2;
        
        // Face level
        var toShoot = projectileFactory.SpawnProjectile(projectileFactory.BaseProjectile, Player.transform.position, this.transform);
        toShoot.Speed = 15;
        toShoot.Duration = 5;
        projectiles.Add(toShoot);

        // Above level
        toShoot = projectileFactory.SpawnProjectile(projectileFactory.BaseProjectile, new Vector2(Player.transform.position.x, Player.transform.position.y + spread), this.transform);
        toShoot.Speed = 15;
        toShoot.Duration = 5;
        projectiles.Add(toShoot);

        // Below level
        toShoot = projectileFactory.SpawnProjectile(projectileFactory.BaseProjectile, new Vector2(Player.transform.position.x, Player.transform.position.y - spread), this.transform);
        toShoot.Speed = 15;
        toShoot.Duration = 5;
        projectiles.Add(toShoot);

        foreach(var proj in projectiles)
            proj.gameObject.SetActive(true);
    }

    void ShootLargerProjectile() {
        var toShoot = projectileFactory.SpawnProjectile(projectileFactory.LargerProjectile, Player.transform.position, this.transform);
        toShoot.gameObject.SetActive(true);
    }

    protected override void RunAI() {
        FollowPoint(Player.transform, 0);
        //Phase 1
        if(health.HP >= health.MaxHP / 2) {
            // Chance to teleport
            // Chance to shoot towards player
            if(!hasJustAttacked) {
                RangedAttack();
                TriggerAttack();
            }
            // Chance to jump
        } else {
        //Phase 2

        }
    }

    protected override void OnDeath() {
        var globalPlayer = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().IsInBossBattle = false;
        base.OnDeath();
    }
}
