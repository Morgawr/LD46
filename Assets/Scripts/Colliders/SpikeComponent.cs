using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeComponent : MonoBehaviour
{
    public ControllableComponent controller;

    void OnTriggerEnter2D(Collider2D other)
    {
        var vulnerableComponent = other.GetComponent<AbstractVulnerableComponent>();
        if(vulnerableComponent != null) {
            vulnerableComponent.OnDeath();
        }
    }
}
