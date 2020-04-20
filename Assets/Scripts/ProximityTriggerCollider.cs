using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProximityTriggerCollider : MonoBehaviour
{


    public abstract class TriggerBehaviour{
        public virtual void TriggerAction() {

        }

        public virtual void TriggerExitAction() {

        }
    }

    public TriggerBehaviour behaviour;
    public string BehaviourName;

    void Start() {
        switch(BehaviourName) {
            case "StartMusic":
                behaviour = new StartMusic();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)  {
        if(behaviour != null)
            behaviour.TriggerAction();
    }

    void OnTriggerExit2D(Collider2D other)  {
        if(behaviour != null)
            behaviour.TriggerExitAction();
    }
}
