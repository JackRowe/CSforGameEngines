using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] float G = 1.0f;
    [SerializeField] Vector2 initialVelocity = Vector2.zero;

    private Rigidbody2D rb;
    private GameObject[] planets;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        planets = GameObject.FindGameObjectsWithTag("Planet");

        rb.AddForce(initialVelocity);
    }
    private void FixedUpdate()
    {
        if (rb == null) { return; }
        Vector2 gravityForce = new Vector2();

        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i] == null) { continue; };

            GameObject planet = planets[i];
            Vector2 distance = planet.transform.position - transform.position;
            float planetMass = (planet.transform.localScale.x * planet.transform.localScale.y);
            float force = G * ((rb.mass * planetMass) / ((float)Mathf.Pow(distance.magnitude, 2)));
            float velocity = force / rb.mass;
            distance.Normalize();
            gravityForce += distance * velocity;
        }

        rb.AddForce(gravityForce);
    }
}
