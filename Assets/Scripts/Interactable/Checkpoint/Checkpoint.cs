﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : Interactable
{

    public override void Interact()
    {
        // TODO: Implement interaction with portal
        Debug.Log("Trigger Checkpoint interaction");
        //GameObject.findWithTag("Respawn");

        controller.SetRespawn(SceneManager.GetActiveScene().name, this.name);
    }
}
