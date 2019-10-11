using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeesCounter : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Sprite _ammoSprite;
    [SerializeField] private int _index;

    private void Awake(){
        _image = GetComponent<Image>();
    }

    void Start(){
        GameEngine.Instance.AmmoChanged += OnAmmoChanged;
    }

    private void OnAmmoChanged(int ammo){
        var newColor = (ammo >= _index) ? Color.white : new Color(1,1,1,0);
        if (newColor != _image.color)
            LeanTween.scale(gameObject, Vector3.one * 1.3f, 0.3f).setEasePunch();
        _image.color = newColor;
        
    }
}
