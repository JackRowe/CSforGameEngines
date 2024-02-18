using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CompositeCollider2D collider;
    private ParticleSystem engineEmitter;

    [SerializeField] float speed = 2.0f;
    [SerializeField] float rotSpeed = 4.0f;
    private float throttle = 0.0f;
    private float rotation = 0.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CompositeCollider2D>();
        engineEmitter = transform.Find("EngineParticles").GetComponent<ParticleSystem>();
        engineEmitter.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collider == null || collision == null) { return; }

        switch (collision.gameObject.tag)
        {
            case("Planet"):
                Debug.Log("destroy");
                return;
            default:
                Debug.Log("nothing");
                return;
        }
    }

    private void Update()
    {
        throttle = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
        rotation = -Input.GetAxis("Horizontal");

        if (!engineEmitter) { return; }
        if (throttle > 0.0f && !engineEmitter.isEmitting) { engineEmitter.Play(); }
        else if(throttle <= 0.0f) { engineEmitter.Stop(); }
    }

    private void FixedUpdate()
    {
        if (!rb) { return; };
        rb.AddRelativeForce(new Vector2(0, throttle * speed));
        rb.AddTorque(rotation * rotSpeed);
    }
}
