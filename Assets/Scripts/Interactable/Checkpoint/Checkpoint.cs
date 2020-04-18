using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : Interactable
{
    public string tagName;

    public override void Interact()
    {
        Debug.Log("Trigger Checkpoint interaction");
        controller.SetRespawn(SceneManager.GetActiveScene().name, this.tagName);
    }

    public string GetTagName()
    {
        return this.tagName;
    }
}
