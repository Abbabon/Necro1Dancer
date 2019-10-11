using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform _transform;
    private bool _movedOnBeat;
    protected Vector3Int _myPosition;
    
    //Animations
    [SerializeField] private Sprite[] _idleSpriteSprites;
    private SpriteRenderer _spriteRenderer;
    private bool _isJumping = false;
    private Animator _animator;
    private int spriteIndex = 0;
    private bool facingRight = true;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _myPosition = GameEngine.Instance.Tilemap.WorldToCell(transform.position);
        GameEngine.Instance.Beat += OnBeat;
    }

    void Update()
    {
        HandleInput();
    }

    //TODO: support more control methods 
    private void HandleInput()
    {
        if (GameEngine.Instance.GameRunning)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)){
                MoveTile(1);
            }else if (Input.GetKeyDown(KeyCode.LeftArrow)){
                MoveTile(-1);
            }
        }
    }

    private void MoveTile(int vectorFactor)
    {
        if (!_movedOnBeat)
        {
            //collisions:
            Vector3Int step = Vector3Int.right * vectorFactor;
            var tilemap = GameEngine.Instance.Tilemap;
            Vector3 future = tilemap.CellToWorld(_myPosition + step);
            Collider2D other = Physics2D.OverlapCircle(new Vector2(future.x + 0.5f, future.y + 0.5f), 0.1f);
            if (other == null || other.gameObject == gameObject)
            {
                transform.Translate(future - tilemap.CellToWorld(_myPosition));
                _myPosition += step;
            }
            else
            {
                //todo: hit dat mob
            }
            
            if (vectorFactor > 0 && !facingRight)
            {
                Flip();
            }
            else if (vectorFactor < 0 && facingRight)
            {
                Flip();
            }
            
            //play animation:
            _isJumping = true;
            _animator.SetTrigger("Jump");
            
            _movedOnBeat = true;
        }
        else // dont move if moved on beat, penalize player
        {
            
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        _transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
    }

    //'Reset' movement for this beat
    private void OnBeat()
    {
        //animation:
        if (!_isJumping)
        {
//            Debug.Log("Static");
//            spriteIndex = (spriteIndex+1)%_idleSpriteSprites.Length;
//            _spriteRenderer.sprite = _idleSpriteSprites[spriteIndex];
        }
        
        //movement:
        if (!_movedOnBeat)
        {
            //penalize player
        }

        _movedOnBeat = false;
    }
    
    //animation events
    public void DoneJumping()
    {
        _isJumping = false;
    }
}
