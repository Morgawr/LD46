using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
    public float delayStart = 5;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        delayStart = delayStart - Time.deltaTime;
        Debug.Log(delayStart);
        if(delayStart < 0 && transform.position.y < 10195/2)
            transform.Translate(Vector3.up * Time.deltaTime * 100);
    }
}
