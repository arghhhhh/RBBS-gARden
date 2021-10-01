using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {       
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
    }
    public void PlaySound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.Play();
        //print("playing sound " + s.name);
    }
    public void PlaySoundAfterDelay(string sound, double length)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.PlayScheduled(AudioSettings.dspTime + 0.05 + length);
        //print("playing sound " + s.name);
    }
    public void PauseSound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.Pause();
        //print("paused sound " + s.name);
    }
    public void StopSound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.Stop();
        //print("stopped " + s.name);
    }
    public bool IsSoundPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogError("Sound: " + sound + " not found!");
        }
        if (s.source.isPlaying) return true;
        else return false;
    }
    public AudioMixerGroup GetAudioMixerGroup(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
        }
        return s.source.outputAudioMixerGroup;
    }
    public void ChangeAudioMixerGroup(string sound, AudioMixerGroup mixerGroup)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }
        s.source.outputAudioMixerGroup = mixerGroup;
    }
    public float TimeUntilAudioFinished(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return 0;
        }
        float time = s.source.clip.length - s.source.time;

        return time;
    }
    IEnumerator FadeAudio(float fadeTime)
    {
        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            if (currentVolume > 0) //if audio is already playing
            {
                AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / (fadeTime)); //fade out
            }
            else
            {
                AudioListener.volume = Mathf.Lerp(0, 1f, elapsedTime / (fadeTime)); //fade in
            }
            yield return null;
        }
    }

    IEnumerator DelayAudio(float delayTime, string soundName)
    {
        yield return new WaitForSeconds(delayTime);
        PlaySound(soundName);
    }
}
