using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingArrow : MonoBehaviour
{
    public float rotationSpeed;
    private Transform target;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        FaceTarget();
    }

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FaceTarget()
    {
        Vector3 direction = target.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }
}
