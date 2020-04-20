using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Totem : Interactable
{
    public string message;
    public float maxDisplayTime = 5;
    bool hidden = true;
    Timer messageTimer = new Timer();

    void resetMessageCooldown()
    {
        hidden = true;
        MessageBox.SetActive(false);
    }

    protected override void Update()
    {
    }

    protected override void OnGUI()
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
        hidden = false;
        isOnTooltip = false;

        if(message != "")
            MessageBox.SetActive(true);
        Lore.text = message;
        StartCoroutine(messageTimer.Countdown(maxDisplayTime, new Delegates.EmptyDel(resetMessageCooldown)));
    }
}
