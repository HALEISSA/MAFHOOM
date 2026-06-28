using UnityEngine;
using System;

public class AutoWalkToPoint : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;

    private bool isWalking = false;

    public Action OnArrived;

    public void StartWalking(Transform newTarget)
    {
        target = newTarget;
        isWalking = true;
    }

    public void StartWalking(Transform newTarget, Action arrivalAction)
    {
        target = newTarget;
        OnArrived = arrivalAction;
        isWalking = true;
    }

    void Update()
    {
        if (!isWalking || target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            isWalking = false;
            OnArrived?.Invoke();
        }
    }
}