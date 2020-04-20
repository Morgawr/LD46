using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{

    Player player;

    public EnemyAI Snail;
    public EnemyAI Worm;

    bool areWeDone = false;

    void Start(){
        player = Player.GetInstance();
    }

    // Update is called once per frame
    void Update() {
        if(areWeDone)
            return;
        if(Snail == null && Worm == null) {
            areWeDone = true;
            //player.MusicManager.
        }
    }
}
