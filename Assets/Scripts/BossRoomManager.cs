using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public ControllableComponent Player;

    public List<EnemyAI> Bosses = new List<EnemyAI>();

    // Start is called before the first frame update
    void Start() {
    }

    void Update() {
        // This is bad code but we shouldn't really have more than one or two bosses
        // at the same time in an area so it doesn't really matter.
        for(var i = 0; i < Bosses.Count; i++) {
            if(Bosses[i] == null) {
                Bosses.RemoveAt(i);
                i--;
            }
        }
        if(Bosses.Count == 0 && Player.Player.IsInBossBattle) {
            Player.Player.IsInBossBattle = false;
            Player.Player.MusicManager.StopBossMusic();
        } else if(Bosses.Count > 0 && !Player.Player.IsInBossBattle) {
            Player.Player.IsInBossBattle = true;
            Player.Player.MusicManager.StartBossMusic();
        }
    }
}
