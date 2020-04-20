using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{

    public static SFXManager GetInstance() {
        return GameObject.FindGameObjectsWithTag("SFXManager")[0].GetComponent<SFXManager>();
    }

    public List<SFXSource> SFXList = new List<SFXSource>();

    Dictionary<string, SFXSource> SFXs = new Dictionary<string, SFXSource>();


    // Start is called before the first frame update
    void Start() {
        foreach(SFXSource s in SFXList) {
            SFXs.Add(s.Name, s);
        }
    }

    public void PlayFX(string name, bool loop = false) {
        if(!SFXs.ContainsKey(name)) {
            throw new UnityException("SFX does not exist or has not been loaded: " + name);
        }
        var fx = SFXs[name].Source;
        if(!loop) {
            fx.PlayOneShot(fx.clip);
        } else {
            if(!fx.isPlaying) {
                fx.loop = true;
                fx.Play();
            }
        }
    }

    public void StopFX(string name) {
        if(!SFXs.ContainsKey(name)) {
            throw new UnityException("SFX does not exist or has not been loaded: " + name);
        }
        var fx = SFXs[name].Source;
        if(fx.isPlaying && !fx.loop) {
            Debug.Log("We tried to stop a non-looping FX");
        }
        if(fx.isPlaying && fx.loop) {
            fx.loop = false;
            fx.Stop();
        }
    }

    public bool IsFXPlaying(string name) {
        if(!SFXs.ContainsKey(name)) {
            throw new UnityException("SFX does not exist or has not been loaded: " + name);
        }
        return SFXs[name].Source.isPlaying;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
