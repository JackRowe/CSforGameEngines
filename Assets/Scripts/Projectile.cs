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
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.AddRelativeForce(new Vector2(0, speed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == Creator) { return; }


        switch (collision.gameObject.tag)
        {
            case ("Asteroid"):
                Destroy(collision.gameObject);
                Destroy(gameObject);

                if(Creator.CompareTag("Player"))
                {
                    SpaceshipController controller = Creator.GetComponent<SpaceshipController>();
                    controller.RefillFuel();
                }

                return;
            case ("Enemy"):
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Death();

                if (Creator.CompareTag("Player"))
                {
                    SpaceshipController controller = Creator.GetComponent<SpaceshipController>();
                    controller.AddMoney(5);
                }

                return;
            case ("Projectile"):
                return;
            case ("Player"):
                SpaceshipController player = collision.gameObject.GetComponent<SpaceshipController>();
                player.Death();
                return;
            default:
                Destroy(gameObject);
                return;
        }
    }

}
