using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0;
    public float airSpeed = 0;
    public Vector2 maxVelocity;
    public float jumpStrength = 0;
    public float climbSpeed = 0;
    // How many attackes we do per second
    public float attackSpeed = 0;
    // How strong we are launched upwards when we hit an enemy with a down attack
    public float downAttackKnockbackStrength = 0;

    public string respawnName = null;
    public string respawnSceneName = null;

    public bool IsGamePaused = false;
    // This is the scene that is currently playing, if we need to pause/load a 
    // new scene we can interact with this.
    public Scene CurrentMainGameScene;

    // HACK: This is terrible but we need this here so it doesn't get reset or 
    // deleted when we travel between portals
    public bool isInteractCooldown = false;

    public void resetInteractCooldown() {
        isInteractCooldown = false;
    }

    // TODO: Move transitions into a transition manager
    Dictionary<string, float> TransitionDictionary = new Dictionary<string, float>();

    void TransitionFrame(string name, float timeLeft) {
        TransitionDictionary[name] = timeLeft;
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

}
