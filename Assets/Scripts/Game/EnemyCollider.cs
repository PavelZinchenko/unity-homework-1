using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Vector2 _direction = new Vector2(0, -1);
    [Range(0f,0.5f)][SerializeField] private float _tolerance = 0.1f;

    [SerializeField] private UnityEvent _successEvent = new();
    [SerializeField] private UnityEvent _failureEvent = new();

    private readonly List<ContactPoint2D> _contactPoints = new();

    private void OnCollisionStay2D(Collision2D collision)
    {
        ProcessCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision);
    }

    private void ProcessCollision(Collision2D collision)
    {
        if (!_enemyLayerMask.Contains(collision.gameObject.layer)) return;

        _contactPoints.Clear();
        collision.GetContacts(_contactPoints);

        var direction = _direction.normalized;

        foreach (var contact in _contactPoints)
            if (Vector2.Dot(contact.normal, direction) >= 1f - _tolerance)
                _successEvent.Invoke();
            else
                _failureEvent.Invoke();
    }
}
