using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float moveSpeedMultiplier;
    [SerializeField] float superSpeedMultiplier;
    [SerializeField] float walkSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashSpeedChangeFactor;

    public float maxYSpeed;

    [SerializeField] float groundDrag;

    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayers;
    bool grounded;

    [Header("Slope Handler")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [SerializeField] Transform orientation;
    
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    Rigidbody rb;
    Player p;

    [SerializeField] MovementState state;
    public enum MovementState
    {
        walking,
        dashing,
        superarmor,
        air
    }

    public bool dashing;
    public bool deflecting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        p = GetComponent<Player>();
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (p.hasControl)
        {
            GroundCheck();
            GatherInput();
            SpeedControl();
            StateHandler();
        }
    }

    private void FixedUpdate()
    {
        if (p.hasControl)
            MovePlayer();
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayers);
        if (state == MovementState.walking)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, groundLayers))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void GatherInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && state != MovementState.dashing)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        if (deflecting)
        {
            state = MovementState.superarmor;
            desiredMoveSpeed = 0f;
        }
        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }   
        else
        {
            state = MovementState.air;
            desiredMoveSpeed = walkSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference) 
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        if (state == MovementState.dashing || state == MovementState.superarmor) return;

        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * moveSpeedMultiplier * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        //on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * moveSpeedMultiplier * 10f, ForceMode.Force);
        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * moveSpeedMultiplier * airMultiplier * 10f, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (p.superSpeed) moveSpeedMultiplier = superSpeedMultiplier;
        else moveSpeedMultiplier = 1f;

        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed * moveSpeedMultiplier)
                rb.velocity = rb.velocity.normalized * moveSpeed * moveSpeedMultiplier;
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if (flatVelocity.magnitude > moveSpeed * moveSpeedMultiplier)
            {
                Vector3 limitVel = flatVelocity.normalized * moveSpeed * moveSpeedMultiplier;
                rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
            }
        }

        //limit y velocity
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }

    private void Jump()
    {
        exitingSlope = true;
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }    

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public void SetMoveSpeedMultiplier(float mult) { moveSpeedMultiplier = mult; }

    public float GetMoveSpeedMultiplier() { return moveSpeedMultiplier; }

    public MovementState GetMovementState()
    {
        return state;
    }

    public Transform GetOrientation()
    {
        return orientation;
    }
}
