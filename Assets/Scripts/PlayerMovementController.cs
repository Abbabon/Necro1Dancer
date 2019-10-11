using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Transform _transform;
    private bool _movedOnBeat;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void Start()
    {
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
            _transform.Translate(vectorFactor * Vector2.right);
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
