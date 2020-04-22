using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVulnerableComponent : AbstractVulnerableComponent
{
    Player Player;
    GameObject[] StaggerList;
    Timer exhaustedTimer = new Timer();

    void Start()
    {
        Debug.Log("Reload this!");
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        StaggerList = GameObject.FindGameObjectsWithTag("Stagger");
    }

    void resetExhaustedCooldown()
    {
        Player.isExhausted = false;
        Player.CurrentStag = Player.MaxStag;

        foreach (var stagger in StaggerList)
        {
            stagger.SetActive(true);
        }
    }

    public override void GetDamaged(int value) {

        // We shouldn't get damaged when flickering (i-frames)
        if(Player.isFlickering) {
            return;
        }
        
        if (Player.IsInBossBattle)
        {
            Player.CurrentLife -= value;
            return;
        }


        var currentStag = Player.CurrentStag;
        if (currentStag > 0)
        {
            if (Player.isFlickering)
                return;

            StaggerList[currentStag - 1].SetActive(false);
            Player.CurrentStag--;
        }
        else
        {
            if (Player.isExhausted)
                return;

            SFXManager.GetInstance().PlayFX("Staggered");
            Player.isExhausted = true;
            StartCoroutine(exhaustedTimer.Countdown(5f, new Delegates.EmptyDel(resetExhaustedCooldown)));
        }
    }
}
