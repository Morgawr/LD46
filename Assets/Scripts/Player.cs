using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0;
    public float airSpeed = 0;
    public Vector2 maxVelocity;
    public float jumpStrength = 0;
    public float climbSpeed = 0;
    // How many attackes we do per second
    public float attackSpeed = 0;
    // How strong we are launched upwards when we hit an enemy with a down attack
    public float downAttackKnockbackStrength = 0;
}
