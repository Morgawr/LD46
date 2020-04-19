using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Totem : Interactable
{
    public string message;
    public float maxDisplayTime = 5;
    float displayTime = 5;
    bool hidden = true;
    Timer messageTimer = new Timer();

    void resetMessageCooldown()
    {
        hidden = true;
        MessageBox.SetActive(false);
    }

    void Update()
    {
    }

    void OnGUI()
    {
        if (!hidden)
        {
            
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

        
        MessageBox.SetActive(true);
        Lore.text = message;
        StartCoroutine(messageTimer.Countdown(5f, new Delegates.EmptyDel(resetMessageCooldown)));
    }
}
