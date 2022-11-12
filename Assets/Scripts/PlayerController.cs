using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float WalkSpeed;
    public float SprintSpeed;
    public float CrouchSpeed;

    public float GroundDrag;
    public float SlideDrag;

    public float JumpForce;
    public float JumpCooldown;
    public float AirMulitiplier;

    public Transform Orientation;

    public float PlayerHeight;
    public LayerMask Ground;

    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.LeftControl;

    private float horizontalInput;
    private float verticalInput;

    private Rigidbody rb;

    private bool grounded;

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

    [Header("Disc")]
    public GameObject projectile;
    public float ThrowForce;
    public float ThrowCooldown;
    public float SpinForce;

    public Vector3 BankAngle;

    public Transform CamOrientation;

    public bool AimLock;

    public PlayerCam cam;

    // Start is called before the first frame update
    void Start()
    {
        AimLock = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.1f, Ground);

        MyInput();
        SpeedControl();

        if (grounded)
        {
            jumpCount = 2;

            if (moveState == MovementState.Sliding)
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
        
        if (AimLock)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cam.XSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cam.YSens;

            BankAngle.x += mouseX * .01f;
            BankAngle.y += mouseY * .01f;

            BankAngle.x = Mathf.Clamp(BankAngle.x, -1, 1);
            BankAngle.y = Mathf.Clamp(BankAngle.y, -0.5f, 0.5f);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Throw()
    {
        Vector3 rotation = CamOrientation.rotation.eulerAngles;
        rotation.z -= BankAngle.x * 90; //bank = horizontal mouse
        rotation.x += BankAngle.y * 90; //pitch = vertical mouse
        Rigidbody disc = Instantiate(projectile, transform.position + CamOrientation.forward * 1.5f, Quaternion.Euler(rotation)).GetComponent<Rigidbody>();
        disc.AddForce(CamOrientation.forward * ThrowForce, ForceMode.Impulse);
        disc.AddTorque(disc.transform.up * SpinForce, ForceMode.Impulse);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(JumpKey) && canJump && jumpCount > 0)
        {
            canJump = false;
            jumpCount--;

            Jump();
        }

        if (grounded)
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AimLock = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            AimLock = false;
            Throw();
            BankAngle.Set(0, 0, 0);
        }
    }

    private void MovePlayer()
    {
        if (moveState == MovementState.Sliding)
        {
            return;
        }

        Vector3 moveForce = (Orientation.forward * verticalInput + Orientation.right * horizontalInput) * moveSpeed * 10f;
        if (grounded)
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
}
