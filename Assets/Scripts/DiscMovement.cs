using UnityEngine;

public class DiscMovement : MonoBehaviour
{
    public float LiftCoeff;
    public float StaticLiftCoeff;

    public LayerMask Ground;
    public LayerMask Goal;
    public LayerMask Bounds;

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

        float lift = (LiftCoeff * -localVel.y + StaticLiftCoeff) * Mathf.Pow(rb.velocity.magnitude, 2);
        rb.AddForce(transform.up * lift);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((Ground.value & (1 << collision.transform.gameObject.layer)) == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((Goal.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
            Debug.Log("Score");
        }
        if ((Bounds.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
