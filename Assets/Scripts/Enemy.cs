using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float rotSpeed = 4.0f;
    [SerializeField] GameObject projectilePrefab;

    private GameObject target;
    private Rigidbody2D rb;
    private Rigidbody2D rbTarget;
    private Animator animator;
    private CircleCollider2D circleCollider;

    private bool updateShip = true;
    private float t = 0.0f;

    private float lastShot = 0.0f;
    private float shotCooldown = 3.0f;
    private int shotChance = 100;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rbTarget = target.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if (!rb) { return; }
        if (!updateShip) { rb.velocity = Vector3.zero; rb.totalTorque = 0; rb.freezeRotation = true; return; }

        t += Time.fixedDeltaTime;

        Vector3 targetDistance = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDistance, -Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, -targetRotation.y, targetRotation.w), rotSpeed / 100);

        print(Random.Range(1, shotChance));
        if (t - lastShot > shotCooldown && Random.Range(1, shotChance) <= 1)
        {
            lastShot = t;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation, transform);
            Projectile script = projectile.GetComponent<Projectile>();
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            script.Creator = gameObject;
            rbProjectile.velocity = ((target.transform.position+ new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)) - transform.position)).normalized * 5;
        }

        if (targetDistance.magnitude > 10)
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
        circleCollider.enabled = false;
        Destroy(gameObject, 1.0f);

    }
}
