using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;

    private readonly List<ContactPoint2D> _contactPoints = new();
    private readonly HashSet<GameObject> _ground = new();

    private const float MinGroundNormalY = 0.9f;

    public bool IsGrounded => _ground.Count > 0;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsGround(collision))
            _ground.Add(collision.gameObject);
        else
            _ground.Remove(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGround(collision))
            _ground.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _ground.Remove(collision.gameObject);
    }

    private bool IsGround(Collision2D collision)
    {
        if (!_groundLayerMask.Contains(collision.gameObject.layer))
            return false;

        _contactPoints.Clear();
        collision.GetContacts(_contactPoints);
        foreach (var contact in _contactPoints)
            if (contact.normal.y >= MinGroundNormalY)
                return true;

        return false;
    }
}
