using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlickerComponent : MonoBehaviour
{

    public SpriteRenderer sprite;

    public float FlickerDuration; 
    public float TotalDuration;
    
    float startFlickerTime;

    Timer flickerTimer;

    void OnFlickerSwap() {
        if(Time.time - startFlickerTime >= TotalDuration) {
            sprite.enabled = true;
            return;
        }
        sprite.enabled = !sprite.enabled;
        StartCoroutine(flickerTimer.Countdown(FlickerDuration, new Delegates.EmptyDel(OnFlickerSwap)));
    }

    // Start is called before the first frame update
    void Start() {
        flickerTimer = new Timer();
    }

    public void StartFlicker() {
        startFlickerTime = Time.time;
        OnFlickerSwap();
    }

}
