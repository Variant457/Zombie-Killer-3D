using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    private Vector2 moveDirection;
    public float moveSpeed = 5f;

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
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
}