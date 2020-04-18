using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractVulnerableComponent : MonoBehaviour
{
    public Delegates.EmptyDel OnDeath;
    public abstract void GetDamaged(int value);
}
