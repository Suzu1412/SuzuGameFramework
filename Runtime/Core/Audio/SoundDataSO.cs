using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public enum ClipPlayMode
{
    Random,
    Sequential
}

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/Audio/Sound Data")]
public class SoundDataSO : ScriptableObject
{
    public string ID;

    [Header("Clip Settings")]
    [SerializeField] private List<AudioClip> _clips = new();
    [SerializeField] private ClipPlayMode _clipPlayMode = ClipPlayMode.Random;
    private int _lastPlayedIndex = -1;

    public bool Loop = false;
    [Range(0f, 1f)] public float Volume = 1f;
    [Range(0f, 1f)] public float VolumeVariance = 0f;
    [Range(-3f, 3f)] public float Pitch = 1f;
    [Range(0f, 3f)] public float PitchVariance = 0f;

    [Header("Delay & Fade")]
    public float Delay = 0f;
    public float FadeInTime = 0f;
    public float FadeOutTime = 0f;

    [Header("3D Settings")]
    [Range(0f, 1f)] public float SpatialBlend = 0f;
    public float MinDistance = 1f;
    public float MaxDistance = 500f;

    [Header("Cooldown")]
    public float Cooldown = 0f;

    public float GetRandomizedVolume() => Mathf.Clamp01(Volume + Random.Range(-VolumeVariance, VolumeVariance));
    public float GetRandomizedPitch() => Mathf.Clamp(Pitch + Random.Range(-PitchVariance, PitchVariance), -3f, 3f);

    public AudioClip GetNextClip()
    {
        if (_clips.Count == 0) return null;
        if (_clips.Count == 1) return _clips[0];

        switch (_clipPlayMode)
        {
            case ClipPlayMode.Random:
                return _clips[Random.Range(0, _clips.Count)];

            case ClipPlayMode.Sequential:
                _lastPlayedIndex = (_lastPlayedIndex + 1) % _clips.Count;
                return _clips[_lastPlayedIndex];

            default:
                return _clips[0];
        }
    }
}
