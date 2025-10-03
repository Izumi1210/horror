using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreditButton : MonoBehaviour
{
    public Sprite onImage;
    public Sprite pushImage;
    Image image;
    Sprite offImage;
    bool clicked = false;
    public AudioManager audioManager;
    public AudioClip selectSound;
    public AudioClip hoverSound;

    void Start() {
        image = GetComponent<Image>();
        offImage = image.sprite;
    }

    public void OnPointerEnter(){
        if(!clicked){
            audioManager.PlayOneShot(hoverSound);
            image.sprite = onImage;
        }
    }

    public void OnPointerExit(){
        if(!clicked){
            image.sprite = offImage;
        }
    }

    public void OnPointerClick(){
        clicked = true;
        image.sprite = pushImage;
        audioManager.PlayOneShot(selectSound);
    }
}
