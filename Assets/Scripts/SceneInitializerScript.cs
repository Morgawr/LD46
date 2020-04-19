using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializerScript : MonoBehaviour
{

    void InitializePortal() {
        var portalManager = GameObject.FindGameObjectsWithTag("PortalManager")[0].GetComponent<PortalManager>();
        if(portalManager.NextSpawnPortal == "")
            return;
        foreach (var p in GameObject.FindGameObjectsWithTag("Portal")) {
            if (p.GetComponent<PortalController>().SelfData.PortalName == portalManager.NextSpawnPortal) {
                this.transform.position = p.transform.position;
                break;
            }
        }

    }

    // This component is used to set all the parameters for the player and 
    // anything else that needs to be set in a scene after a transition.
    // (Like audio, enemy respawn, etc)
    void Start() {
        // Point camera to this component
        var mainCamera = GameObject.FindGameObjectsWithTag("MainCineCamera")[0].GetComponent<Cinemachine.CinemachineVirtualCamera>();
        mainCamera.Follow = this.transform;
        mainCamera.LookAt = this.transform;

        InitializePortal();
    }

}
