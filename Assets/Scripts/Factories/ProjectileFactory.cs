using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public GameObject BaseProjectile;

    public ProjectileComponent SpawnProjectile(GameObject projectile, Transform target, Transform parent) {
        var proj = Instantiate(projectile, parent.position, Quaternion.identity).GetComponent<ProjectileComponent>();
        var heading = Vector3.zero;
        if(target.position.x < parent.position.x) { 
            heading = parent.position - target.position;
        } else {
            heading = target.position - parent.position;
        }
        var distance = Mathf.Abs(Vector2.Distance(parent.position, target.position));
        proj.Direction = heading / distance;
        proj.transform.SetParent(parent);
        return proj;
    }
}
