using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamageComponent : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        var vulnerableComponent = other.GetComponent<AbstractVulnerableComponent>();
        if(vulnerableComponent != null) {
            vulnerableComponent.OnDeath();
        }
    }
}
