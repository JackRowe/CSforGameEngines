using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] float scale = 1.0f;

    GameObject target;
    Material material;

    private void Awake()
    {
        // Get the target (player) and material
        target = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    private void Update()
    {
        // If there is a target and a material, update the material's offset with the player's position divided by the scale variable
        // This emulates a parallax effect by slowly scrolling the background material depending on the scale variable
        if (material == null || target == null) { return; }
        material.SetVector("_Offset", target.transform.position / scale);
    }
}
