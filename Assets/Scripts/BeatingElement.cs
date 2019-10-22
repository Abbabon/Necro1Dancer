using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatingElement : MonoBehaviour
{
    private Vector3 _originalScale;
    [SerializeField] private float _scaleFactor = 1.2f;
    [SerializeField] private float _time = 0.2f;
    [SerializeField] private LeanTweenType _ease = LeanTweenType.punch;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    void Start(){
        GameEngine.Instance.Beat += OnBeat;
    }
    
    void OnBeat()
    {
        LeanTween.scale(gameObject, _originalScale * _scaleFactor, _time).setEase(_ease);
    }
}
