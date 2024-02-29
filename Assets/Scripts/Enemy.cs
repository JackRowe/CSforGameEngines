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
        // Get the target (player), the player's rigidbody and components of the enemy
        target = GameObject.FindGameObjectWithTag("Player");
        rbTarget = target.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        // Make sure the enemy has a rigidbody and the updateship variable is true, otherwise force the enemy to stop moving
        if (!rb) { return; }
        if (!updateShip) { rb.velocity = Vector3.zero; rb.totalTorque = 0; rb.freezeRotation = true; return; }

        t += Time.fixedDeltaTime;

        // Get the distance between the target and the enemy as a vector
        // then get the rotation needed to look towards the target by using that vector
        // finally apply the rotation to the enemy through a smooth lerp towards it
        Vector3 targetDistance = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDistance, -Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, -targetRotation.y, targetRotation.w), rotSpeed / 100);

        // If the last shot was longer than shotCooldown ago and the random number generated is less than or equal to one
        // spawn a projectile with the enemy's positition and rotation
        // then set it's velocity as the target distance with some slight variation, this will shoot it towards the target
        if (t - lastShot > shotCooldown && Random.Range(1, shotChance) <= 1)
        {
            lastShot = t;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation, transform);
            Projectile script = projectile.GetComponent<Projectile>();
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            script.Creator = gameObject;
            rbProjectile.velocity = ((targetDistance+ new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)) - transform.position)).normalized * 5;
        }

        // Smoothly lerp the enemy's velocity towards the target if they're further away than 10 units
        // Or smoothly lerp the velocity away from the target if they're closer
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
        // When the enemy dies, play the death animation and stop the enemy from moving or colliding
        // Finally destroying it after a second
        animator.SetTrigger("Death");
        updateShip = false;
        circleCollider.enabled = false;
        Destroy(gameObject, 1.0f);

    }
}
