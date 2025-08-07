using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : PersistentSingleton<SoundManager>
{
    [Header("Audio Mixer Group")]
    [SerializeField] private AudioMixer _audioMixer = default;

    [Header("Float Variable Volume")]
    [SerializeField] private FloatVariableSO _masterVolume;
    [SerializeField] private FloatVariableSO _musicVolume;
    [SerializeField] private FloatVariableSO _sfxVolume;

    [SerializeField] private ObjectPoolSO _audioChannelPool;
    [SerializeField] private SoundGameEventBinding _musicEventBinding;
    [SerializeField] private SoundGameEventBinding _sfxEventBinding;

    private AudioChannel _musicChannel;
    private List<AudioChannel> _sfxChannelList = new();

    // Ensure that there won't be too many sounds playing at the same time
    private readonly Dictionary<SoundDataSO, int> _soundInstanceCounts = new();
    [SerializeField] private int _maxSoundInstances = 3;

    public void PlaySFX(SoundDataSO soundData)
    {
        if (!CanPlaySound(soundData))
            return;

        if (_audioChannelPool.Get().TryGetComponent(out AudioChannel sfxChannel))
        {
            _sfxChannelList.Add(sfxChannel);
            RegisterSoundInstance(soundData);
            sfxChannel.Play(soundData);
            sfxChannel.OnSoundFinishedPlaying += OnAudioChannelFinishedPlaying;
        }

    }
    public void PlayMusic(SoundDataSO soundData) 
    {
        if (_musicChannel == null && _audioChannelPool.Get().TryGetComponent(out AudioChannel channel))
        {
            _musicChannel = channel;
        }
        _musicChannel.Play(soundData, false);
    }

    public void StopMusic() => _musicChannel.Stop();

    private const string PARAM_MASTER = "MasterVolume";
    private const string PARAM_MUSIC = "MusicVolume";
    private const string PARAM_SFX = "SFXVolume";


    void OnEnable()
    {
        _masterVolume.OnValueChanged += SetMasterVolume;
        _musicVolume.OnValueChanged += SetMusicVolume;
        _sfxVolume.OnValueChanged += SetSFXVolume;

        // Apply initial values
        SetMasterVolume(_masterVolume.Value);
        SetMusicVolume(_musicVolume.Value);
        SetSFXVolume(_sfxVolume.Value);

        // Suscribe to events
        _musicEventBinding.Bind(PlayMusic, this);
        _sfxEventBinding.Bind(PlaySFX, this);
    }

    void OnDisable()
    {
        _masterVolume.OnValueChanged -= SetMasterVolume;
        _musicVolume.OnValueChanged -= SetMusicVolume;
        _sfxVolume.OnValueChanged -= SetSFXVolume;

        _musicEventBinding.Unbind(PlayMusic, this);
        _sfxEventBinding.Unbind(PlaySFX, this);
    }

    private void SetMasterVolume(float volume)
    {
        SetGroupVolume(PARAM_MASTER, volume);
    }

    private void SetMusicVolume(float volume)
    {
        SetGroupVolume(PARAM_MUSIC, volume);
    }

    private void SetSFXVolume(float volume)
    {
        SetGroupVolume(PARAM_SFX, volume);
    }


    private void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        normalizedVolume = Mathf.Clamp01(normalizedVolume);
        bool volumeSet = _audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));

        if (!volumeSet)
            Debug.LogError($"[SoundManager] Parameter '{parameterName}' not found.");
    }

    private float MixerValueToNormalized(float mixerValue)
    {
        // Convert from dB to linear 0–1 range (UI-friendly)
        return Mathf.Pow(10f, mixerValue / 20f);
    }

    private float NormalizedToMixerValue(float normalizedValue)
    {
        // Convert from 0–1 UI value to dB
        return Mathf.Log10(Mathf.Clamp(normalizedValue, 0.0001f, 1f)) * 20f;
    }

    private void OnAudioChannelFinishedPlaying(AudioChannel audioChannel)
    {
        audioChannel.OnSoundFinishedPlaying -= OnAudioChannelFinishedPlaying;
        UnregisterSoundInstance(audioChannel.SoundData);
        _sfxChannelList.Remove(audioChannel);
        audioChannel.ReturnToPool();
    }

    private bool CanPlaySound(SoundDataSO data)
    {
        return !_soundInstanceCounts.TryGetValue(data, out int count) || count < _maxSoundInstances;
    }

    private void RegisterSoundInstance(SoundDataSO data)
    {
        if (_soundInstanceCounts.ContainsKey(data))
            _soundInstanceCounts[data]++;
        else
            _soundInstanceCounts[data] = 1;
    }

    private void UnregisterSoundInstance(SoundDataSO data)
    {
        if (_soundInstanceCounts.TryGetValue(data, out int count))
        {
            count--;
            if (count <= 0)
                _soundInstanceCounts.Remove(data);
            else
                _soundInstanceCounts[data] = count;
        }
    }
}
