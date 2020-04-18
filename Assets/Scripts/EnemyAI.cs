using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public ControllableComponent Player;

    public delegate void AttackRoutine();


    public AttackRoutine[] RangedAttacks;
    public AttackRoutine[] MeleeAttacks;

    public float MeleeRange = 0;
    public float AttackSpeed = 0;
    bool hasJustAttacked = false;
    bool isPlayerSpotted = false;

    Timer attackTimer = new Timer();

    void Start() {
        
    }

    void resetAttackCooldown() {
        hasJustAttacked = false;
    }

    float CalculateDistanceFromPlayer() {
        return Physics2D.Distance(Player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>()).distance;
    }

    protected virtual void Patrol() {
        // Patrol routine
        Debug.Log("patrolling");
    }

    protected virtual void MeleeAttack() {
        // Melee attack
        Debug.Log("melee attack");
    }
    
    protected virtual void RangedAttack() {
        // Ranged attack
        Debug.Log("ranged attack");
    }

    protected virtual void ApproachPlayer() {
        Debug.Log("Approaching player");
    }

    public virtual void PlayerSpotted(bool spotted) {
        isPlayerSpotted = spotted;
    }

    void Update() {
        if(!isPlayerSpotted) {
            Patrol();
        } else if(!hasJustAttacked) {
            if(CalculateDistanceFromPlayer() > MeleeRange) {
                RangedAttack();
            } else {
                MeleeAttack();
            }
            hasJustAttacked = true;
            StartCoroutine(attackTimer.Countdown(1f / AttackSpeed, new Timer.SideEffector(resetAttackCooldown)));
        } else {
            // TODO: Add different logic for melee vs non-melee enemy types
            ApproachPlayer();
        }
    }

    void OnDrawGizmosSelected () {
        Gizmos.color = new Color(1.0f, 0f, 0f, .2f);
        Gizmos.DrawSphere(transform.position, MeleeRange);
    }
}
