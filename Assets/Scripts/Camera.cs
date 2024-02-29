using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        // Make the camera follow the player
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
