using UnityEngine;

public class  SEOnClick : MonoBehaviour 
{
    public AudioSource audioSource;
    public AudioClip[] soundEffects;

    private int clickCount = 0;

    void Start()
    {
    }

    void OnMouseDown()
    {
        if (audioSource == null || soundEffects.Length == 0) return;
        
        int index = Mathf.Min(clickCount, soundEffects.Length - 1);

        audioSource.PlayOneShot(soundEffects[index]);

        clickCount++;
    }
}