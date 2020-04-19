using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVulnerableComponent : AbstractVulnerableComponent
{
    Player player;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

    public Player Player;
    GameObject[] StaggerList;

    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        StaggerList = GameObject.FindGameObjectsWithTag("Stagger");

    }

    public override void GetDamaged(int value) {
        Debug.Log(value);

        if (player.IsInBossBattle)
        {
            Debug.Log(value);
            player.CurrentLife -= value;
            return;
        }
        
        Timer exaustedTimer = new Timer();

        void resetExaustedCooldown()
        {
            Debug.Log("Rest Exauxted!");
            Player.isExausted = false;
            Player.CurrentStag = Player.MaxStag;

            foreach(var stagger in StaggerList)
            {
                stagger.SetActive(true);
            }
        }

        var currentStag = Player.CurrentStag;
        if (currentStag > 0)
        {
            Debug.Log(currentStag);

            if (Player.isFlickering)
                return;

            StaggerList[currentStag - 1].SetActive(false);
            Player.CurrentStag--;
        }
        else
        {
            Debug.Log("Exausted!");
            if (Player.isExausted)
                return;

            Player.isExausted = true;
            StartCoroutine(exaustedTimer.Countdown(5f, new Delegates.EmptyDel(resetExaustedCooldown)));
        }
    }
}
