using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : Interactable
{
    public string tagName;
    public int ManaRefillPerInterval = 0;

    void RefillLife() {
        int toRefill = Player.AccumulatedMana - ManaRefillPerInterval > 0 ? ManaRefillPerInterval : Player.AccumulatedMana;
        Player.AccumulatedMana = (int)Mathf.Clamp(Player.AccumulatedMana - ManaRefillPerInterval, 0, Mathf.Infinity);
        int maxAmountCanRefill = Player.MaxLife - Player.CurrentLife;
        if(maxAmountCanRefill - toRefill > 0) {
            Player.CurrentLife += toRefill;
            toRefill = 0;
        } else {
            Player.CurrentLife = Player.MaxLife; // We top up the mana
            toRefill -= maxAmountCanRefill; // We leave some leftover to put back 
        }
        Player.AccumulatedMana += toRefill;
    }

    public override void Interact()
    {
        // SetRespawn does nothing if the respawn is the same already so 
        // we can keep calling this without problems.
        controller.SetRespawn(this.gameObject.scene.name, this.tagName);
        RefillLife();
    }

    public string GetTagName()
    {
        return this.tagName;
    }
}
