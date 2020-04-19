using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulser : MonoBehaviour
{

    public Light Target;
    public float minRange;
    public float maxRange;
    public float interval;

    private float startTime = 0f;
    private float startRange;


    // Start is called before the first frame update
    void Start() {
        startTime = Time.time;
        startRange = Target.range;
    }

    // Update is called once per frame
    void Update() {
        var timeDiff = (Time.time - startTime) * 1000;
        float value = Mathf.Sin(((timeDiff % interval) / interval) * Mathf.PI *2);

        var rangeHalf = (maxRange - minRange) / 2f;
        var updatedValue = value * rangeHalf;
        Target.range = startRange - updatedValue;

    }
}
