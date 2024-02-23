using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject wreckPrefab;
    [SerializeField] GameObject asteroidPrefab;
    private GameObject player;
    private Rigidbody2D rbPlayer;

    public GameObject SpawnedWreck;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbPlayer = player.GetComponent<Rigidbody2D>();
    }

    public void SpawnWreck()
    {
        SpawnedWreck = Instantiate(wreckPrefab, new Vector3(Random.Range(-250, 250), Random.Range(-250, 250),0), Quaternion.identity);
    }
    public void SpawnAsteroid()
    {
        Instantiate(asteroidPrefab, player.transform.position + (new Vector3(rbPlayer.velocity.x, rbPlayer.velocity.y) * 10), Quaternion.identity);
    }
}
