using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float rotSpeed = 4.0f;

    GameObject target;
    Rigidbody2D rb;
    Rigidbody2D rbTarget;
    Animator animator;

    bool updateShip = true;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rbTarget = target.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!rb) return;
        if (!updateShip) rb.velocity = Vector3.zero; rb.totalTorque = 0; rb.freezeRotation = true;

        Vector3 targetDistance = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDistance, -Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, -targetRotation.y, targetRotation.w), rotSpeed / 100);

        if(targetDistance.magnitude > 10)
        {
            rb.velocity = Vector3.Slerp(rb.velocity, rbTarget.velocity + new Vector2(targetDistance.x, targetDistance.y), speed / 100);
        }
        else
        {
            rb.velocity = Vector3.Slerp(rb.velocity, rbTarget.velocity - new Vector2(targetDistance.x, targetDistance.y), speed / 100);
        }
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        updateShip = false;
        Destroy(gameObject, 1.0f);

    }
}
