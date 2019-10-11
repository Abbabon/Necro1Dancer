using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MovingObject : MonoBehaviour
{
    public bool IsCollectable;
    [SerializeField] protected List<MoveType> _moveSet;
    protected int _moveIndex = 0;
    protected Vector3Int _myPosition;
        
    protected void Start()
    {
        _myPosition = GameEngine.Instance.Tilemap.WorldToCell(transform.position);
        GameEngine.Instance.Beat += OnBeat;
    }

    protected abstract void OnBeat();
}