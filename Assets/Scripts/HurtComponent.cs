using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtComponent : MonoBehaviour
{

    public delegate void OnHurtReactionDel();

    public OnHurtReactionDel OnHurtReaction;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GetHurt(DamageDealerComponent damageDealer) {
        if(OnHurtReaction != null) {
            OnHurtReaction();
        }
        // TODO: Deal with player getting staggered or during a boss battle lose stamina
        // This exists only if it's an enemy
        var healthComponent = GetComponent<EnemyHealthComponent>();
        if (healthComponent != null) {
            healthComponent.GetDamaged(damageDealer.Damage);
        }
        Debug.Log("Got hit! " + this.name);
    }
}
