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
        // Get the rigid body and all planets 
        rb = GetComponent<Rigidbody2D>();
        planets = GameObject.FindGameObjectsWithTag("Planet");

        rb.AddForce(initialVelocity);
    }
    private void FixedUpdate()
    {
        // Make sure the rigid body exists before creating the vector to be applied at the end of the update
        if (rb == null) { return; }
        Vector2 gravityForce = new Vector2();

        // Loop through each planet, ensuring it exists
        // then I create the force applies to the object using newtons gravitational formula: Force = (Gravity * Mass 1 * Mass 2) / distance^2
        // then using newtons 2nd law: Force = mass * acceleration, I get the acceleration for the object bt dividing force by the object's mass
        // this is then applied to the overall gravity force and then applied to the rigidbody
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i] == null) { continue; };

            GameObject planet = planets[i];
            Vector2 distance = planet.transform.position - transform.position;
            float planetMass = (planet.transform.localScale.x * planet.transform.localScale.y);
            float force = G * ((rb.mass * planetMass) / ((float)Mathf.Pow(distance.magnitude, 2))); // Force = (Gravity * Mass 1 * Mass 2) / distance^2
            float acceleration = force / rb.mass;
            distance.Normalize();
            gravityForce += distance * acceleration;
        }

        rb.AddForce(gravityForce);
    }
}
