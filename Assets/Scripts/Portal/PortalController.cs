using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public ControllableComponent controller;

    void OnTriggerEnter2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(false);
    }
}
