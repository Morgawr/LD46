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

        ControllableComponent avatar = GetComponent<ControllableComponent>();

        foreach (var p in GameObject.FindGameObjectsWithTag("Portal")) {
            if (p.GetComponent<PortalController>().SelfData.PortalName == portalManager.NextSpawnPortal) {
                avatar.transform.position = p.transform.position;
                break;
            }
        }

        // Point camera to this component
        var mainCamera = GameObject.FindGameObjectsWithTag("MainCineCamera")[0].GetComponent<Cinemachine.CinemachineVirtualCamera>();
        mainCamera.Follow = this.transform;
        mainCamera.LookAt = this.transform;

    }

}
