using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VelocityClamper
{
    public static Vector2 ClampVelocity(Vector2 current, Vector2 max) {
        return new Vector2(Mathf.Clamp(current.x, -max.x, max.x), Mathf.Clamp(current.y, -max.y, max.y));
    }
}
