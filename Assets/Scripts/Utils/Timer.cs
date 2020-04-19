using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    // TODO : This probably can be static
    public IEnumerator Countdown(float duration, Delegates.EmptyDel effector) {
        var normalizedTime = 0f;
        while(normalizedTime <= 1f) {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        effector();
    }

    static public IEnumerator TransitionCountdown(float duration, string name, Delegates.TransitionDel effector) {
        var normalizedTime = 0f;
        while(normalizedTime <= 1f) {
            normalizedTime += Time.deltaTime / duration;
            effector(name, duration - duration * normalizedTime);
            yield return null;
        }
        effector(name, 0);
    }

    static public IEnumerator RefillableTicker(Delegates.CheckAndMaybeContinueDel effector) {
        while(effector()) {
            yield return null;
        }
    }

}
