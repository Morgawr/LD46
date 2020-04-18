using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : Interactable
{
    public ControllableComponent controller;

    void OnTriggerEnter2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(this);
    }
    
    public override void Interact()
    {
        // TODO: Implement interaction with portal
        Debug.Log("Trigger portal interaction in portal");
    }
}
