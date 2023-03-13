using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour, ICharacterAnimation
{
    [Range(0f,1f)][SerializeField] private float _speedThreshold = 0.1f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetVelocity(float runSpeedNormalized, float jumpSpeed, bool grounded)
    {
        transform.UpdateDirection(runSpeedNormalized, _speedThreshold, ref _rightToLeft);

        _animator.SetFloat("speedY", jumpSpeed);
        _animator.SetFloat("speedX", Mathf.Abs(runSpeedNormalized));
        _animator.SetBool("grounded", grounded);
    }

    public void Hit()
    {
        _animator.SetTrigger("hit");
    }

    public void Die()
    {
        _animator.SetTrigger("die");
    }

    private bool _rightToLeft;
    private Animator _animator;
}
