using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{

    public ControllableComponent controller;

    void OnTriggerExit2D(Collider2D other) {
        controller.SignalIsClimbing(false);
    }
}
