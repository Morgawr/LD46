using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootholdComponent : MonoBehaviour
{
    public ControllableComponent controller;

    Collider2D selfCollider = null;
    
    void Start() {
        selfCollider = this.GetComponent<Collider2D>();
    }

    void Update() {
        if(selfCollider.IsTouchingLayers(LayerMask.GetMask("FootholdLayer", "EnemyLayer")))
            controller.SignalInAir(false);
        else 
            controller.SignalInAir(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        controller.SignalInAir(false);
    }

    void OnTriggerExit2D(Collider2D other) {
        controller.SignalInAir(true);
    }
}
