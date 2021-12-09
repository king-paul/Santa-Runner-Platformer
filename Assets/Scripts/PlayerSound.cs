// Author: Paul King

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    // audio clip objects
    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip highJumpSound;
    public AudioClip landSound;
    public AudioClip hazardSound;
    public AudioClip collideSound;
    public AudioClip fallSound;
    public AudioClip collectSound;

    // audio source objects
    private AudioSource playerAudio;
    private AudioSource footsteps;

    // Start is called before the first frame update
    void Start()
    {
        // get audio sources
        var audioSources = GetComponents<AudioSource>();
        footsteps = audioSources[0];
        playerAudio = audioSources[1];
    }

    /// <summary>
    /// Plays any audio clip once  passed as a parameter
    /// </summary>
    /// <param name="clip">The audio clip to be played</param>
    public void PlaySound(AudioClip clip)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(clip);
    }

    /// <summary>
    /// Turns the footsteps sound back on after a fixed amount of time
    /// </summary>
    /// <param name="time">The time delay before playing the sound</param>
    public void PlayRunningSound(float time)
    {
        if(!footsteps.isPlaying)
            footsteps.PlayScheduled(time);
    }

    /// <summary>
    /// Plays the clip used for the jump sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayJumpSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(jumpSound, volumeScale);
    }

    /// <summary>
    /// Plays the clip used for the high jump sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayHighJumpSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(highJumpSound, volumeScale);
    }

    /// <summary>
    /// Plays the clip used for the "player landing" sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayLandSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(landSound, volumeScale);
    }

    /// <summary>
    /// Plays the clip used for the "collision with wall" sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayCollideSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(collideSound, volumeScale);
    }

    /// <summary>
    /// Plays the clip used for the "death from hazard" sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayHazardSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(hazardSound, volumeScale);
    }

    /// <summary>
    /// Plays the clip used for the "falling" sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayFallSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(fallSound, volumeScale);
    }

    /// <summary>
    /// Plays the clip used for the "coin collect" sound effect
    /// </summary>
    /// <param name="volumeScale">The volume of the sound</param>
    public void PlayCoinCollectSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(collectSound, volumeScale);
    }

}