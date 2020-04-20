using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterComponent : MonoBehaviour
{

    bool hasLoaded = false;
    public MusicManager MusicManager;
    public Camera MainCamera;
    public GameObject Canvas;


    //Timer timer = new Timer();

    // Start is called before the first frame update
    void Start() {
        MusicManager.StartMenu();
    }

    void TransitionToMainGame() {
        MusicManager.StartNormalGame();
        SceneManager.UnloadSceneAsync(this.gameObject.scene);
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scene_L0", LoadSceneMode.Additive);
        GameObject.Destroy(MusicManager.gameObject);
        GameObject.Destroy(MainCamera.gameObject);
        GameObject.Destroy(Canvas);
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.IsPressed("Jump") && !hasLoaded) {
            TransitionToMainGame();
            //StartCoroutine(timer.Countdown(3f, new Delegates.EmptyDel(TransitionToMainGame)));
            hasLoaded = true;
        }
    }

}
