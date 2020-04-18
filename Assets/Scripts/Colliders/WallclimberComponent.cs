using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallclimberComponent : MonoBehaviour
{

    public ControllableComponent controller;

    void OnTriggerEnter2D(Collider2D other) {
        controller.SignalIsWalltouching(true);
    }

    void OnTriggerExit2D(Collider2D other) {
        controller.SignalIsWalltouching(false);
    }

}
