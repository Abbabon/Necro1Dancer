using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform _transform;
    private bool _movedOnBeat;
    protected Vector3Int _myPosition;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
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
