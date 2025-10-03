using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioManager : ScriptableObject {
    public bool buttonSound = false;
    public AudioClip sound = null;
    public void PlayOneShot(AudioClip clip) {
        if (audioSource != null) {
            audioSource.PlayOneShot(clip);
        }
    }

    public AudioSource audioSource { get; set; }
}