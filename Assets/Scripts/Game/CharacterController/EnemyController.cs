using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _destroyWhenYBelow = -10;
    [Range(0,10)][SerializeField] private int _lives = 2;

    private float _moveDirection;
    private CharacterController _characterController;

    private const float MinObstacleNormalX = 0.75f;
    private const float Speed = 1.0f;

    public void SetTarget(Transform target)
    {
        _moveDirection = target.localPosition.x > transform.localPosition.x ? Speed : -Speed;
    }

    public void OnGetHit()
    {
        if (!_characterController.TryGetHit()) return;

        if (--_lives < 0)
            _characterController.Die();
    }

    public void OnHitPlayer()
    {
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _moveDirection = transform.localScale.x > 0 ? Speed : -Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessObstacles(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ProcessObstacles(collision);
    }

    private void ProcessObstacles(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; ++i)
        {
            var contactPoint = collision.GetContact(i);
            var x = contactPoint.normal.x;

            if (x > MinObstacleNormalX) _moveDirection = Speed;
            if (x < -MinObstacleNormalX) _moveDirection = -Speed;
        }
    }

    private void Update()
    {
        _characterController.Move(_moveDirection);
        
        if (transform.localPosition.y < _destroyWhenYBelow) 
            Destroy(gameObject);
    }
}
