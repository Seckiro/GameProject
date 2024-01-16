using System;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : SingletonMono<AudioManager>
{
    public AudioSource bgmPlayer;
    public AudioSource soundPlayer;

    // 播放背景音乐
    public void PlayMusic(string name)
    {
        if (bgmPlayer.isPlaying == false)
        {
            AudioClip clip = Resources.Load<AudioClip>(name);
            bgmPlayer.clip = clip;
            bgmPlayer.loop = true;
            bgmPlayer.Play();
        }

    }
    public void StopMusic()
    {
        bgmPlayer.Stop();
    }

    // 播放台本
    public void PlaySound(string name)
    {
        if (soundPlayer.isPlaying)
        {
            soundPlayer.Stop();
        }

        AudioClip clip = Resources.Load<AudioClip>(name);
        soundPlayer.clip = clip;
        soundPlayer.Play();
    }

    public void StopSound()
    {
        soundPlayer.Stop();
    }
}

