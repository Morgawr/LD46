using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveFalse : MonoBehaviour
{
    GameObject MessageBox;
    bool start = true;

    void Start()
    {
        MessageBox = GameObject.FindGameObjectWithTag("MessageBox");
        
    }

    void Update()
    {
        if (start)
        {
            MessageBox.SetActive(false);
            start = false;
        }
    }
}
