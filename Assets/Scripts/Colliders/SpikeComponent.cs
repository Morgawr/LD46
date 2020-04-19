using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeComponent : MonoBehaviour
{
    public ControllableComponent controller;

    void Start() {
        foreach(var p in GameObject.FindGameObjectsWithTag("Avatar")) {
            if(p.scene.name == this.gameObject.scene.name) {
                controller = p.GetComponent<ControllableComponent>();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var vulnerableComponent = other.GetComponent<AbstractVulnerableComponent>();
        if(vulnerableComponent != null) {
            vulnerableComponent.OnDeath();
        }
    }
}
