using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    public Player Player;

    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        if(!Player.IsTransitionDone("Pause")) {
            return;
        } else {
            Time.timeScale = 0;
        }

        if(InputManager.IsPressed("pause")) {
            // Do Unpause logic here
            Debug.Log("Unpause detected");
            Player.IsGamePaused = false;
            Time.timeScale = 1;
            Player.StartTransitionEvent("Unpause", .2f);
            SceneManager.UnloadSceneAsync("PauseScene");
        }

        if(InputManager.IsPressed("quit")) {
            Application.Quit();
        }
    }
}
