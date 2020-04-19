using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public ControllableComponent controller;
    public Player Player;

    public abstract void Interact();

    void OnTriggerEnter2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(null);
    }

    protected virtual void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }
}
