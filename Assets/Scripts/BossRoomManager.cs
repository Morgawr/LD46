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
        if(Bosses.Count == 0 && Player.Player.IsInBossBattle) {
            Player.Player.IsInBossBattle = false;
            Player.Player.MusicManager.StopBossMusic();
        } else if(Bosses.Count > 0 && !Player.Player.IsInBossBattle) {
            Player.Player.IsInBossBattle = true;
            Player.Player.MusicManager.StartBossMusic();
        }
    }
}
