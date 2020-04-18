using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Interactable
{
    public string message;

    public override void Interact()
    {
        // TODO: Implement interaction with portal
        Debug.Log("Trigger Totem interaction");
        //GameObject.findWithTag("Respawn");

    }
}
