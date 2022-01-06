using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    private float[] _volumes;
    private Action<float>[] _onAudioPropertyChanged;

    public static AudioSettingsManager Instance { get; protected set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _volumes = new float[(int)AudioType.count - 1];

        //soon read from saved settings
        for (int audioIndex = 0; audioIndex < _volumes.Length; audioIndex++)
        {
            _volumes[audioIndex] = 1f;
        }
    }

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

    public void OnVolumeChanged(AudioType type, float volume)
    {
        float volumeMultiplyer = GetVolumeByType(AudioType.wholeAudio);

        _volumes[(int)type] = volume;

        if (type == AudioType.wholeAudio)
        {
            for (int i = 0; i < _onAudioPropertyChanged.Length; i++)
            {
                _onAudioPropertyChanged[i]?.Invoke(_volumes[i] * volumeMultiplyer);
            }
        }
        else
        {
            _onAudioPropertyChanged[(int)type]?.Invoke(volume * volumeMultiplyer);
        }
    }

    public float GetVolumeByType(AudioType type) => _volumes[(int)type];

    //acoustic properties
    #region acoustic
    public Action<bool> OnAcousticChanged;
    
    public void OnAcousticPropertyChanged(bool isOn)
    {
        OnAcousticChanged?.Invoke(isOn);
    }
    #endregion
}
