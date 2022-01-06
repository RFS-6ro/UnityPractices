using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSoundReferenceOnCamera : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    [SerializeField] private AudioType _type;
    [SerializeField] private float _areaAudibilityaRadius;

    private Transform _transform;
    private AudioSource _source;
    private AudioSettingsManager _audioSettings;

    private float _maxVolume = 1f;
    private bool _isAcoustic = true;

    private float _stereoValue;
    private float _newVolume = 1;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main.transform;
        }

        _source = GetComponent<AudioSource>();
        _transform = transform;
    }
    
    private void OnEnable()
    {
        StartCoroutine(WaitForInstance());
    }

    private IEnumerator WaitForInstance()
    {
        yield return new WaitUntil(() => AudioSettingsManager.Instance != null);

        _audioSettings = AudioSettingsManager.Instance;
        _maxVolume = AudioSettingsManager.Instance.GetVolumeByType(_type);
        _audioSettings.Subscribe(_type, OnVolumeChanged);
        _audioSettings.OnAcousticChanged += OnAcousticChange;
    }

    private void FixedUpdate()
    {
        if (_camera == null)
        {
            return;
        }

        float distance = _camera.position.x - _transform.position.x;
        
        if (Mathf.Abs(distance) < _areaAudibilityaRadius)
        {
            _stereoValue = (-1) * distance / _areaAudibilityaRadius;

            _newVolume = (_areaAudibilityaRadius - Mathf.Abs(distance)) / _areaAudibilityaRadius;

            _newVolume *= _maxVolume;
        }
        else
        {
            _stereoValue = 0f;
            _newVolume = 0f;
        }

        if (!_isAcoustic)
        {
            _stereoValue = 0f;
        }

        _source.volume = _newVolume;
        _source.panStereo = _stereoValue;
    }

    private void OnVolumeChanged(float newVolume)
    {
        _maxVolume = newVolume;
    }

    private void OnAcousticChange(bool property)
    {
        _isAcoustic = property;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _areaAudibilityaRadius);
    }

    private void OnDisable()
    {
        _audioSettings = AudioSettingsManager.Instance;
        if (_audioSettings != null)
        {
            _audioSettings.Unsubscribe(_type, OnVolumeChanged);
            _audioSettings.OnAcousticChanged -= OnAcousticChange;
        }
    }
}
