using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Handles player movement (e.g. walking, crouching)
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    public float lookSpeed = 25f;
    public float lookAngleLimit = 60f;
    public float xRotation = 0f;

    [Header("Speed Settings")]
    public float walkSpeed = 10f;
    public float crouchSpeed = 5f;

    private float gravity = 9.81f;

    [Header("Height Settings")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float transitionSpeed = 5f;
    private bool isCrouching = false;

    [Header("Component References")]
    private CharacterController controller;
    private Camera playerCamera;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocityVector;

    public Transform playerMesh;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        HandleLook();
        HandleMove();
        HandleCrouch();
    }

    // Called automatically by the Input System when look input is received
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    
    // Called automatically when a movement input value is produced (WASD/controller)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Called when the crouch input is triggered; context.preformed ensures it's only on press
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
        }
    }

    /// <summary>
    /// Rotates the player and camera based on stored look input. Should be called every frame.
    /// </summary>
    public void HandleLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);

        xRotation -= lookInput.y * lookSpeed * Time.deltaTime;

        // Prevent looking too far up/down to avoid unnatural camera rotation
        xRotation = Mathf.Clamp(xRotation, -lookAngleLimit, lookAngleLimit);

        // Transform player rotation using quaternion eulers
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    /// <summary>
    /// Move the player based on stored movement input. Should be called every frame.
    /// </summary>
    public void HandleMove()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isCrouching ? crouchSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded && velocityVector.y < 0)
        {
            velocityVector.y = -2f;
        }

        velocityVector.y -= gravity * Time.deltaTime;
        controller.Move(velocityVector * Time.deltaTime);
    }

    /// <summary>
    /// Smoothly transitions between standing and crouching height using interpolation.
    /// </summary>
    public void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, transitionSpeed * Time.deltaTime);
    }
}
