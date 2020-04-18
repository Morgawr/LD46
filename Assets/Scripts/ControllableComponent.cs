using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableComponent : MonoBehaviour
{

    public Rigidbody2D body = null;
    public Collider2D avatarCollider = null;
    public SlashComponent sideAttack = null;
    public SlashComponent downAttack = null;
    public SlashComponent upAttack = null;

    public Player Player;

    bool isInAir = false;
    bool isJumpCooldown = false;
    bool isWalltouching = false;
    bool isAttackCooldown = false;

    bool isOnLadder = false;
    bool isOnInteractable = false;

    bool isFacingRight = true;

    // Enemy or platform?
    bool isTouchingSomething = false;

    Timer jumpTimer;
    Timer attackTimer;

    void resetJumpCooldown() {
        isJumpCooldown = false;
    }

    void resetAttackCooldown() {
        isAttackCooldown = false;
    }

    public bool CanMove() {
        return !isInAir || isOnLadder;
    }

    public void SignalInAir(bool start){
        isInAir = start;
    }

    public void SignalIsWalltouching(bool start) {
        isWalltouching = start;
    }

    public void SignalIsClimbing(bool start) {
        if(start) {
            body.gravityScale = 0;
            body.velocity = new Vector2(0, 0);
        } else {
            body.gravityScale = 1;
        }
        isOnLadder = start;
    }

    public void SignalIsOnInteractable(bool start)
    {
        isOnInteractable = start;
    }

    void OnHurtWrapper() {
        var flicker = this.GetComponent<SpriteFlickerComponent>();
        flicker.StartFlicker();
        // Get thrown in a random direction 45 degrees
        float thrownForce = 100f;
        float y = 0.7f; 
        float x = Random.Range(0, 20) % 2 == 0 ? 1 : -1;

        this.body.AddForce(new Vector2(x, y) * thrownForce, ForceMode2D.Impulse);
        SignalIsClimbing(false);
    }

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        jumpTimer = new Timer();
        attackTimer = new Timer();

        var hurtComponent = this.GetComponent<HurtComponent>();
        hurtComponent.OnHurtReaction = new Delegates.EmptyDel(OnHurtWrapper);

        // When we hit something with a down attack, we are launched upwards
        downAttack.AttackHitCallback = new Delegates.EmptyDel(KnockbackOnDownAttack);
    }

    void FlipPlayer() {
        isFacingRight = !isFacingRight;
        var oldScale = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
    }

    void KnockbackOnDownAttack() {
        // Reset downfall velocity
        body.velocity = new Vector2(0, 0);
        body.AddForce(new Vector2(0f, 1f) * Player.downAttackKnockbackStrength, ForceMode2D.Impulse);
    }

    void HandleAttackLogic() {
        bool haveAttacked = false;
        // Attack down mid-air
        if (isInAir && InputManager.IsPressed("down") && !downAttack.isActiveAndEnabled) {
            downAttack.Slash(5);
            haveAttacked = true;
        }
        else if(InputManager.IsPressed("up") && !upAttack.isActiveAndEnabled) {
            upAttack.Slash(5);
            haveAttacked = true;
        }
        else if (!sideAttack.isActiveAndEnabled) { // Attack side
            sideAttack.Slash(5);
            haveAttacked = true;
        }
        if (haveAttacked) {
            isAttackCooldown = true;
            StartCoroutine(attackTimer.Countdown(1 / Player.attackSpeed, new Delegates.EmptyDel(resetAttackCooldown)));
        }
    }

    // Update is called once per frame
    void Update() {

        if(!isWalltouching || CanMove()) {
            var actualMoveSpeed = (isInAir && !isOnLadder) ? Player.airSpeed : Player.moveSpeed;

            if(InputManager.IsPressed("left"))  {
                if(isFacingRight) 
                    FlipPlayer();
                body.AddForce(new Vector2(-1, 0) * actualMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }
            else if (InputManager.IsPressed("right")) {
                if(!isFacingRight)
                    FlipPlayer();
                body.AddForce(new Vector2(1, 0) * actualMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }
        }

        if(InputManager.IsPressed("jump") && CanMove() && !isJumpCooldown) {
            body.AddForce(new Vector2(0, 1) * Player.jumpStrength);
            isJumpCooldown = true;
            this.SignalIsClimbing(false);
            StartCoroutine(jumpTimer.Countdown(0.5f, new Delegates.EmptyDel(resetJumpCooldown)));
        }

        if (InputManager.IsPressed("up") || InputManager.IsPressed("down")) {
            if(avatarCollider.IsTouchingLayers(LayerMask.GetMask("LadderLayer"))) {
                this.SignalIsClimbing(true);
            }
            // TODO: Add portal exiting/interaction
        }

        if(isOnLadder) {
            var climbForce = new Vector2(0, 0);
            if(InputManager.IsPressed("up")) {
                climbForce = new Vector2(0, 1);
            }
            if(InputManager.IsPressed("down") && isInAir && !isTouchingSomething) {
                climbForce = new Vector2(0, -1);
            }
            transform.Translate(climbForce * Player.climbSpeed * Time.deltaTime);
        } else {
            // We should be able to attack here
            if(InputManager.IsPressed("attack") && !isAttackCooldown) {
                HandleAttackLogic();
            }
        }

        if(isOnInteractable)
        {
            if (InputManager.IsPressed("up"))
            {
                Debug.Log("Trigger portal interaction");
            }
        }

        body.velocity = VelocityClamper.ClampVelocity(body.velocity, Player.maxVelocity);
    }

    void OnCollisionEnter2D(Collision2D other) {
        isTouchingSomething = true;
    }

    void OnCollisionExit2D(Collision2D other) {
        isTouchingSomething = false;
    }
}
