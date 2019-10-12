using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameEngine : MonoBehaviour
{
    // Beat-related
    public Action Beat;
    private float _beatEpsilon = 0.1f;
    
    // Gameplay-related
    private int _health;
    public Action<int> HealthChanged;
    private int _ammo;
    public int Ammo { get { return _ammo; } }
    public Action<int> AmmoChanged;
    private int _sessionNumberOfBeats;
    public Action<int> BeatsChanged;
    private ScreenFlash _screenFlash = new ScreenFlash();
    
    [Button]
    public void LoseHealth()
    {
        StartCoroutine(_screenFlash.Flash(_redFlashOfDoom));
        _health--;
        HealthChanged?.Invoke(_health);

        if (_health <= 0)
        {
            StopGame();
        }
    }

    [Button]
    public void GainAmmo()
    {
        _ammo++;
        AmmoChanged?.Invoke(_ammo);
    }

    [Button]
    public void LoseAmmo()
    {
        _ammo--;
        AmmoChanged?.Invoke(_ammo);
    }
    
    //TODO: Sound Manager 
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioTestClip;
    public bool TestBeat;

    // Session-State related

    [SerializeField] private CanvasGroup _redFlashOfDoom;
    [SerializeField] private CanvasGroup _menuCanvasGroup;
    [SerializeField] private CanvasGroup _retryCanvasGroup;
    [SerializeField] private CanvasGroup _hudCanvasGroup;
    private bool _gameRunning = false;
    public bool GameRunning { get { return _gameRunning; } }

    private float _bpm = 115f;
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
        ChangeCanvasGroupState(_menuCanvasGroup, true);
        ChangeCanvasGroupState(_retryCanvasGroup, false);
        ChangeCanvasGroupState(_hudCanvasGroup, false);
    }

    private void InitializeSession()
    {
        _health = 3;
        HealthChanged?.Invoke(_health);
        
        _ammo = 0;
        AmmoChanged?.Invoke(_ammo);
        
        _sessionNumberOfBeats = 0;
        BeatsChanged?.Invoke(_sessionNumberOfBeats);
    }

    private void OnBeat()
    {
        if (TestBeat)
        {
            _audioSource.PlayOneShot(_audioTestClip);
        }

        _sessionNumberOfBeats++;
        BeatsChanged?.Invoke(_sessionNumberOfBeats);
    }

    private void Update()
    {
        if (_gameRunning){
            _currentBeatCounter += Time.deltaTime;

            if (_currentBeatCounter >= _beatFraction)
            {
                Debug.Log(_currentBeatCounter);
                Beat?.Invoke();
                _currentBeatCounter = 0;
            }
        }
    }

    public void StartGame()
    {
        ChangeCanvasGroupState(_menuCanvasGroup, false);
        ChangeCanvasGroupState(_retryCanvasGroup, false);
        ChangeCanvasGroupState(_hudCanvasGroup, true);
        
        _audioSource.Stop();
        _audioSource.Play();
        
        _gameRunning = true;
        InitializeSession();
    }

    public void RestartScene()
    {
        //TODO: retry without scene loading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void StopGame()
    {
        //TODO: anything related to high scores and retry screen wassach go in
        
        ChangeCanvasGroupState(_menuCanvasGroup, false);
        ChangeCanvasGroupState(_retryCanvasGroup, true);
        ChangeCanvasGroupState(_hudCanvasGroup, false);;
        _gameRunning = false;
    }

    private void ChangeCanvasGroupState(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
    }
}
