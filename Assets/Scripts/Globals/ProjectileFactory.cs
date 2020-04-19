using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public GameObject BaseProjectile;
    public GameObject LargerProjectile;

    // TODO: figure out sprite flip and origin flip for projectiles when origin target is flipped

    public ProjectileComponent SpawnProjectile(GameObject projectile, Vector2 target, Transform parent) {
        var proj = Instantiate(projectile, parent.position, Quaternion.identity).GetComponent<ProjectileComponent>();
        var heading = Vector3.zero;
        heading = target - (Vector2)parent.position;
        var distance = Vector2.Distance(parent.position, target);
        proj.Direction = heading / distance;
        proj.transform.SetParent(parent.parent, true);
        return proj;
    }
}
