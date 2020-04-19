using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTicker : MonoBehaviour
{

    public Player Player;
    float timeAccumulator = 0;

    bool needToStop = false;
    public bool isTimerRunning = true;
    Coroutine tickerCoroutine = null;

    bool TimeTick() {
        if(needToStop) {
            needToStop = false;
            tickerCoroutine = null;
            return false;
        }
        timeAccumulator += Time.deltaTime;
        // Update Player.CurrentLife
        // Dumb as fuck routine here
        while(timeAccumulator - 1 > 0) { // We tick every second
            Player.CurrentLife -= Mathf.FloorToInt(Player.LifeLossPerSecond);
            timeAccumulator--;
        }

        if(Player.CurrentLife <= 0) {
            Player.OnDeath();
            tickerCoroutine = null;
            return false;
        }
        return true;
    }

    public void StartTicking() {
        Debug.Log("Started ticking");
        isTimerRunning = true;
        needToStop = false;
        if(tickerCoroutine != null) {
            Debug.Log("Coroutine started twice, ignoring the second time");
        }

        tickerCoroutine = this.StartCoroutine(Timer.RefillableTicker(new Delegates.CheckAndMaybeContinueDel(TimeTick)));
    }

    public void StopTicking() {
        needToStop = true;
        isTimerRunning = false;
    }
    
    void Update() {
        if(isTimerRunning && tickerCoroutine == null)
            StartTicking();
    }
}
