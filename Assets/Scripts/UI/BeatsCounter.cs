using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeatsCounter : MonoBehaviour
{
    private TextMeshProUGUI _beatsText;
    [SerializeField] private bool _showText = false;

    private void Awake(){
        _beatsText = GetComponent<TextMeshProUGUI>();
    }

    void Start(){
        GameEngine.Instance.BeatsChanged += OnBeatChanged;
    }

    private void OnBeatChanged(int beats){
        _beatsText.text = _showText ? $"You did it with {beats} beats!" : $"{beats}";
    }
}
