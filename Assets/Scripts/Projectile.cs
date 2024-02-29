using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Creator;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [SerializeField] int speed = 0;

    private void Awake()
    {
        // Get components of the projectile and apply it's velocity
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.AddRelativeForce(new Vector2(0, speed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure the object collided with exists and it isn't the creator of the projectile
        if (collision == null || collision.gameObject == null || collision.gameObject == Creator) { return; }


        switch (collision.gameObject.tag)
        {
            // If the projectile collided with an asteroid, destroy the asteroid
            // if the player fired the projectile, refill the player's fuel
            case ("Asteroid"):
                Destroy(collision.gameObject);
                Destroy(gameObject);

                if(Creator.CompareTag("Player"))
                {
                    SpaceshipController controller = Creator.GetComponent<SpaceshipController>();
                    controller.RefillFuel();
                }

                return;
            // If the projectile collided with an enemy, call the enemy's death method and add money to the player
            case ("Enemy"):
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Death();

                if (Creator.CompareTag("Player"))
                {
                    SpaceshipController controller = Creator.GetComponent<SpaceshipController>();
                    controller.AddMoney(5);
                }

                return;
            // Make sure the projectile doesnt delete itself if it collides with another projectile
            case ("Projectile"):
                return;
            // Finally, if the projectile hit the player, call the player's death method.
            case ("Player"):
                SpaceshipController player = collision.gameObject.GetComponent<SpaceshipController>();
                player.Death();
                return;
            // Otherwise just destroy the projectile
            default:
                Destroy(gameObject);
                return;
        }
    }

}
