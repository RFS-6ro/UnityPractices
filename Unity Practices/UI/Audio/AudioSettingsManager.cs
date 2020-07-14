using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; protected set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _volumes = new float[(int)AudioType.count];

        //soon read from saved settings
        for (int audioIndec = 0; audioIndec < _volumes.Length; audioIndec++)
        {
            _volumes[audioIndec] = 1f;
        }
    }

    private float[] _volumes;
    private Action<float>[] _onAudioPropertyChanged;

    public void Subscribe(AudioType type, Action<float> action)
    {
        if (_onAudioPropertyChanged == null)
        {
            _onAudioPropertyChanged = new Action<float>[(int)AudioType.count - 1];
        }

        _onAudioPropertyChanged[(int)type] += action;
    }

    public void Unsubscribe(AudioType type, Action<float> action)
    {
        if (_onAudioPropertyChanged == null)
        {
            return;
        }

        var invokations = _onAudioPropertyChanged[(int)type].GetInvocationList();

        for (int i = 0; i < invokations.Length; i++)
        {
            var actionPart = invokations[i];

            if (actionPart.Method.Name == action.Method.Name)
            {
                _onAudioPropertyChanged[(int)type] -= action;
                return;
            }
        }

        throw new Exception("Unsubscrubed method missing");
    }

    public void OnVolumeChanged(GameObject slider)
    {
        AudioType type = slider.GetComponent<SliderHandler>().Type;
        float volume = slider.GetComponent<Slider>().value;

        _volumes[(int)type] = volume;

        if (type == AudioType.wholeAudio)
        {
            for (int i = 0; i < _onAudioPropertyChanged.Length; i++)
            {
                _onAudioPropertyChanged[i]?.Invoke(_volumes[i] * _volumes[(int)AudioType.wholeAudio]);
            }
        }
        else
        {
            _onAudioPropertyChanged[(int)type]?.Invoke(volume * _volumes[(int)AudioType.wholeAudio]);
        }
    }

    public float GetVolumeByType(AudioType type)
    {
        //soon check localisation

        return _volumes[(int)type];
    }

    //acoustic properties
    #region
    public Action<bool> OnAcousticChanged;
    
    public void OnAcousticPropertyChanged(Toggle toggle)
    {
        OnAcousticChanged?.Invoke(toggle.isOn);
    }
    #endregion
}
