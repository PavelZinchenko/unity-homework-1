using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ICharacterAnimation))]
public class CharacterController : MonoBehaviour
{
    [Range(1, 100)][SerializeField] private float _movementSpeed = 10f;
    [Range(1, 500)][SerializeField] private float _jumpForce = 10f;
    [Range(1, 100)][SerializeField] private float _bounceForce = 10f;
    [Range(10f, 1000f)][SerializeField] private float _acceleration = 500f;
    [Range(0.1f, 5f)][SerializeField] private float _jumpCooldown = 0.2f;
    [Range(0f, 3f)][SerializeField] private float _invincibilityTime = 0.5f;
    [Range(0f, 10f)][SerializeField] private float _corpseDisappearTime = 5f;
    [SerializeField] private bool _airControl = false;

    [SerializeField] private UnityEvent _jumpEvent = new();
    [SerializeField] private UnityEvent _hitEvent = new();

    private bool _jump;
    private bool _bounce;
    private float _lastHitTime;
    private float _movementDirection;
    private float _lastJumpTime;
    private Rigidbody2D _rigidbody2D;
    private GroundDetector _groundDetector;
    private ICharacterAnimation _animation;

    public void Move(float speed)
    {
        _movementDirection = speed;
    }

    public void Jump(bool pressed)
    {
        _jump = pressed;
    }

    public void Bounce()
    {
        _bounce = true;
    }

    public void Die()
    {
        _rigidbody2D.simulated = false;
        _animation.Die();

        StartCoroutine(WaitThenDestroy(_corpseDisappearTime));
    }

    private IEnumerator WaitThenDestroy(float totalSeconds)
    {
        if (TryGetComponent<SpriteRenderer>(out var renderer))
        {
            var color = renderer.color;
            float timeLeft = totalSeconds;
            while (timeLeft > 0)
            {
                renderer.color = new Color(color.r, color.g, color.b, color.a * timeLeft / totalSeconds);
                timeLeft -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(totalSeconds);
        }

        Destroy(gameObject);
    }

    public bool TryGetHit()
    {
        if (Time.time - _lastHitTime < _invincibilityTime) return false;
        _lastHitTime = Time.time;
        _animation.Hit();
        _hitEvent.Invoke();
        return true;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animation = GetComponent<ICharacterAnimation>();

        // some characters don't need it
        if (!TryGetComponent(out _groundDetector))
            _groundDetector = null;
    }

    private void FixedUpdate()
    {
        var velocity = _rigidbody2D.velocity;
        var targetVelocity = new Vector2(_movementDirection * _movementSpeed, velocity.y);
        var grounded = _groundDetector ? _groundDetector.IsGrounded : true;

        if (_jump && grounded && Time.fixedTime - _lastJumpTime >= _jumpCooldown)
        {
            velocity.y = -_jumpForce;
            _lastJumpTime = Time.fixedTime;
            _jumpEvent.Invoke();
        }

        if (_bounce)
        {
            velocity.y = Mathf.Min(velocity.y, -_bounceForce);
            _lastJumpTime = Time.fixedTime;
            _bounce = false;
        }

        if (_airControl || grounded)
            _rigidbody2D.AddForce((targetVelocity - velocity) * Time.fixedDeltaTime * _acceleration);

        _animation.SetVelocity(velocity.x / _movementSpeed, velocity.y, grounded);
    }
}
