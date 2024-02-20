using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject wreckPrefab;

    public GameObject SpawnedWreck;

    public void SpawnWreck()
    {
        SpawnedWreck = Instantiate(wreckPrefab, transform);
    }
}
