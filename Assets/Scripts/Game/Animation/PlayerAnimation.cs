using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour, ICharacterAnimation
{
    [Range(0f,1f)][SerializeField] private float _speedThreshold = 0.1f;

    private bool _rightToLeft;
    private Animator _animator;

    private readonly int _speedX = Animator.StringToHash("speedX");
    private readonly int _speedY = Animator.StringToHash("speedY");
    private readonly int _grounded = Animator.StringToHash("grounded");
    private readonly int _hit = Animator.StringToHash("hit");
    private readonly int _die = Animator.StringToHash("die");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetVelocity(float runSpeedNormalized, float jumpSpeed, bool grounded)
    {
        transform.UpdateDirection(runSpeedNormalized, _speedThreshold, ref _rightToLeft);

        _animator.SetFloat(_speedY, jumpSpeed);
        _animator.SetFloat(_speedX, Mathf.Abs(runSpeedNormalized));
        _animator.SetBool(_grounded, grounded);
    }

    public void Hit()
    {
        _animator.SetTrigger(_hit);
    }

    public void Die()
    {
        _animator.SetTrigger(_die);
    }
}
