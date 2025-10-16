using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //general thought process get the center of the player
    //when calculatting for slope use it check for slope when fully up and crotching
    #region "Variables"
    [SerializeField] private Transform playerCenterpoint;
    [SerializeField] private Transform playerGeneralCollider;
    //[SerializeField] private Transform playerHurtBox;
    [SerializeField] private Transform playerModel;

    [Header("State Speeds")]
    //Determines the Speeds of walking and sprint 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Player Velocity")]
    [SerializeField] private Vector3 playerVelocity;

    [Header("Crouching Values")]
    //Determines the Speed of crouch

    [SerializeField] private float crouchYScale;
    [SerializeField] private float startYScale;

    [Header("Bool Values")]
    public bool isRestricted;
    public bool isSliding;
    public bool isWallRunning;

    [Header("Applied Forces")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool readyToJump;

    //Ground Check
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Check")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] RaycastHit slopeHit;
    [SerializeField] private bool exitingSlope;
    [SerializeField] private bool currentlyOnSlope;

    //Current orientation of the camera
    [SerializeField] private Transform orientation;

    [SerializeField] private float verticalInput;
    [SerializeField] private float horizontalInput;

    [SerializeField]
    private enum MovementState
    {
        restricted,
        walking,
        sprinting,
        crouching,
        air
    }
    [SerializeField] private MovementState state;

    //Gets the input values from input Controller
    private Vector3 moveInput;
    private float jumpInput;
    private float sprintInput;
    private float crouchInput;

    //The direction the player moves in
    [SerializeField] private Vector3 moveDirection;

    private Rigidbody rb;

    #endregion

    public Vector3 MoveInput
    {
        get
        {
            return moveInput;
        }
        set
        {
            moveInput = value;
        }
    }

    public float JumpInput
    {
        get
        {
            return jumpInput;
        }
        set
        {
            jumpInput = value;
        }
    }

    public float SprintInput
    {
        get
        {
            return sprintInput;
        }
        set
        {
            sprintInput = value;
        }
    }

    public float CrouchInput
    {
        get
        {
            return crouchInput;
        }
        set
        {
            crouchInput = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        if(state != MovementState.restricted)
        {
            MovePlayer();
        }
        
        //debug value
        playerVelocity = rb.linearVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks Ground
        //Debug.Log(Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround));
        grounded = Physics.Raycast(playerCenterpoint.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        SpeedLimiter();

        UpDateInput();

        StateHandler();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    /// <summary>
    /// takes
    /// </summary>
    private void UpDateInput()
    {
        verticalInput = moveInput.y;
        horizontalInput = moveInput.x;

        if (jumpInput == 1 && readyToJump && grounded)
        {
            Jump();

            readyToJump = false;

            Invoke(nameof(JumpReset), jumpCooldown);

            jumpInput = 0;
        }

        currentlyOnSlope = OnSlope();

        Crouch();
    }

    /// <summary>
    /// The movement State handler for the player that changes the current state of the player based on
    /// a varaiety of connected bools, and inputs
    /// </summary>
    private void StateHandler()
    {
        if (isRestricted)
        {
            state = MovementState.restricted;
        }

        // Crouching overrides walk/sprint
        if (crouchInput == 1)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Sprinting
        else if (grounded && sprintInput == 1)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Air
        else if (!isWallRunning) 
        {
            state = MovementState.air;
        }

        // Smooth speed transition
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    /// <summary>
    /// 
    /// </summary>
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!isWallRunning)
        {
            rb.useGravity = !OnSlope();
        }
        //This helps the players movement when walking down a slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 25f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        /*
        //This allows the player to move up slopes, with a similar speed to flat ground,
        else if (OnSlope() && crouchInput == 1)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * (moveSpeed * 2.0f) * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        */
        //Base movement when on the ground and not on a slope
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //allows movement when the player is in the air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

    }

    private void SpeedLimiter()
    {
        //
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        //
        else if (OnSlope() && crouchInput == 1)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        //
        else
        {
            Vector3 flatVal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVal.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVal.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }

    }

    private void Jump()
    {
        exitingSlope = true;

        //Resets Y velocity, makes sure Y velocity is 0
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void JumpReset()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void Crouch()
    {
        if (crouchInput == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (OnSlope())
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * (moveSpeed * 2.0f) * 20f, ForceMode.Force);

                if (rb.linearVelocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
            else
            {
                rb.MovePosition(rb.position + Vector3.down * 0.03f);
            }
        }

        if (crouchInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void Tilt()
    {

    }

    public bool OnSlope()
    {
        if (Physics.Raycast(playerCenterpoint.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }



    private void OnDrawGizmos()
    {
        //Physics.Raycast(playerCenterpoint.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (Physics.Raycast(playerCenterpoint.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.3f))
        {
            // Draw the ray to the hit point
            Gizmos.color = Color.green;
            Gizmos.DrawRay(playerCenterpoint.position, Vector3.down * hit.distance);

            // Draw a sphere at the hit point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.2f);
        }
        else
        {
            // Draw the full-length ray if no hit
            Gizmos.color = Color.green;
            Gizmos.DrawRay(playerCenterpoint.position, Vector3.down * 5);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerCenterpoint.position, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerCenterpoint.position, playerHeight);
    }
}
