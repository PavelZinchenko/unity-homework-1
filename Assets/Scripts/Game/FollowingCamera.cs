using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [Range(1f, 100f)][SerializeField] private float _speed = 10f;

    private void Update()
    {
        var position = transform.localPosition;
        var newPosition = Vector3.Lerp(position, _target.localPosition + _offset, Time.deltaTime * _speed);
        newPosition.z = position.z;

        transform.localPosition = newPosition;
    }
}
