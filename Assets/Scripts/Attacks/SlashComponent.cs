using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashComponent : MonoBehaviour
{

    float speed = 0;

    public DamageDealerComponent DamageDealer = null;
    public string AnimationName = null;

    // Call this method when the attack hits something, if we need to
    public Delegates.EmptyDel AttackHitCallback; 

    public void Slash(float speed = 0) {
        this.speed = speed;
        this.gameObject.SetActive(true);
    }

    void OnEnable() {
        var anim = GetComponent<Animation>();
        if(speed != 0)
            anim[AnimationName].speed = speed;
        anim.Play();
    }

    public void EndSlash() {
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            other.GetComponent<HurtComponent>().GetHurt(DamageDealer);
            if(AttackHitCallback != null)
                AttackHitCallback();
            EndSlash();
        } 
    }
}
