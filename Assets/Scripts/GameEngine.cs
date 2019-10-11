using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


public class GameEngine : MonoBehaviour
{
    public Action Beat;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioTestClip;
    public bool TestBeat;
        
    private bool _isRunning = true;
    
    [SerializeField] private float _bpm = 115f;
    private float _beatFraction;
    private float _currentBeatCounter = 0f;

    private static GameEngine _instance;
    public static GameEngine Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;

        _audioSource = GetComponent<AudioSource>();

        _beatFraction = 60f / _bpm;
        Beat += OnBeat;
    }
    

    private void OnBeat()
    {
        if (TestBeat)
        {
            Debug.Log("Boop");
            _audioSource.PlayOneShot(_audioTestClip);
        }
    }

    private void Update()
    {
        if (_isRunning){
            _currentBeatCounter += Time.deltaTime;

            if (_currentBeatCounter > _beatFraction)
            {
                Beat.Invoke();
                _currentBeatCounter = 0;
            }
        }
    }
}
