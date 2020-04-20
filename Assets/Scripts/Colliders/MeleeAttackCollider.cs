using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollider : MonoBehaviour
{
    ControllableComponent controller;
    public DamageDealerComponent damageDealer;

    void Start() {
        foreach(var p in GameObject.FindGameObjectsWithTag("Avatar")) {
            if(p.scene.name == this.gameObject.scene.name) {
                controller = p.GetComponent<ControllableComponent>();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // We know it's only the player that can collide with Melee enemy attack
        // but we verify just to be sure
        if(other.gameObject != controller.gameObject) {
            Debug.Log("collided with something that we shouldn't have: " + other.gameObject.name);
            return;
        }

        // TODO: have damage cooldown logic

        var hurtComponent = other.GetComponent<HurtComponent>();
        if(hurtComponent != null) {
            hurtComponent.GetHurt(damageDealer);
        }
    }
}
