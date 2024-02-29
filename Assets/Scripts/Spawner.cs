using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject wreckPrefab;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject enemyPrefab;
    private GameObject player;
    private Rigidbody2D rbPlayer;

    public GameObject SpawnedWreck;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbPlayer = player.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Spawns a new wreck at a random location
    /// </summary>
    public void SpawnWreck()
    {
        SpawnedWreck = Instantiate(wreckPrefab, new Vector3(Random.Range(-250, 250), Random.Range(-250, 250),0), Quaternion.identity);
    }

    /// <summary>
    /// Spawns an asteroid infront of the player's veloicty, just offscreen with some variation
    /// </summary>
    public void SpawnAsteroid()
    {
        Instantiate(asteroidPrefab, player.transform.position + (new Vector3(rbPlayer.velocity.x + Random.Range(-1, 1), rbPlayer.velocity.y) * 10), Quaternion.identity);
    }

    /// <summary>
    /// Spawns an enemy infront of the player's velocity, just offscreen with some variation
    /// </summary>
    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, player.transform.position + (new Vector3(rbPlayer.velocity.x + Random.Range(-100, 100), rbPlayer.velocity.y) * 10), Quaternion.identity);
    }
}
