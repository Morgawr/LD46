using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableComponent : MonoBehaviour
{

    public Rigidbody2D body = null;

    public Player Player;

    bool isInAir = false;
    bool isJumpCooldown = false;

    Timer jumpTimer;

    void resetJumpCooldown() {
        isJumpCooldown = false;
    }

    public bool CanMove() {
        return !isInAir;
    }

    public void SignalInAir(bool start){
        isInAir = start;
    }

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        jumpTimer = new Timer();
    }

    void ClampVelocity() {
        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -Player.maxVelocity, Player.maxVelocity), body.velocity.y);
    }

    // Update is called once per frame
    void Update() {
        if(InputManager.IsPressed("left") && CanMove()) {
            body.AddForce(new Vector2(-1, 0) * Player.moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        else if (InputManager.IsPressed("right") && CanMove())
            body.AddForce(new Vector2(1, 0) * Player.moveSpeed * Time.deltaTime, ForceMode2D.Impulse);

        if(InputManager.IsPressed("jump") && CanMove() && !isJumpCooldown) {
            body.AddForce(new Vector2(0, 1) * Player.jumpStrength);
            isJumpCooldown = true;
            StartCoroutine(jumpTimer.Countdown(0.5f, new Timer.SideEffector(resetJumpCooldown)));
        }

        ClampVelocity();
    }
}
