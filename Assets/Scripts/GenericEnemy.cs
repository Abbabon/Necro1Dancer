using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MovingObject
{
    protected override void OnBeat()
    {
        MoveType stepDir = _moveSet[_moveIndex];
        Vector3Int step = makeStep(stepDir);
        var tilemap = GameEngine.Instance.Tilemap;
        Vector3 future = tilemap.CellToWorld(_myPosition + step);
        Collider2D other = Physics2D.OverlapCircle(new Vector2(future.x + 0.5f, future.y + 0.5f), 0.1f);
        if (other == null || other.gameObject == gameObject)
        {
            transform.Translate(future - tilemap.CellToWorld(_myPosition));
            _myPosition += step;
            _moveIndex = ++_moveIndex % _moveSet.Count;
        }
        else
        {
            //todo: hit dat player
        }
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

}
