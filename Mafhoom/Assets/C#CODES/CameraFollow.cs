using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPosition = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
    }
}
