using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 target;

    void Awake()
    {
        target = transform.position;
        target.y += 2f;
    }

    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            target.y *= -1.0f;
        }
    }
}