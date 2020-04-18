using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashComponent : MonoBehaviour
{

    float speed = 0;

    // GameObject that is causing the slash attack
    public Transform DamageDealer = null;

    public void Slash(float speed = 0) {
        this.speed = speed;
        this.gameObject.SetActive(true);
    }

    void OnEnable() {
        var anim = GetComponent<Animation>();
        if(speed != 0)
            anim["SlashAttack"].speed = speed;
        anim.Play();
    }

    public void EndSlash() {
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            // TODO hit code here
            Debug.Log("Hit enemy!");
            other.GetComponent<HurtComponent>().GetHurt(DamageDealer);
            EndSlash();
        }
    }
}
