using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MovingObject
{
    public Action OnDeathEvent;

    protected override void OnBeat()
    {
        MoveType stepDir = _moveSet[_moveIndex];
        Vector3Int step = makeStep(stepDir);
        transform.Translate(GameEngine.Instance.Tilemap.CellToWorld(_myPosition + step) - GameEngine.Instance.Tilemap.CellToWorld(_myPosition));
        _myPosition += step;
        _moveIndex = ++_moveIndex % _moveSet.Count;
    }

    private Vector3Int makeStep(MoveType stepDir)
    {
        switch (stepDir)
        {
            case (MoveType.Down):
                return new Vector3Int(0, -1, 0);
            case (MoveType.Left):
                return new Vector3Int(-1, 0, 0);
            case (MoveType.Right):
                return new Vector3Int(1, 0, 0);
            case (MoveType.Up):
                return new Vector3Int(0, 1, 0);
        }
        return Vector3Int.zero;
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnDeathEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}
