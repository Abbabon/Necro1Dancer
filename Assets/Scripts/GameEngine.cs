using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameEngine : MonoBehaviour
{
    // Beat-related
    public Action Beat;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioTestClip;
    public bool TestBeat;
        
    private bool _isRunning = true;
    
    [SerializeField] private float _bpm = 115f;
    private float _beatFraction;
    private float _currentBeatCounter = 0f;

    // Tiles
    [SerializeField] Tilemap _tilemap;
    public Tilemap Tilemap { get { return _tilemap; } }

    // Singleton
    private static GameEngine _instance;
    public static GameEngine Instance { get { return _instance; } }

    protected void Awake()
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
