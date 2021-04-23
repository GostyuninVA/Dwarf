using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    public float TopLimit = 10.0f;
    public float BottomLimit = -10.0f;
    public float FollowSpeed = 0.5f;

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 newPosition = transform.position;

            newPosition.y = Mathf.Lerp(newPosition.y, Target.position.y, FollowSpeed);

            newPosition.y = Mathf.Min(newPosition.y, TopLimit);
            newPosition.y = Mathf.Max(newPosition.y, BottomLimit);

            transform.position = newPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 topPoint = new Vector3(transform.position.x, TopLimit, transform.position.z + 10);
        Vector3 bottomPoint = new Vector3(transform.position.x, BottomLimit, transform.position.z + 10);

        Gizmos.DrawLine(topPoint, bottomPoint);
    }
}
