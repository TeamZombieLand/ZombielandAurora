using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class PlayerAudio : MonoBehaviour
{
    #region Singleton
    public static PlayerAudio Instance;
    private void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion

    public AudioSource playerAudioSource;
    public AudioClip gruntingSound;
    public AudioClip[] takedamageSound;


    private void OnEnable()
    {
        Health.OnPlayerHealthChange += onPlayerHealthChange; 
    }
    private void OnDisable()
    {
        Health.OnPlayerHealthChange -= onPlayerHealthChange;
    }

    private void onPlayerHealthChange(Health health, bool regain)
    {
        if (!regain)
        {
            PlayOneTimeAudio(takedamageSound[Random.Range(0, takedamageSound.Length)]);
           /* if ((health.currentHealth / health.maxHealth) < 0.2f)
            {
                PlayAudioOnLoop(gruntingSound);
            }*/
        }
    }

    public void PlayAudioOnLoop(AudioClip clip)
    {
        if (playerAudioSource.isPlaying) return;
        playerAudioSource.clip = clip;
        playerAudioSource.loop = true;
        playerAudioSource.Play();
    }
    public void PlayOneTimeAudio(AudioClip clip)
    {
        playerAudioSource.PlayOneShot(clip);
    }
}
