using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{

    public ControllableComponent player;
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
    public string EnemyName;

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

    public Player PlayerLoreInfo;
    protected GameObject MessageBox;
    protected Text Lore;

    Timer messageTimer = new Timer();

    public void FlipX() {
        Debug.Log("Flipped");
        isFacingRight = !isFacingRight;
        var oldScale = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
    }

    void Awake(){
    }

    protected virtual void Start() {
        if(isBoss) {
            if(Player.GetInstance().HasDefeated(this.EnemyName)){
                GameObject.Destroy(this.gameObject);
                Debug.Log("We should die here");
            }
        }
        var hurtComponent = this.GetComponent<HurtComponent>();
        hurtComponent.OnHurtReaction = new Delegates.EmptyDel(OnHurtWrapper);
        var healthComponent = this.GetComponent<EnemyHealthComponent>();
        healthComponent.OnDeath = new Delegates.EmptyDel(OnDeath);
        var players = GameObject.FindGameObjectsWithTag("Avatar");
        foreach(var p in players) {
            if(p.scene == this.gameObject.scene) {
                this.player = p.GetComponent<ControllableComponent>();
            }
        }

        PlayerLoreInfo = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        MessageBox = PlayerLoreInfo.MessageBox;
        Lore = PlayerLoreInfo.Lore;
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

    void resetMessageCooldown()
    {
        if(MessageBox)
            MessageBox.SetActive(false);
    }

    protected float CalculateDistanceFromPlayer() {
        return Vector2.Distance(transform.position, player.transform.position);
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
        FollowPoint(player.transform, AggroSpeed);
    }

    protected virtual void BossPhaseAttack() {
        Debug.Log("Called phase attack on a boss with no unique AI.");
    }

    // We can override OnDeath in child classes for EnemyAI so we can have OnDeath
    // effects (like enemy explodes, etc)
    protected virtual void OnDeath() {
        if(isBoss) {
            SFXManager.GetInstance().PlayFX("BossWin");
            var bossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossRoomManager>();
            if(!bossManager.Bosses.Remove(this)){
                Debug.Log("We failed removing boss " + this.gameObject.name + " from BossManager.");
            }
            if (this.EnemyName != "SnailBoss2" && this.EnemyName != "WormBoss2") {
                // Add Extra life to player
                player.Player.MaxLife += 100;
                player.Player.CurrentLife = player.Player.MaxLife;
                player.Player.DefeatedBoss(this.EnemyName);
            }
            if(this.EnemyName == "SnailBoss") {
                player.ObtainDoubleJump();

                if (MessageBox)
                {
                    MessageBox.SetActive(true);
                    Lore.text = "You've unlocked double jump!";
                    player.Player.StartCoroutine(messageTimer.Countdown(3f, new Delegates.EmptyDel(resetMessageCooldown)));
                }
            }
        } else {
            SFXManager.GetInstance().PlayFX("CombatWin");
        }
        var mana = GetComponent<EnemyHealthComponent>().ManaReward;
        player.AcquireMana(mana);
        // Try to remove the enemy that might have been spotting us so we can
        // transition out of combat
        player.Player.RemoveSpotter(this);
        if(particleOnDeath != null) {
            particleOnDeath.transform.SetParent(this.transform.parent, true);
            particleOnDeath.gameObject.SetActive(true);
        }
        Destroy(this.patroller.gameObject);
        Destroy(this.gameObject);
    }

    public virtual void PlayerSpotted(bool spotted) {
        if(spotted) {
            player.Player.AddSpotter(this);
        } else { // We are not spotted anymore
            player.Player.RemoveSpotter(this);
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

    protected virtual void DoNothing() {

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
        } else {
            DoNothing();
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
