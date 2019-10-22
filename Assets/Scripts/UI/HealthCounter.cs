using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCounter : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;
    
    [SerializeField] private int _index;

    private void Awake(){
        _image = GetComponent<Image>();
    }

    void Start(){
        GameEngine.Instance.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int health){
         var newSprite = (health >= _index) ? fullSprite : emptySprite;
         _image.sprite = newSprite;
    }
}
