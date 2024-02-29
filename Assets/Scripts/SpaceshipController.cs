using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    private Rigidbody2D rb;
    private CompositeCollider2D compositeCollider;
    private ParticleSystem engineEmitter;
    private Animator animator;
    private Spawner spawner;
    private Interface ui;

    [SerializeField] float speed = 2.0f;
    [SerializeField] float rotSpeed = 4.0f;
    private float throttle = 0.0f;
    private float rotation = 0.0f;
    private float fuel = 100.0f;
    private bool updateShip = true;
    private int survivors = 0;
    private int money = 0;

    private float t = 0.0f;

    private float lastShot = 0.0f;
    private float shotCooldown = 0.2f;

    private float lastAsteroid = 0.0f;
    private float asteroidCooldown = 10.0f;
    private int asteroidChance = 100;

    private float lastEnemy = 0.0f;
    private float enemyCooldown = 10.0f;
    private int enemyChance = 500;

    private void Awake()
    {
        // Get components of the player alongside the ui script and the engine particle emitter
        spawner = GetComponent<Spawner>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        compositeCollider = GetComponent<CompositeCollider2D>();
        ui = GameObject.Find("Camera/Canvas").GetComponent<Interface>();
        engineEmitter = transform.Find("EngineParticles").GetComponent<ParticleSystem>();
        engineEmitter.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check both the other collider and the player's collider exist
        if (compositeCollider == null || collision == null) { return; }

        switch (collision.gameObject.tag)
        {
            // If the player hits a planet or an asteroid, call the death method
            case ("Planet") or ("Asteroid"):
                Death();
                return;
            // If it's a wreck, destroy the wreck and add a random amount of survivors
            case ("Wreck"):
                survivors += Random.Range(10,20);
                Destroy(collision.gameObject);
                return;
            // If it's the station, convert the survivors to money, call the refill fuel method and open the shop.
            case ("Station"):
                money += survivors;
                survivors = 0;
                ui.OpenShop();
                RefillFuel();
                return;
            default:
                return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check both the other collider and the player's collider exist
        if (compositeCollider == null || collision == null) { return; }

        switch (collision.gameObject.tag)
        {
            // Refill fuel and close the shop when the player stops colloding
            case ("Station"):
                ui.CloseShop();
                RefillFuel();
                return;
            default:
                return;
        }
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    public void Death()
    {
        // Play the death animation, stops updating the ship and just loads the scene again
        animator.SetTrigger("Died");
        updateShip = false;
        SceneManager.LoadScene("GameScene");
    }

    public int GetSurvivors() => survivors;
    public int GetMoney() => money;
    public float GetFuel() => fuel;

    public void RefillFuel() { fuel = 100.0f; }
    public void AddMoney(int addedMoney) { money += addedMoney; }

    private void Update()
    {
        // Get the player's input with W providing a throttle output between 0 and 1, and inverting the horizontal axis for the ships rotation
        throttle = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
        rotation = -Input.GetAxis("Horizontal");

        // Boost the ship's speed
        if (Input.GetKey(KeyCode.LeftShift)) { throttle *= 2; }

        // If the player has their left mouse button down and the time between the last shot is greater than shotCooldown
        // create a projectile with the player's current location and rotation and set it's velocity to the player's current velocity
        if (Input.GetMouseButton(0) && t - lastShot > shotCooldown) {
            lastShot = t;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation, transform);
            Projectile script = projectile.GetComponent<Projectile>();
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            script.Creator = gameObject;
            rbProjectile.velocity = rb.velocity;
        }

        t += Time.deltaTime;

        // If theres no engine emitter, return
        // otherwise if the player has the throttle on, play the particles
        // finally if they dont, stop playing the particles
        if (!engineEmitter) { return; }
        if (throttle > 0.0f && !engineEmitter.isEmitting && updateShip && fuel > 0.0f) { engineEmitter.Play(); }
        else { engineEmitter.Stop(); }
    }

    private void FixedUpdate()
    {
        if (!rb) { return; };

        if(rb.velocity.magnitude > 10)
        {
            // If the time between last spawned asteroid past asteroidCooldown and the random number generated is less than 1
            // spawn an asteroid
            if (t - lastAsteroid > asteroidCooldown && Random.Range(1, asteroidChance) <= 1)
            {
                spawner.SpawnAsteroid();
                lastAsteroid = t;
            }

            // Same thing for the enemy 
            if (t - lastEnemy > enemyCooldown && Random.Range(1, enemyChance) <= 1)
            {
                spawner.SpawnEnemy();
                lastEnemy = t;
            }
        }

        // If the update ship variable is false, stop moving the ship
        // if there's no fuel, just return
        if (!updateShip) { rb.velocity = Vector3.zero; rb.totalTorque = 0; rb.freezeRotation = true; return; }
        if (fuel <= 0.0f) { return; }

        // Add the player's input to the ship and decrease the amount of fuel
        rb.AddRelativeForce(new Vector2(0, throttle * speed));
        rb.AddTorque(rotation * rotSpeed);
        fuel -= throttle / 30;

        // Finally, spawn a wreck if there isn't already one spawned and the player has 0 survivors on board
        if (survivors <= 0 && !spawner.SpawnedWreck)
        {
            spawner.SpawnWreck();
        }
    }
}
