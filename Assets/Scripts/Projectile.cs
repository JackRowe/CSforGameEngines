using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Creator;
    private Rigidbody2D rb;

    [SerializeField] int speed = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.AddRelativeForce(new Vector2(0, speed));
        //transform.rotation = .up);
    }

}
