using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMovement : MonoBehaviour
{
    public float LiftCoeff;
    public float DragCoeff;

    public LayerMask Ground;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = Mathf.Epsilon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
        float angle = Mathf.Atan2(-localVel.y, localVel.z);

        float lift = LiftCoeff * Mathf.Sin(angle) * (Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2));
        float drag = -DragCoeff * Mathf.Sin(angle) * Mathf.Pow(rb.velocity.magnitude, 2);
        rb.AddForce(Vector3.Cross(rb.velocity, transform.right).normalized * lift);
        rb.AddForce(rb.velocity.normalized * drag);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != Ground.value)
        {
            //Destroy(gameObject);
        }
    }
}
