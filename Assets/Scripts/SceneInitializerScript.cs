using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializerScript : MonoBehaviour
{
    // This component is used to set all the parameters for the player and 
    // anything else that needs to be set in a scene after a transition.
    // (Like audio, enemy respawn, etc)
    void Start() {
        var portalManager = GameObject.FindGameObjectsWithTag("PortalManager")[0].GetComponent<PortalManager>();
        if(portalManager.NextSpawnPortal == "")
            return;

        ControllableComponent avatar = null;
        
        var currentScene = portalManager.PortalLookup[portalManager.NextSpawnPortal];
        foreach (var a in GameObject.FindGameObjectsWithTag("Avatar")) {
            if(a.gameObject.scene.name == currentScene) {
                avatar = a.GetComponent<ControllableComponent>();
                break;
            }
        }
        if(avatar == null) 
            throw new UnityException("Cannot find avatar for scene " + currentScene);

        foreach (var p in GameObject.FindGameObjectsWithTag("Portal")) {
            if (p.GetComponent<PortalController>().SelfData.PortalName == portalManager.NextSpawnPortal) {
                avatar.transform.position = p.transform.position;
                break;
            }
        }
    }

}
