using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float CrouchSpeed;

    [SerializeField] private float GroundDrag;
    [SerializeField] private float SlideDrag;

    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpCooldown;
    [SerializeField] private float AirMulitiplier;

    [SerializeField] private Transform Orientation;

    [SerializeField] private float PlayerHeight;
    [SerializeField] private float PlayerWidth;
    [SerializeField] private LayerMask Ground;

    [SerializeField] private KeyCode JumpKey = KeyCode.Space;
    [SerializeField] private KeyCode SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode CrouchKey = KeyCode.LeftControl;

    [SerializeField] private ThrowManager throwManager;

    private float horizontalInput;
    private float verticalInput;

    private Rigidbody rb;

    private bool grounded;
    private bool onWall;

    private bool canJump;
    private int jumpCount = 2;

    private float moveSpeed;

    private MovementState moveState;

    enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Sliding
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.1f, Ground);

        if (!grounded)
        {
            onWall = Physics.Raycast(transform.position, Orientation.transform.right, PlayerWidth * 0.5f + 0.1f, Ground);
            if (!onWall)
            {
                onWall = Physics.Raycast(transform.position, -Orientation.transform.right, PlayerWidth * 0.5f + 0.1f, Ground);
            }
        }

        MyInput();
        SpeedControl();

        if (grounded || onWall)
        {
            jumpCount = 2;

            if ((moveState == MovementState.Sliding || onWall) && !throwManager.HoldingDisc)
            {
                rb.drag = SlideDrag;
            }
            else
            {
                rb.drag = GroundDrag;
            }
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(JumpKey) && canJump && jumpCount > 0 && !throwManager.HoldingDisc)
        {
            canJump = false;
            jumpCount--;

            Jump();
        }

        if (grounded || onWall)
        {
            moveSpeed = WalkSpeed;
            moveState = MovementState.Walking;

            if (Input.GetKey(SprintKey))
            {
                moveSpeed = SprintSpeed;
                moveState = MovementState.Sprinting;
            }

            if (Input.GetKey(CrouchKey))
            {
                moveSpeed = CrouchSpeed;
                moveState = MovementState.Crouching;

                if (rb.velocity.magnitude > CrouchSpeed)
                {
                    moveSpeed = SprintSpeed;
                    moveState = MovementState.Sliding;
                }
            }
        }
        else
        {
            //moveSpeed = SprintSpeed;
            if (Input.GetKey(CrouchKey) && rb.velocity.magnitude > CrouchSpeed + 0.5f)
            {
                moveState = MovementState.Sliding;
            }
        }

        if (!canJump)
        {
            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (moveState == MovementState.Sliding || throwManager.HoldingDisc)
        {
            return;
        }

        Vector3 moveForce = (Orientation.forward * verticalInput + Orientation.right * horizontalInput) * moveSpeed * 5f;
        if (grounded || onWall)
        {
            rb.AddForce(moveForce, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveForce * AirMulitiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (moveState != MovementState.Sliding && rb.velocity.magnitude > moveSpeed)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Disc(Clone)")
        {
            Destroy(collision.gameObject);
            throwManager.PickUpDisc();
        }
        Debug.Log(collision.gameObject.name);
    }
}
