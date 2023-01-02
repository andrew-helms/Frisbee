using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;

    [SerializeField] private float groundDrag;
    [SerializeField] private float slideDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMulitiplier;

    [SerializeField] private Transform orientation;

    [SerializeField] private float playerHeight;
    [SerializeField] private float playerWidth;
    [SerializeField] private LayerMask ground;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [SerializeField] private bool toggleSprint;

    [SerializeField] private RunManager runManager;
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
        moveSpeed = walkSpeed;
        moveState = MovementState.Walking;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);

        if (!grounded)
        {
            onWall = Physics.Raycast(transform.position, orientation.transform.right, playerWidth * 0.5f + 0.1f, ground);
            if (!onWall)
            {
                onWall = Physics.Raycast(transform.position, -orientation.transform.right, playerWidth * 0.5f + 0.1f, ground);
            }
        }

        MyInput();
        SpeedControl();

        if (onWall && !grounded)
        {
            rb.AddForce(transform.up * 1f, ForceMode.Acceleration);
        }

        if (grounded || onWall)
        {
            jumpCount = 2;

            if ((moveState == MovementState.Sliding || onWall) && !throwManager.HoldingDisc)
            {
                rb.drag = slideDrag;
            }
            else
            {
                rb.drag = groundDrag;
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
        if (runManager.Paused)
        {
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && jumpCount > 0 && !throwManager.HoldingDisc)
        {
            canJump = false;
            jumpCount--;

            Jump();
        }

        if (grounded || onWall)
        {
            if (!toggleSprint)
            {
                moveSpeed = walkSpeed;
                moveState = MovementState.Walking;
            }

            if (Input.GetKeyDown(sprintKey))
            {
                if (moveState == MovementState.Sprinting && toggleSprint)
                {
                    moveSpeed = walkSpeed;
                    moveState = MovementState.Walking;
                }
                else
                {
                    moveSpeed = sprintSpeed;
                    moveState = MovementState.Sprinting;
                }
            }

            if (moveState == MovementState.Sprinting && rb.velocity.magnitude < crouchSpeed + 0.5f)
            {
                moveSpeed = walkSpeed;
                moveState = MovementState.Walking;
            }

            if (Input.GetKey(crouchKey))
            {
                moveSpeed = crouchSpeed;
                moveState = MovementState.Crouching;

                if (rb.velocity.magnitude > crouchSpeed + 0.5f)
                {
                    moveSpeed = sprintSpeed;
                    moveState = MovementState.Sliding;
                }
            }
        }
        else
        {
            //moveSpeed = SprintSpeed;
            if (Input.GetKey(crouchKey) && rb.velocity.magnitude > crouchSpeed + 0.5f)
            {
                moveState = MovementState.Sliding;
            }
        }

        if (!canJump)
        {
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (moveState == MovementState.Sliding || throwManager.HoldingDisc)
        {
            return;
        }

        Vector3 moveForce = (orientation.forward * verticalInput + orientation.right * horizontalInput) * moveSpeed * 6f;
        if (grounded || onWall)
        {
            rb.AddForce(moveForce, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveForce * airMulitiplier, ForceMode.Acceleration);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (moveState != MovementState.Sliding && flatVel.magnitude > moveSpeed)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * rb.mass, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "Disc(Clone)")
    //    {
    //        Destroy(collision.gameObject);
    //        throwManager.PickUpDisc();
    //    }
    //}
}
