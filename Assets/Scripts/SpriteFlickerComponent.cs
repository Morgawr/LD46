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
    Color normalColor = Color.white;
    Color newColor = Color.red;

    void OnFlickerSwap() {
        if(Time.time - startFlickerTime >= TotalDuration) {
            sprite.color = normalColor;
            return;
        }
        if(sprite.color == normalColor) {
            sprite.color = newColor;
        } else {
            sprite.color = normalColor;
        }
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
