// Author: Paul King

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PickUpItem : MonoBehaviour
{
    GameManager gameManager;
    AudioSource playerAudio;
    public AudioClip collectSound;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        var audioSources = GetComponents<AudioSource>();
        playerAudio = audioSources[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Coin")
        {
            Destroy(other.gameObject);
            gameManager.AddCoin();

            if (collectSound != null)
                playerAudio.PlayOneShot(collectSound); 
        }
    }
}
