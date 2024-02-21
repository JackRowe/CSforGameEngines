using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject wreckPrefab;

    public GameObject SpawnedWreck;

    public void SpawnWreck()
    {
        SpawnedWreck = Instantiate(wreckPrefab, new Vector3(Random.Range(-250, 250), Random.Range(-250, 250),0), Quaternion.identity);
    }
}
