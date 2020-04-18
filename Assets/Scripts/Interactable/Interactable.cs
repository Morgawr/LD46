using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public ControllableComponent controller;

    public abstract void Interact();

    void OnTriggerEnter2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(null);
    }
}
