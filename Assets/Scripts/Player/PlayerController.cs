using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float lookSpeed = 25f;
    public float lookAngleLimit = 60f;
    public float xRotation = 0f;

    public float walkSpeed = 10f;
    public float crouchSpeed = 5f;
    private float gravity = 9.81f;

    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float transitionSpeed = 5f;
    private bool isCrouching = false;

    private CharacterController controller;
    private Camera playerCamera;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocityVector;

    public Transform playerMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLook();
        HandleMove();
        HandleCrouch();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
        }
    }

    public void HandleLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);

        xRotation -= lookInput.y * lookSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -lookAngleLimit, lookAngleLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void HandleMove()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isCrouching ? crouchSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocityVector.y < 0)
            velocityVector.y = -2f; // small downward force to keep grounded

        velocityVector.y -= gravity * Time.deltaTime;
        controller.Move(velocityVector * Time.deltaTime);
    }

    public void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, transitionSpeed * Time.deltaTime);
    }
}
