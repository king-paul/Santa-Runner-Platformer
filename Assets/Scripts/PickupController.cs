using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupController : MonoBehaviour
{
    public UnityEvent onCollect;
    public AudioClip collectSound;

    GameManager gameManager;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Destroy(this.gameObject);
            onCollect.Invoke();
        }
    }

    public void AddPresent() { gameManager.AddPresent(); }
    public void AddStamina(float amount) {  }

}
