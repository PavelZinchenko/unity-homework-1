using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _activatedEvent = new();
    [SerializeField] private UnityEvent _deactivatedEvent = new();

    private bool _isActive;
    private readonly HashSet<GameObject> _intruders = new();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _intruders.Add(collider.gameObject);
        SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        _intruders.Remove(collider.gameObject);
        SetActive(_intruders.Count > 0);
    }

    private void SetActive(bool isActive)
    {
        if (_isActive == isActive) return;
        _isActive = isActive;

        if (_isActive)
            _activatedEvent.Invoke();
        else
            _deactivatedEvent.Invoke();
    }
}
