using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;

    [SerializeField] private float groundDrag;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeedMulti = 1;

    private bool isSprinting = false;
    /*
    [SerializeField] private float maxStaminaAmount;
    float staminaAmount;*/

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float playerHeight;
    bool grounded;

    [SerializeField] private Transform orientation;

    Rigidbody rb;

    Vector2 moveInput;

    StaminaController staminaController;

    void Start()
    {
        staminaController = GetComponent<StaminaController>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MovePlayer();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    void MovePlayer()
    {
        Vector3 moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        if (isSprinting )
        {
            rb.AddForce(moveDirection * movementSpeed * 10f * sprintSpeedMulti, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection * movementSpeed * 10f, ForceMode.Force);
        }

        SpeedControl();
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float currentSpeed = isSprinting ? sprintSpeedMulti * movementSpeed : movementSpeed;

        Debug.Log(currentSpeed);

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void SetSprintingBool(bool value)
    {
        isSprinting = value;
    }

    #region new input methodes
    public void OnMove(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.phase != InputActionPhase.Started)
        {
            if (value.ReadValue<float>() >= 1f)
            {
                staminaController.CheckCanSprint();
            }
            else
            {
                staminaController.StopSprinting();
            }
        }
    }
    #endregion
}