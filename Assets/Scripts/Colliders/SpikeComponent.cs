using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeComponent : MonoBehaviour
{
    public ControllableComponent controller;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Spike");
        controller.OnDeath();
    }
}
