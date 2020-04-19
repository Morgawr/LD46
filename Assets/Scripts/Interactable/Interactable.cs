using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public ControllableComponent controller;
    public Player Player;
    public string tooltip = "Press up to interact";

    protected bool isOnTooltip = false;

    public abstract void Interact();

    void OnTriggerEnter2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(this);
        isOnTooltip = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(null);
        isOnTooltip = false;
    }

    protected void OnGUI()
    {
        if (isOnTooltip)
        {
            GUI.Label(new Rect((Screen.width - 150f) / 2, Screen.height / 10, 200f, 200f), tooltip);
        }
    }

    protected virtual void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }
}
