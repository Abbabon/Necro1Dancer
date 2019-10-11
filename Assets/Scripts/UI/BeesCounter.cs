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
        _image.color = (ammo >= _index) ? Color.white : new Color(1,1,1,0);
    }
}
