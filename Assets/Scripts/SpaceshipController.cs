using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CompositeCollider2D compositeCollider;
    private ParticleSystem engineEmitter;
    private Animator animator;

    [SerializeField] float speed = 2.0f;
    [SerializeField] float rotSpeed = 4.0f;
    private float throttle = 0.0f;
    private float rotation = 0.0f;
    private bool updateShip = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        compositeCollider = GetComponent<CompositeCollider2D>();
        engineEmitter = transform.Find("EngineParticles").GetComponent<ParticleSystem>();
        engineEmitter.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (compositeCollider == null || collision == null) { return; }

        switch (collision.gameObject.tag)
        {
            case ("Planet"):
                Death();
                return;
            default:
                return;
        }
    }

    private void Death()
    {
        animator.SetTrigger("Died");
        updateShip = false;
    }

    private void Update()
    {
        throttle = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
        rotation = -Input.GetAxis("Horizontal");

        if (!engineEmitter) { return; }
        if (throttle > 0.0f && !engineEmitter.isEmitting && updateShip) { engineEmitter.Play(); }
        else if(throttle <= 0.0f) { engineEmitter.Stop(); }
    }

    private void FixedUpdate()
    {
        if (!rb) { return; };
        if (!updateShip) { rb.velocity = Vector3.zero; rb.totalTorque = 0; rb.freezeRotation = true; return; }
        rb.AddRelativeForce(new Vector2(0, throttle * speed));
        rb.AddTorque(rotation * rotSpeed);
    }
}
