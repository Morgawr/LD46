using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtComponent : MonoBehaviour
{

    public delegate void OnHurtReactionDel();

    public OnHurtReactionDel OnHurtReaction;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GetHurt(Transform damageDealer) {
        if(OnHurtReaction != null) {
            OnHurtReaction();
        }
        // TODO get damage (???) logic
        Debug.Log("Got hit! " + this.name);
    }
}
