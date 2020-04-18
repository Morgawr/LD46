using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVulnerableComponent : AbstractVulnerableComponent
{

    public override void GetDamaged(int value) {
        // Do nothing because player has no health
        // TODO: add stamina values
    }
}
