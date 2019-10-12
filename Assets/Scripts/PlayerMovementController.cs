using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Projectile _projectilePrefab;

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
    
    private int _lastMovedDirection;

    protected void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        GameEngine.Instance.Beat += OnBeat;
    }

    protected void Update()
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameEngine.Instance.Ammo > 0)
            {
                ShootProjectile();
                GameEngine.Instance.LoseAmmo();
            }
        }
    }

    private void MoveTile(int vectorFactor)
    {
        if (!_movedOnBeat)
        {
            Vector3Int step = Vector3Int.right * vectorFactor;
            var tilemap = GameEngine.Instance.Tilemap;
            var futureCell = tilemap.CellToWorld(tilemap.WorldToCell(transform.position) + step);
            
            var futurePos = new Vector2(futureCell.x + 0.5f, futureCell.y + 0.5f);
            var other = Physics2D.OverlapCircle(futurePos, 0.1f);
            
            var enemy = other?.GetComponent<GenericEnemy>();            
            if (enemy == null)
            {
                transform.position = futureCell;
            }
            else
            {
                if (enemy.IsCollectable)
                {
                    enemy.KillEnemy();
                    GameEngine.Instance.GainAmmo();
                }
                else 
                {
                    GameEngine.Instance.LoseHealth();
                }
            }
                
            _lastMovedDirection = vectorFactor;
            
            if (vectorFactor > 0 && !facingRight)
            {
                Flip();
            }
            else if (vectorFactor < 0 && facingRight)
            {
                Flip();
            }
            
            //play animation:
            _animator.SetTrigger("Jump");
            _isJumping = true;
            
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
        if (!_isJumping){
            _animator.SetTrigger("Breath");
        }
        
        //movement:
        if (!_movedOnBeat){
            //penalize player
        }

        _movedOnBeat = false;
    }
    
    //animation events
    public void DoneJumping(){
        _isJumping = false;
    }

    private void ShootProjectile()
    {
        var projectile = Instantiate(_projectilePrefab, transform.position + Vector3.right * _lastMovedDirection, Quaternion.identity);
        projectile.Direction = _lastMovedDirection;
    }
}
