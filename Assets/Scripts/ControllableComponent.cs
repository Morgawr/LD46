using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllableComponent : MonoBehaviour
{

    public Rigidbody2D body = null;
    public Collider2D avatarCollider = null;
    public SlashComponent sideAttack = null;
    public SlashComponent downAttack = null;
    public SlashComponent upAttack = null;
    public Animator PlayerAnimator;

    public Player Player;
    public SFXManager SFXManager;

    bool isInAir = false;
    bool isJumpCooldown = false;
    bool isWalltouching = false;
    bool isAttackCooldown = false;
    bool isSideAttack = false;
    bool isUpAttack = false;
    bool isDownAttack = false;
    bool isInAttackAnimation = false;

    bool isOnLadder = false;
    Interactable OnInteractable = null;

    bool isFacingRight = true;

    // Enemy or platform?
    bool isTouchingSomething = false;

    int jumpCounter = 0;

    Timer jumpTimer = new Timer();
    Timer attackTimer = new Timer();
    Timer interactTimer = new Timer();
    Timer flickerTimer = new Timer();

    void resetJumpCooldown() {
        isJumpCooldown = false;
    }

    void resetAttackCooldown() {
        isAttackCooldown = false;
    }

    void resetFlickerCooldown()
    {
        Player.isFlickering = false;
    }

    public void RefillJumps() {
        if(isJumpCooldown) {
            return; // Can't refill on a cooldown
        }
        jumpCounter = 1;
        if(Player.HasDoubleJump) {
            jumpCounter++;
        }
    }

    public bool CanMove() {
        return !isInAir || isOnLadder && !Player.isExhausted;
    }

    public void SignalInAir(bool start){
        if(!start)
            RefillJumps();
        isInAir = start;
    }

    public void SignalIsWalltouching(bool start) {
        isWalltouching = start;
    }

    public void SignalIsClimbing(bool start) {
        if(start) {
            body.gravityScale = 0;
            body.velocity = new Vector2(0, 0);
            RefillJumps();
        } else {
            body.gravityScale = 1;
        }
        isOnLadder = start;
    }

    public void SignalIsOnInteractable(Interactable element)
    {
        OnInteractable = element;
    }

    public void SetRespawn(string sceneName, string respawnName)
    {
        if(Player.respawnSceneName != sceneName || Player.respawnName != respawnName) {
            Player.respawnSceneName = sceneName;
            Player.respawnName = respawnName;
        }
    }

    public void Respawn()
    {
        var respawnList = GameObject.FindGameObjectsWithTag("Respawn");

        foreach (var respawn in respawnList)
        {
            var checkpoint = respawn.GetComponent<Checkpoint>();

            // HACK: this is a hack, sometimes the player variable is null when
            // respawning so it's just faster to fetch it again.
            if(Player == null) {
                Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            }
            if (checkpoint.GetTagName() == Player.respawnName) {
                this.transform.position = respawn.transform.position;
                return;
            }
        }
        // TODO: Have an exception here and have a default checkpoint to 
        // respawn when the game begins
        this.transform.position = Vector3.zero;
    }

    void OnHurtWrapper() {

        // If we are already hit, don't get hit again
        if(Player.isFlickering)  {
            return;
        }

        Player.isFlickering = true;

        var flicker = this.GetComponent<SpriteFlickerComponent>();
        flicker.StartFlicker();
        // Get thrown in a random direction 45 degrees
        float thrownForce = 100f;
        float y = 0.7f; 
        float x = Random.Range(0, 20) % 2 == 0 ? 1 : -1;

        this.body.AddForce(new Vector2(x, y) * thrownForce, ForceMode2D.Impulse);
        SignalIsClimbing(false);

        StartCoroutine(flickerTimer.Countdown(0.5f, new Delegates.EmptyDel(resetFlickerCooldown)));
    }

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        SFXManager = SFXManager.GetInstance();

        var hurtComponent = this.GetComponent<HurtComponent>();
        hurtComponent.OnHurtReaction = new Delegates.EmptyDel(OnHurtWrapper);

        this.GetComponent<AbstractVulnerableComponent>().OnDeath = new Delegates.EmptyDel(OnDeath);

        // When we hit something with a down attack, we are launched upwards
        downAttack.AttackHitCallback = new Delegates.EmptyDel(KnockbackOnDownAttack);
        RefillJumps();
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
            isDownAttack = true;
        }
        else if(InputManager.IsPressed("up") && !upAttack.isActiveAndEnabled) {
            upAttack.Slash(5);
            haveAttacked = true;
            isUpAttack = true;
        }
        else if (!sideAttack.isActiveAndEnabled) { // Attack side
            sideAttack.Slash(5);
            haveAttacked = true;
            isSideAttack = true;
        }
        if (haveAttacked) {
            SFXManager.PlayFX("MCAttack");
            isAttackCooldown = true;
            StartCoroutine(attackTimer.Countdown(1 / Player.attackSpeed, new Delegates.EmptyDel(resetAttackCooldown)));
        }
    }


    void HandleAnimationSet() {

        if(Player.isExhausted) { // Extra case for idle
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", true);
            return;
        }

        // Wait for attack animation to end before we try another animation
        if(isInAttackAnimation && !Player.isExhausted) 
            return;

        if(isSideAttack) { // Side attack
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", true);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", false);
            isInAttackAnimation = true;
            return;
        }

        if(isUpAttack) { // Up attack
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", true);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", false);
            isInAttackAnimation = true;
            return;
        }

        if(isDownAttack) { // Down attack
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", true);
            PlayerAnimator.SetBool("Staggered", false);
            isInAttackAnimation = true;
            return;
        }

        // This is very messy we have to be careful in manually toggling all the
        // right boolean states for every type of animation but it's the best
        // we can do in this jam
        if(!isInAir && !isOnLadder && Mathf.Abs(body.velocity.x) > 0.2) { // Running
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", true);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", false);
        }

        if(!isInAir && !isOnLadder && Mathf.Abs(body.velocity.x) < 0.2) { // Idle
            PlayerAnimator.SetBool("Idle", true);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", false);
        }

        if(isInAir && !isOnLadder) { // Jump
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", true);
            PlayerAnimator.SetBool("Climb", false);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", false);
        }
  
        if(isOnLadder) { // Climb
            PlayerAnimator.SetBool("Idle", false);
            PlayerAnimator.SetBool("Running", false);
            PlayerAnimator.SetBool("Jump", false);
            PlayerAnimator.SetBool("Climb", true);
            PlayerAnimator.SetBool("SideAttack", false);
            PlayerAnimator.SetBool("UpAttack", false);
            PlayerAnimator.SetBool("DownAttack", false);
            PlayerAnimator.SetBool("Staggered", false);
        }

    }

    void DoPause() {
        Debug.Log("Pause called");
        Player.IsGamePaused = true;
        Player.StartTransitionEvent("Pause", .2f);
        //SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("CreditsScene", LoadSceneMode.Additive);
    }

    void LateUpdate() {
        if (Player.IsGamePaused ||
            (!Player.IsTransitionDone("Unpause") && Player.DoesTransitionExist("Unpause")))
            return;

        if(InputManager.IsPressed("pause")) { // Pause the game here
            DoPause();
            return;
        }
    }

    // Update is called once per frame
    void Update() {
        if(Player.IsGamePaused)
            return;

        HandleAnimationSet();
        if (Player.isExhausted)
            return;

        if(!isWalltouching || CanMove()) {
            var actualMoveSpeed = (isInAir && !isOnLadder) ? Player.airSpeed : Player.moveSpeed;


            // HACK: This is terrible code
            if((InputManager.IsPressed("left") || InputManager.IsPressed("right")) && !Player.isExhausted) { // We are walking
                if(!SFXManager.IsFXPlaying("Steps") && !isInAir)
                    SFXManager.PlayFX("Steps", true); // Want to loop
                if(SFXManager.IsFXPlaying("Steps") && isInAir)
                    SFXManager.StopFX("Steps");
            } else if(SFXManager.IsFXPlaying("Steps")) {
                SFXManager.StopFX("Steps");
            }

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

        if(InputManager.IsPressed("jump") && !Player.isExhausted && !isJumpCooldown && jumpCounter > 0) {
            SFXManager.PlayFX("Jump");
            body.AddForce(new Vector2(0, 1) * Player.jumpStrength);
            isJumpCooldown = true;
            jumpCounter--;
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

        if(OnInteractable) {

            if (InputManager.IsPressed("up") && !Player.isInteractCooldown) {
                Player.isInteractCooldown = true;
                Player.StartCoroutine(interactTimer.Countdown(0.5f, new Delegates.EmptyDel(Player.resetInteractCooldown)));
                OnInteractable.Interact();
                if(!SFXManager.IsFXPlaying("Interaction"))
                    SFXManager.PlayFX("Interaction"); // TODO: Maybe we want to have a different sound for the mana refill station
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

    // This is what happens when the player dies
    public void OnDeath() {
        // Destroy and reload current scene
        Player.WeDiedAndWeAreRespawning = true;
        SFXManager.PlayFX("Staggered");
        SceneManager.UnloadSceneAsync(this.gameObject.scene.name);
        var respawnScene = Player.respawnSceneName;
        // TODO: This should be a crash/exception
        if(respawnScene == null)
            respawnScene = this.gameObject.scene.name;
        Player.IsInBossBattle = false;
        SceneManager.LoadSceneAsync(respawnScene, LoadSceneMode.Additive);
    }

    public void OnAttackAnimationEnd(SlashComponent attack) {
        Debug.Log("Called On End " + attack.gameObject.name);
        isSideAttack = false;
        isUpAttack = false;
        isDownAttack = false;
        isInAttackAnimation = false;
    }

    public void AcquireMana(int value) {
        // TODO: Add particle effects and cool stuff here
        Player.AccumulatedMana += value;
    }

    public void ObtainDoubleJump() {
        // TODO: Display a notice here
        Player.HasDoubleJump = true;
    }
}
