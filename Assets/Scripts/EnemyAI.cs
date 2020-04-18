using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public ControllableComponent Player;
    public PatrolComponent patroller;
    public Rigidbody2D body;

    public delegate void AttackRoutine();

    public List<AttackRoutine> RangedAttacks = new List<AttackRoutine>();
    public List<AttackRoutine> MeleeAttacks = new List<AttackRoutine>();

    public float MeleeRange = 0;
    public float AttackSpeed = 0;
    public float ApproachChance = 0;
    public Vector2 MaxMovementSpeed;
    public float PatrolSpeed = 0;
    public float AggroSpeed = 0;
    public bool isFlying = false;

    bool isFacingRight = true;

    protected ProjectileFactory projectileFactory; 
    bool hasJustAttacked = false;
    bool isPlayerSpotted = false;
    bool hasTriedToApproach = false;
    bool isApproaching = false;

    Timer attackTimer = new Timer();
    Timer approachTimer = new Timer();

    public void FlipX() {
        isFacingRight = !isFacingRight;
        var oldScale = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
    }

    protected virtual void Start() {
        
    }

    AttackRoutine RandomAttackRoutine(List<AttackRoutine> attacks) {
        var randChoice = (int)Mathf.Floor(Random.Range(0f, attacks.Count));
        return attacks[randChoice];
    }

    void resetAttackCooldown() {
        hasJustAttacked = false;
    }

    void resetApproachCooldown() {
        hasTriedToApproach = false;
        isApproaching = false;
    }

    protected float CalculateDistanceFromPlayer() {
        return Vector2.Distance(transform.position, Player.transform.position);
    }

    void FollowPoint(Transform targetPoint, float speed) {
        if(this.transform.position.x < targetPoint.position.x && !isFacingRight) {
            FlipX();
        } else if(this.transform.position.x > targetPoint.position.x && isFacingRight) {
            FlipX();
        }

        Vector2 force = (targetPoint.position - transform.position).normalized * speed;
        if(!isFlying) {
            force.y = 0;
        }

        body.AddForce(force * Time.deltaTime);
    }

    protected virtual void Patrol() {
        if(patroller.ShouldGetNextPoint(this.transform)) {
            patroller.TriggerNextPoint();
        }
        FollowPoint(patroller.GetCurrentPoint(), PatrolSpeed);
    }

    protected virtual void MeleeAttack() {
        // Melee attack
        Debug.Log("melee attack");
    }
    
    protected virtual void RangedAttack() {
       RandomAttackRoutine(RangedAttacks)();
    }

    protected virtual void ApproachPlayer() {
        FollowPoint(Player.transform, AggroSpeed);
    }

    public virtual void PlayerSpotted(bool spotted) {
        isPlayerSpotted = spotted;
    }

    void RunAI() {
        if(!isPlayerSpotted) {
            Patrol();
        } else if(!hasJustAttacked) {
            isApproaching = false;
            if(CalculateDistanceFromPlayer() > MeleeRange) {
                RangedAttack();
            } else {
                MeleeAttack();
            }
            hasJustAttacked = true;
            StartCoroutine(attackTimer.Countdown(1f / AttackSpeed, new Timer.SideEffector(resetAttackCooldown)));
        } else {
            if(RangedAttacks.Count == 0) {
                ApproachPlayer();
                return;
            } 
            // TODO : have melee attack
            //if(MeleeAttacks.Count == 0) {
            //    // We do nothing because we want to stay away from the player
            //    // TODO: Maybe have logic to run away from player
            //    return;
            //}
            // Try to approach the player if you can
            if(!hasTriedToApproach) {
                var chance = Random.Range(0f, 1f);
                if (chance < ApproachChance) {
                    // Every half second try to see if you can approach the player or not
                    isApproaching = true;
                    StartCoroutine(approachTimer.Countdown(0.5f, new Timer.SideEffector(resetApproachCooldown)));
                }
            }
            if(isApproaching) {
                ApproachPlayer();
            }
        }
    }

    void Update() {
        RunAI();
        body.velocity = VelocityClamper.ClampVelocity(body.velocity, MaxMovementSpeed);
    }

    void OnDrawGizmosSelected () {
        Gizmos.color = new Color(1.0f, 0f, 0f, .2f);
        Gizmos.DrawSphere(transform.position, MeleeRange);
    }
}
