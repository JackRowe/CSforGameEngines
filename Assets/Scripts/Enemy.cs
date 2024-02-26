using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float rotSpeed = 4.0f;
    GameObject target;
    Rigidbody2D rb;
    Rigidbody2D rbTarget;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rbTarget = target.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!rb) return;

        Vector3 targetDistance = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDistance, -Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, -targetRotation.y, targetRotation.w), rotSpeed / 100);

        if (Mathf.Abs((new Vector3(rb.velocity.x, rb.velocity.y) + targetDistance.normalized).magnitude) <= 10)
        {
            rb.AddForce(targetDistance.normalized * speed);
        }
    }
}
