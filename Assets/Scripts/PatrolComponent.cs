using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolComponent : MonoBehaviour
{
    public List<Transform> Points;
    int currentPoint;

    public float marginBeforeNextPoint = 0;

    public void SelectClosestPoint(Transform origin) {
        float distance = Mathf.Infinity;
        for(int i = 0; i < Points.Count; i++) {
            var new_distance = Mathf.Abs(Vector2.Distance(origin.position, Points[i].position));
            if(new_distance < distance) {
                distance = new_distance;
                currentPoint = i;
            }
        }
    }

    public void TriggerNextPoint(bool random = false) {
        if(random) {
            var point = currentPoint;
            while(point == currentPoint && Points.Count > 1) {
                point = Random.Range(0, Points.Count);
            }
            currentPoint = point;
        } else {
            currentPoint = (currentPoint + 1) %  Points.Count;
        }
    }

    public Transform GetCurrentPoint() {
        return Points[currentPoint];
    }

    public bool ShouldGetNextPoint(Transform origin) {
        return ((Mathf.Abs(Vector2.Distance(origin.position, Points[currentPoint].position))) - marginBeforeNextPoint) < 0.5;
    }

    void OnDrawGizmosSelected () {
        foreach(var point in Points) {
            Gizmos.color = new Color(0f, 0.4f, 0.4f, .1f);
            Gizmos.DrawSphere(point.position, marginBeforeNextPoint);
        }
    }
}
