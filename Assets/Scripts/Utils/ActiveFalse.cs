﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveFalse : MonoBehaviour
{
    GameObject MessageBox;
    bool start = true;

    void Start()
    {
        MessageBox = GameObject.FindGameObjectsWithTag("MessageBox")[0];
        
    }

    void Update()
    {
        Debug.Log("is it working");
        if (start)
        {
            MessageBox.SetActive(false);
            start = false;
        }
    }
}