using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public enum GameSessionState
{
    Menu,
    Playing,
    Lose,
    Win,
}

public class GameEngine : MonoBehaviour
{
    // Beat-related
    public Action Beat;
    private float _timeSinceLevelLoadOnStart;
    private int _previousBeat;
    public int Beats { get { return _previousBeat; } }
    [SerializeField] private int _beatsForLevel;
    public int BeatsForLevel => _beatsForLevel;

    // Gameplay-related
    private int _health;
    public Action<int> HealthChanged;
    private int _ammo;
    public int Ammo { get { return _ammo; } }
    public Action<int> AmmoChanged;
    private int _sessionNumberOfBeats;
    public Action<int> BeatsChanged;
    private ScreenFlash _screenFlash = new ScreenFlash();
    private Vector2 _checkpoint;

    [Button]
    public void LoseHealth()
    {
        DoScreenFlash();

        _health--;
        HealthChanged?.Invoke(_health);

        if (_health <= 0){
            PlayerDie();
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
        if (_ammo > 0)
        {
            _ammo--;
            AmmoChanged?.Invoke(_ammo);
        }
    }

    //TODO: Sound Manager 
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioTestClip;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _startSound;
    [SerializeField] private AudioClip _inGameMusic;
    [SerializeField] private AudioClip _winFanfare;
    public bool TestBeat;

    // Session-State related

    [SerializeField] private CanvasGroup _redFlashOfDoom;
    [SerializeField] private CanvasGroup _menuCanvasGroup;
    [SerializeField] private CanvasGroup _winningCanvasGroup;
    [SerializeField] private CanvasGroup _retryCanvasGroup;
    [SerializeField] private CanvasGroup _hudCanvasGroup;
    private GameSessionState _gameState = GameSessionState.Menu;
    public bool GameRunning { get { return _gameState == GameSessionState.Playing; } }

    private float _bpm = 115f;
    private float _beatFraction;
    public float BeatFraction { get { return _beatFraction; } }

    // Tiles
    [SerializeField] Tilemap _tilemap;
    public Tilemap Tilemap { get { return _tilemap; } }

    // Player
    [SerializeField] PlayerMovementController _player;

    // Singleton
    private static GameEngine _instance;
    public static GameEngine Instance { get { return _instance; } }

    protected void Awake()
    {
        _instance = this;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _menuMusic;
        _audioSource.Play();

        _beatFraction = 60f / _bpm;
        Beat += OnBeat;
    }

    private void Start()
    {
        ChangeGameState(GameSessionState.Menu);
    }

    public void SetPlayerCheckpoint(Vector2 position)
    {
        _checkpoint = position;
    }

    public void PlayerDie()
    {
        ResetHealthAndAmmo();

        _player.ResetToCheckpoint(_checkpoint);
    }

    private void InitializeSession()
    {
        ResetHealthAndAmmo();

        _sessionNumberOfBeats = 0;
        BeatsChanged?.Invoke(_sessionNumberOfBeats);
    }

    private void ResetHealthAndAmmo()
    {
        _health = 3;
        HealthChanged?.Invoke(_health);

        _ammo = 0;
        AmmoChanged?.Invoke(_ammo);
    }

    private void OnBeat()
    {
        if (TestBeat)
        {
            _audioSource.PlayOneShot(_audioTestClip);
        }

        _sessionNumberOfBeats++;
        BeatsChanged?.Invoke(_sessionNumberOfBeats);

        if (_sessionNumberOfBeats >= _beatsForLevel)
        {
            LostGame();
        }
    }

    private void Update()
    {
        if (GameRunning)
        {
            var _timeSinceLevelLoad = Time.timeSinceLevelLoad - _timeSinceLevelLoadOnStart;
            int frameBeat = (int)(_timeSinceLevelLoad / _beatFraction);

            if (frameBeat > _previousBeat)
            {
                _previousBeat = frameBeat;
                Beat?.Invoke();
                
            }
        }
        else //menues, right now its quite redundent but habit forces me to create this
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (_gameState)
                {
                    case GameSessionState.Menu:
                        StartGame();
                        break;
                    case GameSessionState.Win:
                    case GameSessionState.Lose:
                        RestartScene();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void StartGame()
    {
        ChangeGameState(GameSessionState.Playing);
        _audioSource.Stop();
        _audioSource.clip = _inGameMusic;
        _audioSource.Play();
        _audioSource.PlayOneShot(_startSound);

        _timeSinceLevelLoadOnStart = Time.timeSinceLevelLoad;
        InitializeSession();
    }

    private void ChangeGameState(GameSessionState gameSessionState)
    {
        _gameState = gameSessionState;

        ChangeCanvasGroupState(_menuCanvasGroup, (gameSessionState == GameSessionState.Menu));
        ChangeCanvasGroupState(_retryCanvasGroup, (gameSessionState == GameSessionState.Lose));
        ChangeCanvasGroupState(_winningCanvasGroup, (gameSessionState == GameSessionState.Win));
        ChangeCanvasGroupState(_hudCanvasGroup, (gameSessionState == GameSessionState.Playing));
    }

    public void RestartScene()
    {
        //TODO: retry without scene loading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WonGame()
    {
        ChangeGameState(GameSessionState.Win);
        
        _audioSource.clip = _winFanfare;
        _audioSource.Play();
    }
    
    public void LostGame()
    {
        ChangeGameState(GameSessionState.Lose);
    }

    public void DoScreenFlash()
    {
        StartCoroutine(_screenFlash.Flash(_redFlashOfDoom));
    }

    private void ChangeCanvasGroupState(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
    }
}
