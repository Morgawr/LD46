using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpiritEnd : Interactable
{
    public string message;
    public float maxDisplayTime = 5;
    float displayTime = 5;
    bool hidden = true;
    Timer messageTimer = new Timer();
    Timer creditsTimer = new Timer();

    void resetMessageCooldown()
    {
        MessageBox.SetActive(false);
    }

    void showCredits()
    {
        SceneManager.LoadSceneAsync("CreditsScene", LoadSceneMode.Additive);
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
        Debug.Log("Trigger Ending tree interaction");
        hidden = false;
        isOnTooltip = false;

        MessageBox.SetActive(true);
        Lore.text = message;
        StartCoroutine(messageTimer.Countdown(maxDisplayTime, new Delegates.EmptyDel(resetMessageCooldown)));
        StartCoroutine(creditsTimer.Countdown(maxDisplayTime, new Delegates.EmptyDel(showCredits)));
    }
}