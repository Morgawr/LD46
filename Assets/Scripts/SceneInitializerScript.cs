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

    bool InitializeRespawn() {
        var Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        if(Player.WeDiedAndWeAreRespawning) {
            // Remove all mana here
            // Refill all life here
            Player.AccumulatedMana = 0;
            Player.CurrentLife = Player.MaxLife;
            Player.WeDiedAndWeAreRespawning = false;
            this.GetComponent<ControllableComponent>().Respawn();
            return true;
        }
        return false;
    }

    // This component is used to set all the parameters for the player and 
    // anything else that needs to be set in a scene after a transition.
    // (Like audio, enemy respawn, etc)
    void Start() {
        // Point camera to this component
        var mainCamera = GameObject.FindGameObjectsWithTag("MainCineCamera")[0].GetComponent<Cinemachine.CinemachineVirtualCamera>();
        mainCamera.Follow = this.transform;
        mainCamera.LookAt = this.transform;

        // Bind the avatar controller to the player global class
        var avatar = GetComponent<ControllableComponent>();
        var Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        Player.RegisterAvatar(avatar);
        Player.ClearSpotters();

        if(!InitializeRespawn()) {
            InitializePortal();
        }
    }

}
