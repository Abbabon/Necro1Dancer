using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameEngine : MonoBehaviour
{
    // Beat-related
    public Action Beat;
    
    // Gameplay-related
    private int _health;
    public Action<int> HealthChanged;
    private int _ammo;
    public Action<int> AmmoChanged;
    private int _sessionNumberOfBeats;
    public Action<int> BeatsChanged;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioTestClip;
    public bool TestBeat;
        
    private bool _isRunning = true;
    
    [SerializeField] private float _bpm = 115f;
    private float _beatFraction;
    public float BeatFraction { get { return _beatFraction; }}
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

    private void Start()
    {
        //TODO: move to session start
        InitializeSession();
    }

    private void InitializeSession()
    {
        _health = 3;
        HealthChanged?.Invoke(_health);
        
        _ammo = 3;
        AmmoChanged?.Invoke(_ammo);
        
        _sessionNumberOfBeats = 0;
        BeatsChanged?.Invoke(_sessionNumberOfBeats);
    }

    private void OnBeat()
    {
        if (TestBeat)
        {
            //Debug.Log("Boop");
            _audioSource.PlayOneShot(_audioTestClip);
        }

        _sessionNumberOfBeats++;
        BeatsChanged?.Invoke(_sessionNumberOfBeats);
    }

    private void Update()
    {
        if (_isRunning){
            _currentBeatCounter += Time.deltaTime;

            if (_currentBeatCounter > _beatFraction)
            {
                Beat?.Invoke();
                _currentBeatCounter = 0;
            }
        }
    }
}
