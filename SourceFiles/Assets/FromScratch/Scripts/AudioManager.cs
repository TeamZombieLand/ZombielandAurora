using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager insta;


    [SerializeField] AudioSource bgSource;
    [SerializeField] AudioSource bg_thunderSouce;
    [SerializeField] AudioSource soundSource;

    [SerializeField] AudioClip[] audioClips;

    private void Awake()
    {
        if (insta == null)
        {
            insta = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    public void playSound(int _no, float _vol = 1f) {
        //if (soundSource.isPlaying) soundSource.Stop();
        soundSource.PlayOneShot(audioClips[_no], _vol);
    }
    public void StartThunderSound()
    {
        bg_thunderSouce.gameObject.SetActive(true);
    }

    [SerializeField] AudioClip inviciblePower;
    [SerializeField] AudioClip freezePower;
    [SerializeField] AudioClip speedPower;
    [SerializeField] AudioClip powerEnded;
    internal void PlayInviciblePower()
    {
        soundSource.PlayOneShot(inviciblePower, 0.4f);
    }

    internal void PlayPowerDisabled()
    {
        soundSource.PlayOneShot(powerEnded, 0.4f);
    }

    internal void PlaySpeedPower()
    {
        soundSource.PlayOneShot(speedPower, 0.4f);
    }

    internal void PlayFreezePower()
    {
        soundSource.PlayOneShot(freezePower, 0.4f);
    }


    [SerializeField] AudioClip dailyCollect;
    internal void PlayDailyCollect()
    {
        soundSource.PlayOneShot(dailyCollect, 0.4f);
    }
}
