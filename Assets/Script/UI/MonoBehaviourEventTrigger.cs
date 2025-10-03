using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourEventTrigger : MonoBehaviour {
    public UnityEvent onAwake = new UnityEvent();
    public UnityEvent onDestroy = new UnityEvent();
    public AudioManager audioManager;

    void Awake() {
        onAwake.Invoke();
        if (audioManager.buttonSound == true) {
            this.GetComponent<AudioSource>().PlayOneShot(audioManager.sound);
            audioManager.buttonSound = false;
            audioManager.sound = null;
        }
    }

    void OnDestroy() {
        onDestroy.Invoke();
    }
}