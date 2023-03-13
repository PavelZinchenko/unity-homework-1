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
    private bool _isCoroutineRunning;
    private AudioSource _audioSource;

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

    public void Activate(bool active)
    {
        _isActivated = active;

        if (_isActivated && !_isCoroutineRunning)
            StartCoroutine(StartAlarm());
    }

    private IEnumerator StartAlarm()
    {
        if (_isCoroutineRunning) yield break;

        _isCoroutineRunning = true;
        _audioSource.Play();
        _alarmStartedEvent.Invoke();

        float volume = _volumeMin;

        do
        {
            var delta = Time.deltaTime * _volumeChangePerSec;
            volume = Mathf.MoveTowards(volume, _isActivated ? _volumeMax : _volumeMin, delta);
            _audioSource.volume = volume;
            yield return null;
        }
        while (volume > _volumeMin);

        _isCoroutineRunning = false;
        _audioSource.Stop();
        _alarmStoppedEvent.Invoke();
    }
}
