using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [Range(0.01f,10f)][SerializeField] private float _timeMultiplier = 0.1f;
    [SerializeField] private UnityEvent _alarmStartEvent = new();
    [SerializeField] private UnityEvent _alarmStopEvent = new();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _isActive = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        _isActive = false;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isActive)
            Volume = Mathf.MoveTowards(Volume, 1.0f, Time.deltaTime * _timeMultiplier);
        else
            Volume = Mathf.MoveTowards(Volume, 0.0f, Time.deltaTime * _timeMultiplier);
    }

    private float Volume
    {
        get { return _volume; }
        set
        {
            _volume = Mathf.Clamp(value, 0f, 1f);
            
            if (_volume > 0)
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                    _alarmStartEvent.Invoke();
                }

                _audioSource.volume = _volume;
            }
            else if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
                _alarmStopEvent.Invoke();
            }
        }
    }

    private float _volume;
    private bool _isActive;
    private AudioSource _audioSource;
}
