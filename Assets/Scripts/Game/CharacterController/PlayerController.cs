using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Range(0.1f, 3f)][SerializeField] private float _invincibilityTime = 0.5f;

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

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private CharacterController _characterController;
}
