using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : MonoBehaviour
{
    // TODO: Maybe turn this class in a generic Health/Stamina class for enemy/player
    public int HP = 0;
    public int MaxHP = 0;

    public Delegates.EmptyDel OnDeath;

    // Start is called before the first frame update
    void Start() {
        HP = MaxHP;
    }

    public void GetDamaged(int value) {
        HP -= value;
        if(HP <= 0) {
            OnDeath();
        }
    }

}
