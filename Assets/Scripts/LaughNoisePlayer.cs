using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughNoisePlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;
    public float soundDelay;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = sound;
        audioSource.loop = false;
        StartCoroutine(PlayWithDelay());
    }

    private System.Collections.IEnumerator PlayWithDelay()
    {
        while (true)
        {
            // Play the audio clip
            audioSource.Play();

            // Wait for the clip to finish (or wait for a specific time)
            yield return new WaitForSeconds(sound.length + soundDelay);

            // Optionally, add more logic to check if you want to stop looping at some point
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
