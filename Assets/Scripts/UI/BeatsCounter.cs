using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeatsCounter : MonoBehaviour
{
    private TextMeshProUGUI _beatsText;
    [SerializeField] private bool _showText = false;
    [SerializeField] private bool _showOpposite = false;

    private void Awake(){
        _beatsText = GetComponent<TextMeshProUGUI>();
    }

    void Start(){
        GameEngine.Instance.BeatsChanged += OnBeatChanged;
    }

    private void OnBeatChanged(int beats){
        int beatsToShow = _showOpposite ? GameEngine.Instance.BeatsForLevel - beats : beats;
        _beatsText.text = _showText ? $"You did it with {beatsToShow} Beats" : $"{beatsToShow}";
        if (beatsToShow <= 50 && beatsToShow > 0)
        {
            _beatsText.text = _showText ? $"You did it with {beatsToShow} Beats" : $"<color=red>{beatsToShow}</color>";
        }
    }
}
