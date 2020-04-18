using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public GameObject BaseProjectile;

    public ProjectileComponent SpawnProjectile(GameObject projectile, Transform target, Transform parent) {
        var proj = Instantiate(projectile, parent.position, Quaternion.identity).GetComponent<ProjectileComponent>();
        var flip = false;
        var heading = Vector3.zero;
        if(target.position.x < parent.position.x) 
            flip = true;
        heading = target.position - parent.position;
        Debug.Log(heading);
        var distance = Vector2.Distance(parent.position, target.position);
        proj.Direction = heading / distance;
        if(flip)
            proj.Direction.x *= -1;
        proj.transform.SetParent(parent);
        return proj;
    }
}
