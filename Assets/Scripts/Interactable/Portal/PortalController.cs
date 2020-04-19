using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : Interactable
{
    PortalManager portalManager;
    public PortalManager.PortalTuple SelfData;
    public string TeleportTo;

    public bool IsBossPortal = false;

    protected override void Start() {
        base.Start();
        portalManager = GameObject.FindGameObjectsWithTag("PortalManager")[0].GetComponent<PortalManager>();
    }

    public override void Interact() {
        // When there is a boss fight, if this is a boss portal then we can't go 
        // back unless the boss is dead.
        if(!(IsBossPortal && Player.IsInBossBattle))
            portalManager.GoToNewPortal(TeleportTo, SelfData.SceneName);
    }

}
