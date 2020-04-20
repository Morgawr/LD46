using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructComponent : MonoBehaviour
{

    // If you add this to a component it will delete itself on load.

    // Update is called once per frame
    void Update() {
        GameObject.Destroy(this.gameObject);
    }
}
