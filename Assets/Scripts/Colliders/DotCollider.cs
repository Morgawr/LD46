using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCollider : MonoBehaviour
{
    public bool collide = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        collide = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        collide = false;
    }

    public bool IsCollide()
    {
        return this.collide;
    }
}
