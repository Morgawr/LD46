using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : MonoBehaviour
{

    public int HP = 0;
    public int MaxHP = 0;

    // Start is called before the first frame update
    void Start() {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
