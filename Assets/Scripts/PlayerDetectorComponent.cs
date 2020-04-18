using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectorComponent : MonoBehaviour
{

    public ControllableComponent player;
    public EnemyAI ai;

    public bool HasLOS = true;
    public Transform LOSOrigin;

    Collider2D selfCollider = null;

    void Start() {
        selfCollider = this.GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D other) {
        if (selfCollider.IsTouchingLayers(LayerMask.GetMask("PlayerLayer"))) {
            if (!HasLOS) {
                ai.PlayerSpotted(true);
                return;
            }
            var losLayerMask = LayerMask.GetMask("PlayerLayer", "PlatformLayer");
            var direction = (player.transform.position - LOSOrigin.transform.position).normalized;
            RaycastHit2D sightTest = Physics2D.Raycast(LOSOrigin.transform.position, direction, Mathf.Infinity, losLayerMask);
            if (sightTest.collider != null) {
                if(sightTest.collider.gameObject == player.gameObject) {
                    ai.PlayerSpotted(true);
                    return;
                }
            }
            ai.PlayerSpotted(false);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!selfCollider.IsTouchingLayers(LayerMask.GetMask("PlayerLayer"))) {
            ai.PlayerSpotted(false);
        }
    }
}
