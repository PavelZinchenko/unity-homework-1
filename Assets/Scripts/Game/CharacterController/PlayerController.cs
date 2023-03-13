using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<float>();
        _characterController.Move(direction);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        var jump = context.ReadValueAsButton();
        _characterController.Jump(jump);
    }

    public void OnGetHit()
    {
        _characterController.TryGetHit();
    }

    public void OnHitEnemy()
    {
        _characterController.Bounce();
    }
}
