using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : Interactable
{
    PortalManager portalManager;
    public PortalManager.PortalTuple SelfData;
    public string TeleportTo;

    protected override void Start() {
        base.Start();
        portalManager = GameObject.FindGameObjectsWithTag("PortalManager")[0].GetComponent<PortalManager>();
    }

    public override void Interact() {
        portalManager.GoToNewPortal(TeleportTo, SelfData.SceneName);
    }

}
