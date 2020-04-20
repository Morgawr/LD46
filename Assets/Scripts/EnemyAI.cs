using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public ControllableComponent Player;
    public PatrolComponent patroller;
    public Rigidbody2D body;
    public EnemyHealthComponent health;
    // If this is null then we don't use particles when we die
    public ParticleSystem particleOnDeath; 

    public List<Delegates.EmptyDel> RangedAttacks = new List<Delegates.EmptyDel>();
    public List<Delegates.EmptyDel> MeleeAttacks = new List<Delegates.EmptyDel>();

    public float MeleeRange = 0;
    public float AttackSpeed = 0;
    public float ApproachChance = 0;
    // This is used for bosses to do alternative phases and normal enemies
    // to check if they can approach the player or not.
    public float ApproachAttemptsPerSecond = 0;
    public Vector2 MaxMovementSpeed;
    public float PatrolSpeed = 0;
    public float AggroSpeed = 0;
    public bool isFlying = false;

    // HACK: This is to check if we have a special attack pattern logic
    // instead of using the default enemy logic.
    public bool isBoss = false;

    bool isFacingRight = true;

    protected ProjectileFactory projectileFactory;
    protected bool hasJustAttacked = false;
    bool isPlayerSpotted = false;
    protected bool hasTriedToApproach = false;
    protected bool isApproaching = false;

    Timer attackTimer = new Timer();
    Timer approachTimer = new Timer();

    public void FlipX() {
        Debug.Log("Flipped");
        isFacingRight = !isFacingRight;
        var oldScale = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
    }

    protected virtual void Start() {
        var hurtComponent = this.GetComponent<HurtComponent>();
        hurtComponent.OnHurtReaction = new Delegates.EmptyDel(OnHurtWrapper);
        var healthComponent = this.GetComponent<EnemyHealthComponent>();
        healthComponent.OnDeath = new Delegates.EmptyDel(OnDeath);
    }

    Delegates.EmptyDel RandomAttackRoutine(List<Delegates.EmptyDel> attacks) {
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

    protected void FollowPoint(Transform targetPoint, float speed) {
        if(this.transform.position.x < targetPoint.position.x && !isFacingRight) {
            FlipX();
        } else if(this.transform.position.x > targetPoint.position.x && isFacingRight) {
            FlipX();
        }

        Vector2 force = (targetPoint.position - transform.position).normalized * speed;

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

    protected virtual void BossPhaseAttack() {
        Debug.Log("Called phase attack on a boss with no unique AI.");
    }

    // We can override OnDeath in child classes for EnemyAI so we can have OnDeath
    // effects (like enemy explodes, etc)
    protected virtual void OnDeath() {
        var mana = GetComponent<EnemyHealthComponent>().ManaReward;
        Player.AcquireMana(mana);
        // Try to remove the enemy that might have been spotting us so we can
        // transition out of combat
        Player.Player.RemoveSpotter(this);
        if(particleOnDeath != null) {
            particleOnDeath.transform.SetParent(this.transform.parent, true);
            particleOnDeath.gameObject.SetActive(true);
        }
        Destroy(this.patroller.gameObject);
        Destroy(this.gameObject);
    }

    public virtual void PlayerSpotted(bool spotted) {
        if(spotted) {
            Player.Player.AddSpotter(this);
        } else { // We are not spotted anymore
            Player.Player.RemoveSpotter(this);
        }
        isPlayerSpotted = spotted;
    }

    void OnHurtWrapper() {
        // TODO: Stagger the enemy
        var flicker = this.GetComponent<SpriteFlickerComponent>();
        flicker.StartFlicker();
    }

    protected void TriggerAttack() {
        hasJustAttacked = true;
        StartCoroutine(attackTimer.Countdown(1f / AttackSpeed, new Delegates.EmptyDel(resetAttackCooldown)));
    }

    protected void ResolveApproach() {
        if (!hasTriedToApproach) {
            var chance = Random.Range(0f, 1f);
            if (chance < ApproachChance) {
                isApproaching = true;
            }
            hasTriedToApproach = true;
            StartCoroutine(approachTimer.Countdown(1/ApproachAttemptsPerSecond, new Delegates.EmptyDel(resetApproachCooldown)));
        }
        if(isApproaching) {
            ApproachPlayer();
        }
    }

    // This AI logic can be overridden by child classes (bosses, etc)
    protected virtual void RunAI() {
        if(!isPlayerSpotted) {
            Patrol();
        } else if(!hasJustAttacked) {
            isApproaching = false;
            if(CalculateDistanceFromPlayer() > MeleeRange) {
                RangedAttack();
            } else {
                MeleeAttack();
            }
            TriggerAttack();
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
            ResolveApproach();
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
