using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerCamera;
    private Rigidbody rb;
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    [Header("Look Settings")]
    public float lookSpeed = 15f;
    public float verticalLookLimit = 70f;
    [Header("Jump Settings")]
    public float jumpForce = 5f;

    private PlayerInput playerInput;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private float xRotation = 0f;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        InputSystem.actions.Disable();
        playerInput.currentActionMap?.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        float mouseX = lookDirection.x * lookSpeed * Time.deltaTime;
        float mouseY = lookDirection.y * lookSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }
}