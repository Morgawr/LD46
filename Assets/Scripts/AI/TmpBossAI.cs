using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpBossAI : EnemyAI {

    protected bool isPhase2 = false;

    protected override void Start() {
        base.Start();
        projectileFactory = GameObject.FindGameObjectsWithTag("ProjectileFactory")[0].GetComponent<ProjectileFactory>();
        RangedAttacks.Add(new Delegates.EmptyDel(ShootProjectiles));
        RangedAttacks.Add(new Delegates.EmptyDel(ShootLargerProjectile));
    }

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

    // For this boss instead of approaching we teleport around
    protected override void ApproachPlayer() {
        // TODO: we need to set an animation timer here
        // true = Random trigger
        patroller.TriggerNextPoint(true);
        transform.position = patroller.GetCurrentPoint().position;
        // We need to reset this to false here because otherwise it will keep
        // looping forever.
        isApproaching = false;
    }

    protected override void RunAI() {
        FollowPoint(Player.transform, 0);
        // Enter phase 2 (change parameters in this case) if we haven't yet
        if(health.HP < health.MaxHP / 2 && !isPhase2) {
            ApproachChance *= 2; // teleport around twice as likely
            AttackSpeed *= 2; // Attack twice as fast
            isPhase2 = true;
        }

        // Chance to teleport
        // Chance to shoot towards player
        if (!hasJustAttacked) {
            RangedAttack();
            TriggerAttack();
        } else {
            ResolveApproach();
        }
    }

    protected override void OnDeath() {
        var globalPlayer = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().IsInBossBattle = false;
        base.OnDeath();
    }
}
