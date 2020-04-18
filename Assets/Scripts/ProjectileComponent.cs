using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{

    public float Duration;
    public float Speed;
    public Vector2 Direction;
    public DamageDealerComponent DamageDealer = null;

    Timer timer = new Timer();

    ControllableComponent Player;


    void Die() {
        Destroy(this.gameObject);
    }

    void OnDurationExpired() {
        Debug.Log("Duration expired");
        Die();
    }

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Avatar")[0].GetComponent<ControllableComponent>();
        StartCoroutine(timer.Countdown(Duration, new Timer.SideEffector(OnDurationExpired)));
    }

    // Update is called once per frame
    void Update() {
        this.transform.Translate(Direction * Speed * Time.deltaTime);
        // TODO: rotate projectile towards direction
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject == transform.parent.gameObject)
            return;
        if(other.gameObject == Player.gameObject) {
            Player.GetComponent<HurtComponent>().GetHurt(DamageDealer);
        }
        Die();
    }
}
