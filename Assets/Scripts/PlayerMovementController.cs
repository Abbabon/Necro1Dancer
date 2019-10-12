using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Projectile _projectilePrefab;

    [SerializeField] private Transform _graphicsTransform;
    private bool _actedOnBeat;
    protected Vector3Int _myPosition;
    private CameraShakeEffect _cameraShaker = new CameraShakeEffect();

    //Animations
    private bool _isJumping = false;
    private Animator _animator;
    private int spriteIndex = 0;
    private bool facingRight = true;
    
    private int _lastMovedDirection;

    protected void Awake()
    {
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
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GameEngine.Instance.Ammo > 0)
                {
                    ShootProjectile();
                    GameEngine.Instance.LoseAmmo();
                }
            }
        }
    }

    private void MoveTile(int vectorFactor)
    {
        if (!_actedOnBeat)
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
                StartCoroutine(_cameraShaker.shake());
                if (enemy.IsCollectable)
                {
                    enemy.KillEnemy();
                    GameEngine.Instance.GainAmmo();
                    //yes, spelling is hawrde
                    _animator.SetTrigger("Swallow");
                    
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
            
            _actedOnBeat = true;
        }
        else // dont move if moved on beat, penalize player
        {
            
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        _graphicsTransform.localScale = new Vector3(_graphicsTransform.localScale.x * -1, _graphicsTransform.localScale.y, _graphicsTransform.localScale.z);
    }

    //'Reset' movement for this beat
    private void OnBeat()
    {
        //animation:
        if (!_isJumping){
            _animator.SetTrigger("Breath");
        }
        
        //movement:
        if (!_actedOnBeat){
            //penalize player
        }

        _actedOnBeat = false;
    }
    
    //animation events
    public void DoneJumping(){
        _isJumping = false;
    }

    private void ShootProjectile()
    {
        if (!_actedOnBeat)
        {
            
            var projectile = Instantiate(_projectilePrefab, transform.position + Vector3.right * _lastMovedDirection, Quaternion.identity);
            projectile.Direction = _lastMovedDirection;
            _animator.SetTrigger("Spit");
            _actedOnBeat = true;
        }
    }
    
    
}
