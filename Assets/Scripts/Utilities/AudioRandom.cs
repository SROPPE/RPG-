
using UnityEngine;

public class AudioRandom : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    AudioSource audioSource;
    System.Random randomNumber;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        randomNumber = new System.Random();
    }
    public void SetAndPlayRandomAudioFromClips()
    {
        audioSource.clip = clips[randomNumber.Next(0, clips.Length)];
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
