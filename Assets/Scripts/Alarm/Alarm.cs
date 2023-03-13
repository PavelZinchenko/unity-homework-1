using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [Range(0.01f, 10f)][SerializeField] private float _volumeChangePerSec = 0.1f;
    [Range(0f, 1f)][SerializeField] private float _volumeMax = 1f;
    [Range(0f, 1f)][SerializeField] private float _volumeMin = 0f;

    [SerializeField] private UnityEvent _alarmStartedEvent = new();
    [SerializeField] private UnityEvent _alarmStoppedEvent = new();

    private bool _isActivated;
    private bool _isRunning;
    private AudioSource _audioSource;

    public void Activate(bool active)
    {
        _isActivated = active;

        if (_isActivated && !_isRunning)
            StartCoroutine(AlarmCoroutine());
    }

    private void OnValidate()
    {
        if (_volumeMin > _volumeMax)
        {
            var temp = _volumeMin;
            _volumeMin = _volumeMax;
            _volumeMax = temp;
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator AlarmCoroutine()
    {
        _isRunning = true;
        _audioSource.Play();
        _alarmStartedEvent.Invoke();

        float volume = _volumeMin;

        do
        {
            var delta = Time.deltaTime * _volumeChangePerSec;

            if (_isActivated && volume < _volumeMax)
                volume = Mathf.MoveTowards(volume, _volumeMax, delta);
            else if (!_isActivated && volume > _volumeMin)
                volume = Mathf.MoveTowards(volume, _volumeMin, delta);

            _audioSource.volume = volume;

            yield return null;
        }
        while (volume > _volumeMin);

        _isRunning = false;
        _audioSource.Stop();
        _alarmStoppedEvent.Invoke();
    }
}
