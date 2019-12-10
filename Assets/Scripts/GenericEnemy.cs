using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GenericEnemy : MovingObject
{
    [FormerlySerializedAs("IsCollectable")]
    [SerializeField] bool _isCollectable;
    [SerializeField] bool _facingRightOnStart;
    public bool IsCollectable { get { return _isCollectable; } }
    [SerializeField] List<MoveType> _moveSet;
    
    private int _spriteIndex = 0;
    [SerializeField] List<Sprite> _animationSprites;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Animator _animator;
    [SerializeField] int _beatsToLive;
    [SerializeField] bool _loop = true;

    public Action OnDeathEvent;

    private int _moveIndex = 0;
    private int _beatsAlive;
    
    [SerializeField] private bool _turnIntoLillypad;
    private bool _turningIntoLillypad = false; 

    protected void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _facingRight = _facingRightOnStart;
        _animator = GetComponentInChildren<Animator>();
    }

    protected override void OnBeat()
    {
        if (_beatsToLive > 0 && ++_beatsAlive > _beatsToLive){
            KillEnemy();
            return;
        }

        if (_spriteRenderer != null &&_animationSprites.Count > 0){
            _spriteIndex = (_spriteIndex+1)%_animationSprites.Count;
            _spriteRenderer.sprite = _animationSprites[_spriteIndex];
        }else if (_animator != null)
        {
            _animator.SetTrigger("Beat");
        }

        var hitOther = TryMove(_moveSet[_moveIndex]);
        if (hitOther == null || hitOther.isTrigger)
        {
            if (_loop)
            {
                _moveIndex = ++_moveIndex % _moveSet.Count;
            }
            else
            {
                _moveIndex = Mathf.Min(++_moveIndex, _moveSet.Count - 1);
            }
        }
        else if (hitOther.CompareTag("Player"))
        {
            GameEngine.Instance.LoseHealth();
        }
    }

    public void KillEnemy()
    {
        //start the chain of events that turn this enemy into a lilypad
        if (_turnIntoLillypad)
        {
            _turningIntoLillypad = true;
            _animator.SetBool("LillypadChain", true);
        }
        else
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        OnDeathEvent?.Invoke();
        Destroy(gameObject);
    }
    
    //called as an animation event
    public void TurnToLillypad()
    {
        DestroySelf();
    }
}
