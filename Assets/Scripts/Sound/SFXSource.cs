using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSource : MonoBehaviour
{
    public string Name;
    public AudioSource Source;

    void Start () {
        Source = GetComponent<AudioSource>();
    }
}
