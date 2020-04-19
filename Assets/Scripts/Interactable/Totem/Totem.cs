using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Interactable
{
    public string message;
    public float maxDisplayTime = 5;
    float displayTime = 5;
    bool hidden = true;

    void Update()
    {
        if (!hidden)
        {
            displayTime -= Time.deltaTime;
            if(displayTime<=0)
                hidden = true;
        }
    }

    void OnGUI()
    {
        if (!hidden)
        {
            GUI.Label(new Rect((Screen.width-400f) / 2, Screen.height / 10, 400f, 200f), message);
        }
        else 
        { 
            base.OnGUI(); 
        }
    }

    public override void Interact()
    {
        // TODO: Implement interaction with totem
        Debug.Log("Trigger Totem interaction");
        displayTime = maxDisplayTime;
        hidden = false;
        isOnTooltip = false;
    }
}
