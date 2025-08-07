using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioChannel : PoolableBehaviour
{
    private AudioSource _audioSource;
    private SoundDataSO _soundData;
    private Coroutine _playingSoundCoroutine;
    internal AudioSource AudioSource => _audioSource != null ? _audioSource : _audioSource = gameObject.GetOrAdd<AudioSource>();

    public event UnityAction<AudioChannel> OnSoundFinishedPlaying;
    public SoundDataSO SoundData => _soundData;

    private void Awake()
    {
        _audioSource = gameObject.GetOrAdd<AudioSource>();
    }

    public void Play(SoundDataSO data, bool isSfx = true)
    {
        if (data == null) return;

        AudioClip clip = data.GetNextClip();

        if (clip == null) return;

        _soundData = data;
        AudioSource.Stop(); // Stop previous just in case
        SetupAudioSource(AudioSource, data, clip);

        if (data.FadeInTime > 0f)
        {
            StartCoroutine(FadeInAndPlay(AudioSource, data));
        }
        else if (data.Delay > 0f)
        {
            StartCoroutine(PlayDelayed(AudioSource, data.Delay));
        }
        else
        {
            AudioSource.Play();
        }

        if (isSfx)
        {
            Invoke(nameof(SoundFinishedPlaying), clip.length);
        }
    }


    public void Stop()
    {
        AudioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        AudioSource.volume = volume;
    }
        
    private void SetupAudioSource(AudioSource source, SoundDataSO data, AudioClip clip)
    {
        source.clip = clip;
        source.loop = data.Loop;
        source.volume = (data.FadeInTime > 0f) ? 0f : data.GetRandomizedVolume();
        source.pitch = data.GetRandomizedPitch();
        source.spatialBlend = data.SpatialBlend;
        source.minDistance = data.MinDistance;
        source.maxDistance = data.MaxDistance;
    }

    private IEnumerator PlayDelayed(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Play();
    }

    private IEnumerator FadeInAndPlay(AudioSource source, SoundDataSO data)
    {
        yield return new WaitForSeconds(data.Delay);

        source.volume = 0f;
        source.Play();

        float timer = 0f;
        float targetVolume = data.GetRandomizedVolume();

        while (timer < data.FadeInTime)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, timer / data.FadeInTime);
            yield return null;
        }

        source.volume = targetVolume;
    }

    public IEnumerator FadeOut(AudioSource source, float fadeOutTime)
    {
        float startVolume = source.volume;
        float timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutTime);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // reset for next use
    }

    public bool IsPlaying()
    {
        return AudioSource.isPlaying;
    }

    private void SoundFinishedPlaying()
    {
        OnSoundFinishedPlaying?.Invoke(this); // The SoundManager will pick this up
    }
}

