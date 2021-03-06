﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public static Player GetInstance() {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    // Physics stats 
    public float moveSpeed = 0;
    public float airSpeed = 0;
    public Vector2 maxVelocity;
    public float jumpStrength = 0;
    public float climbSpeed = 0;

    // Engine stats
    public float LifeLossPerSecond = 1;
    public MusicManager MusicManager;

    // How many attackes we do per second
    public float attackSpeed = 0;
    // How strong we are launched upwards when we hit an enemy with a down attack
    public float downAttackKnockbackStrength = 0;

    public string respawnName = null;
    public string respawnSceneName = null;

    public bool WeDiedAndWeAreRespawning = false;
    public bool RespawnRoutineHasStarted = false;
    public bool IsGamePaused = false;
    // This is the scene that is currently playing, if we need to pause/load a 
    // new scene we can interact with this.
    public Scene CurrentMainGameScene;

    public bool HasDoubleJump = false;

    // HACK
    public GameObject MessageBox;
    public Text Lore;

    // This is the logic when we are in a boss fight
    public bool IsInBossBattle = false;


    // Logic for the lifebar 
    public int CurrentLife = 0;
    public int MaxLife = 0;
    public bool IsTimeTicking = false;
    public int AccumulatedMana = 0;
    public int CurrentStag = 3;
    public int MaxStag = 3;
    public bool isFlickering = false;
    public bool isExhausted = false;

    public bool hasGameEnded = false;

    // HACK: This is terrible but we need this here so it doesn't get reset or 
    // deleted when we travel between portals
    public bool isInteractCooldown = false;

    public void resetInteractCooldown() {
        isInteractCooldown = false;
    }

    // TODO: Move transitions into a transition manager
    Dictionary<string, float> TransitionDictionary = new Dictionary<string, float>();

    HashSet<EnemyAI> spotters = new HashSet<EnemyAI>();

    HashSet<string> bossesDefeated = new HashSet<string>();

    void TransitionFrame(string name, float timeLeft) {
        TransitionDictionary[name] = timeLeft;
    }

    public bool HasDefeated(string name) {
        return bossesDefeated.Contains(name);
    }

    public void DefeatedBoss(string name){
        bossesDefeated.Add(name);
    }

    public void StartTransitionEvent(string name, float duration) {
        name = name.ToLower();
        TransitionDictionary[name] = duration;
        StartCoroutine(Timer.TransitionCountdown(duration, name, new Delegates.TransitionDel(TransitionFrame)));
    }

    public bool IsTransitionDone(string name) {
        name = name.ToLower();
        return TransitionDictionary.ContainsKey(name) && TransitionDictionary[name] == 0;
    }

    public bool DoesTransitionExist(string name) {
        name = name.ToLower();
        return TransitionDictionary.ContainsKey(name);
    }

    public bool IsInCombat() {
        return spotters.Count > 0;
    }

    public void ClearSpotters() {
        spotters.Clear();
    }

    public void RemoveSpotter(EnemyAI enemy) {
        spotters.Remove(enemy);
    }

    public void AddSpotter(EnemyAI enemy) {
        spotters.Add(enemy);
    }

    void Awake() {
        Application.targetFrameRate = 60;
    }

    void Start() {
        CurrentLife = MaxLife;
    }

    void LateUpdate() {
        MusicManager.SetCombatMode(IsInCombat());
        if(hasGameEnded) {
            EndGame();
        }
    }

    public Delegates.EmptyDel OnDeath;
    // This function is called to bind Avatar-specific methods to the
    // globally accessible player class.
    public void RegisterAvatar(ControllableComponent avatar) {
        OnDeath = new Delegates.EmptyDel(avatar.OnDeath);
    }

    public void EndGame() {
        // when we get here we have killed all the bosses and the life bar is not
        // depleting anymore.
        hasGameEnded = true;
        MusicManager.StartEndgame();
    }
}
