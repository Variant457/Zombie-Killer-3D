using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    public float moveSpeed = 5f;
    public float lookSpeed = 5f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        InputSystem.actions.Disable();
        playerInput.currentActionMap?.Enable();
    }

    void Update()
    {
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        Vector3 lookMovement = new Vector3(lookDirection.x, 0, lookDirection.y);
        transform.Rotate(lookMovement * lookSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }
}