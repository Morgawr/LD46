using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : Interactable
{
    PortalManager portalManager;
    public PortalManager.PortalTuple SelfData;
    public string TeleportTo;

    public bool IsBossPortal = false;

    bool WeAreRendering = true;

    protected override void Start() {
        base.Start();
        portalManager = GameObject.FindGameObjectsWithTag("PortalManager")[0].GetComponent<PortalManager>();
    }

    protected override void Update() {
        if(Player.IsInBossBattle && WeAreRendering) {
            var renderer = GetComponent<SpriteRenderer>();
            if(renderer != null)
                renderer.enabled = false;
            WeAreRendering = false;
        }
        if(!Player.IsInBossBattle && !WeAreRendering) {
            var renderer = GetComponent<SpriteRenderer>();
            if(renderer != null)
                renderer.enabled = true;
            WeAreRendering = true;
        }
    }

    public override void Interact() {
        // Do not call base.Interact() here becase we have our own interact FX
        // in PortalManager
        // When there is a boss fight, if this is a boss portal then we can't go 
        // back unless the boss is dead.
        if(!(IsBossPortal && Player.IsInBossBattle))
            portalManager.GoToNewPortal(TeleportTo, SelfData.SceneName);
    }

}
