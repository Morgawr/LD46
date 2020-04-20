using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashComponent : MonoBehaviour
{

    float speed = 0;

    public DamageDealerComponent DamageDealer = null;
    public string AnimationName = null;

    public ControllableComponent player;

    // Call this method when the attack hits something, if we need to
    public Delegates.EmptyDel AttackHitCallback; 

    // Enemies that have already been hit by this slash
    HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

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
        if(player != null) {
            player.OnAttackAnimationEnd(this);
        }
        hitEnemies.Clear();
        this.gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            bool canWeDamage = !hitEnemies.Contains(other);
            var hurtComponent = other.GetComponent<HurtComponent>();
            if(hurtComponent != null && canWeDamage)
                hurtComponent.GetHurt(DamageDealer);
            if(AttackHitCallback != null)
                AttackHitCallback();
            hitEnemies.Add(other);
        } 
    }
}
