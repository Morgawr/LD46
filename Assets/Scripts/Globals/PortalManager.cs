using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    [System.Serializable]
    public class PortalTuple {
        public string SceneName;
        public string PortalName;
    }
    public PortalTuple[] Portals;

    public Dictionary<string, string> PortalLookup = new Dictionary<string, string>();

    void Awake() {
        foreach(PortalTuple p in Portals) {
            PortalLookup[p.PortalName] = p.SceneName;
        }
    }

    // Place where we are going to spawn in the new scene, if any
    public string NextSpawnPortal;

    public void GoToNewPortal(string newPortal, string oldScene) {
        NextSpawnPortal = newPortal;
        var newScene = PortalLookup[NextSpawnPortal];
        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(oldScene);
    }
}