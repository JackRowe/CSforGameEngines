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
        target = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    private void Update()
    {
        if (material == null || target == null) { return; }
        material.SetVector("_Offset", target.transform.position / scale);
    }
}
