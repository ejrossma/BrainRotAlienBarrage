using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    [SerializeField] float dashForce;
    [SerializeField] float dashUpwardForce;
    [SerializeField] float maxDashYSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashDelay = 0.025f;
    private Vector3 delayedForceToApply;

    [Header("CameraEffects")]
    [SerializeField] CameraController cc;
    [SerializeField] float dashFov;

    [Header("Settings")]
    [SerializeField] bool useCameraForward = true;
    [SerializeField] bool allowAllDirections = true;
    [SerializeField] bool disableGravity = false;
    [SerializeField] bool resetVel = true;

    [Header("Cooldown")]
    public float dashCooldown;
    float dashCooldownTimer;

    [Header("Input")]
    [SerializeField] KeyCode dashKey = KeyCode.LeftShift;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(dashKey)) 
        {
            Dash();
        }

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        cc.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), dashDelay);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0;

        cc.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }

    public float GetDashCooldown()
    {
        return dashCooldownTimer;
    }
}
