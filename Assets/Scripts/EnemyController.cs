using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _destroyWhenYBelow = -10;
    [Range(0,10)][SerializeField] private int _lives = 2;

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

        var player = GameObject.FindGameObjectWithTag(_playerTag);

        if (player)
            _direction = player.transform.localPosition.x > transform.localPosition.x ? 1f : -1f;
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

            if (x > MinObstacleNormalX) _direction = 1;
            if (x < -MinObstacleNormalX) _direction = -1;
        }
    }

    private void Update()
    {
        _characterController.Move(_direction);
        
        if (transform.localPosition.y < _destroyWhenYBelow) 
            Destroy(gameObject);
    }

    private float _direction;
    private float _lastHitTime;
    private CharacterController _characterController;

    private const float MinObstacleNormalX = 0.75f;
}
