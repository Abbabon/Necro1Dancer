using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrogUI : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Sprite[] _sprites;
    private int spriteIndex = 0;

    private void Awake(){
        _image = GetComponent<Image>();
    }

    void Start(){
        GameEngine.Instance.Beat += OnBeat;
    }
    
    void OnBeat(){
//        spriteIndex = (spriteIndex+1)%_sprites.Length;
//        _image.sprite = _sprites[spriteIndex];
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.2f).setEasePunch();
    }
}
