using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscMovement : MonoBehaviour
{
    //[SerializeField] private float LiftCoeff;
    [SerializeField] private float cd0 = 0.01f;
    [SerializeField] private float A = 0.5f;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask goal;
    [SerializeField] private LayerMask bounds;

    [SerializeField] private AnimationCurve liftCurve;

    [SerializeField] private ThrowManager throwManager;

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
        float velocity = localVel.magnitude;

        if (velocity < 0.01f)
        {
            return;
        }

        //rb.AddForce(Vector3.up * -9.81f * rb.mass, ForceMode.Force);

        Vector3 localNorm = localVel.normalized;
        float angleOfAttack = -Mathf.Asin(localVel.y / velocity);
        float referenceArea = 0.003f + Mathf.Abs(localNorm.y) * (0.07068f - 0.003f);

        //float cl = 3 * angleOfAttack * (Mathf.Abs(angleOfAttack) < 0.1745 ? 1f : Mathf.Abs(angleOfAttack) < 0.1832596 ? 0.5f : Mathf.Abs(angleOfAttack) < 0.191986 ? 0.25f : 0f);
        float cl = liftCurve.Evaluate(angleOfAttack);
        float lift = cl * 0.5f * 1.225f * Mathf.Pow(velocity, 2) * 0.07068f;
        float drag = 0.5f * 1.225f * Mathf.Pow(velocity, 2) * referenceArea * (cd0 + Mathf.Pow(cl, 2) * A);

        if (Physics.Raycast(transform.position, Vector3.down, 0.3f, ground))
        {
            drag /= 2;
        }

        rb.AddForce(transform.up * lift, ForceMode.Force);
        rb.AddForce(localNorm * -drag, ForceMode.Force);

        Debug.DrawRay(rb.position, transform.up * lift, Color.blue);
        Debug.DrawRay(rb.position, rb.velocity.normalized * -drag, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((goal.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
            throwManager.Reset();
            SceneManager.LoadScene(0);
        }
        if ((bounds.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
