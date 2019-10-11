using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform _transform;
    private bool _movedOnBeat;

    protected void Awake()
    {
        _transform = GetComponent<Transform>();
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
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            MoveTile(1);
        }else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            MoveTile(-1);
        }
    }

    private void MoveTile(int vectorFactor)
    {
        if (!_movedOnBeat)
        {
            var tilemap = GameEngine.Instance.Tilemap;

            var step = Vector3Int.right * vectorFactor;
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
            
            _movedOnBeat = true;
        }
        else // dont move if moved on beat, penalize player
        {
            
        }
    }

    //'Reset' movement for this beat
    private void OnBeat()
    {
        if (!_movedOnBeat)
        {
            //penalize player
        }

        _movedOnBeat = false;
    }
}
