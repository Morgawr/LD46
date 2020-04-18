using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterComponent : MonoBehaviour
{

    bool hasLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasLoaded) {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("2DPlatformScene", LoadSceneMode.Additive);
            hasLoaded = true;
        }
    }
}
