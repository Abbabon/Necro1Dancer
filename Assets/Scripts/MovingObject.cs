using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public bool IsCollectable;
    [SerializeField] protected List<MoveType> _moveSet;
    protected int _moveIndex = 0; 
        
    protected void Start()
    {
        GameEngine.Instance.Beat += OnBeat;
    }

    protected abstract void OnBeat();
}