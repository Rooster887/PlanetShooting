using System.Collections;
using UnityEngine;

public static class AudioSourceExtentions
{
    public static void Play(this AudioSource audioSource, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;

            audioSource.Play();
        }
    }

    public static IEnumerator FadeOut(this AudioSource audioSource, float fadeTime = 0.1f)
    {
        float startVolume = audioSource.volume;

        fadeTime = fadeTime < 0.1f ? 0.1f : fadeTime;

        for (float t = 0f; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, Mathf.Clamp01(t / fadeTime));
            yield return null;
        }

        audioSource.volume = 0f;

        audioSource.Stop();
        audioSource.clip = null;
    }
}