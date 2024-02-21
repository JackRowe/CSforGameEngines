using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject wreckPrefab;

    public GameObject SpawnedWreck;

    public void SpawnWreck()
    {
        SpawnedWreck = Instantiate(wreckPrefab, new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000),0), Quaternion.identity);
    }
}
