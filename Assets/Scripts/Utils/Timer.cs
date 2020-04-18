using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public delegate void SideEffector();
    public IEnumerator Countdown(float duration, SideEffector effector) {
        var normalizedTime = 0f;
        while(normalizedTime <= 1f) {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        effector();
    }
}
