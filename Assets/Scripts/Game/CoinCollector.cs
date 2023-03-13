
using UnityEngine;
using UnityEngine.Events;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private LayerMask _coinLayerMask;
    [SerializeField] private UnityEvent _collectedEvent = new();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_coinLayerMask.Contains(collider.gameObject.layer)) return;        
        _collectedEvent.Invoke();

        Destroy(collider.gameObject);
    }
}
