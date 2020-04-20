using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtComponent : MonoBehaviour
{

    public Delegates.EmptyDel OnHurtReaction;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GetHurt(DamageDealerComponent damageDealer) {

        SFXManager.GetInstance().PlayFX("Impact");
        var healthComponent = GetComponent<AbstractVulnerableComponent>();
        if (healthComponent != null) {
            healthComponent.GetDamaged(damageDealer.Damage);
        }
        if(OnHurtReaction != null) {
            OnHurtReaction();
        }
    }
}
