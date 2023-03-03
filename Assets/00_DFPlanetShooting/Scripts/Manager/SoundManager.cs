using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace J8N9.PlanetShooting
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [Header("BGM")]
        public List<AudioClip> BGMClipList = new List<AudioClip>(); // BGM

        [Header("SE")]
        public List<AudioClip> SEClipList = new List<AudioClip>(); // SE

        public AudioSource[] SeAudioSource;

        public AudioSource BgmAudioSource;

        public void PlaySe(AudioSource audioSource)
        {
            audioSource.Play();
        }

        // BGM再生
        public void PlayBgm(string clipName, bool isLoop = false)
        {
            var audioClip = BGMClipList.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log("BGMは見つかりません");
                return;
            }

            BgmAudioSource.volume = 1.0f;
            BgmAudioSource.Play(audioClip);
            BgmAudioSource.loop = isLoop;
        }

        // BGMフェードアウト
        public void BGMFadeOut(float fadeTime = 2f)
        {
            if (BgmAudioSource.clip == null)
            {
                Debug.Log("BGMは見つかりません");
                return;
            }
            StartCoroutine(BgmAudioSource.FadeOut(fadeTime));
        }
    }
}