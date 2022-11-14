using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscMovement : MonoBehaviour
{
    [SerializeField] private float LiftCoeff;
    [SerializeField] private float StaticLiftCoeff;

    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Goal;
    [SerializeField] private LayerMask Bounds;

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

        float lift = (LiftCoeff * -localVel.y + StaticLiftCoeff) * Mathf.Pow(rb.velocity.magnitude, 2);
        rb.AddForce(transform.up * lift);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((Goal.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            throwManager.Reset();
        }
        if ((Bounds.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
