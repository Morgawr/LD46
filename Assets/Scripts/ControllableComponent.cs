using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableComponent : MonoBehaviour
{

    public Rigidbody2D body = null;
    public Collider2D avatarCollider = null;

    public Player Player;

    bool isInAir = false;
    bool isJumpCooldown = false;
    bool isWalltouching = false;

    bool isOnLadder = false;

    // Enemy or platform?
    bool isTouchingSomething = false;

    Timer jumpTimer;

    void resetJumpCooldown() {
        isJumpCooldown = false;
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

    void OnHurtWrapper() {
        var flicker = this.GetComponent<SpriteFlickerComponent>();
        flicker.StartFlicker();
        // Get thrown in a random direction 45 degrees
        float thrownForce = 100f;
        float y = 0.7f; 
        float x = Random.Range(0, 20) % 2 == 0 ? 1 : -1;

        this.body.AddForce(new Vector2(x, y) * thrownForce, ForceMode2D.Impulse);
        if(isOnLadder) {
            SignalIsClimbing(false);
        }
    }

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        jumpTimer = new Timer();

        var hurtComponent = this.GetComponent<HurtComponent>();
        hurtComponent.OnHurtReaction = new HurtComponent.OnHurtReactionDel(OnHurtWrapper);
    }

    // Update is called once per frame
    void Update() {

        if(!isWalltouching || CanMove()) {
            var actualMoveSpeed = (isInAir && !isOnLadder) ? Player.airSpeed : Player.moveSpeed;

            if(InputManager.IsPressed("left")) 
                body.AddForce(new Vector2(-1, 0) * actualMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            else if (InputManager.IsPressed("right"))
                body.AddForce(new Vector2(1, 0) * actualMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        if(InputManager.IsPressed("jump") && CanMove() && !isJumpCooldown) {
            body.AddForce(new Vector2(0, 1) * Player.jumpStrength);
            isJumpCooldown = true;
            this.SignalIsClimbing(false);
            StartCoroutine(jumpTimer.Countdown(0.5f, new Timer.SideEffector(resetJumpCooldown)));
        }

        if (InputManager.IsPressed("up") || InputManager.IsPressed("down")) {
            if(avatarCollider.IsTouchingLayers(LayerMask.GetMask("LadderLayer"))) {
                this.SignalIsClimbing(true);
            }
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
