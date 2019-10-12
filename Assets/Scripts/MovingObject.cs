using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MovingObject : MonoBehaviour
{
    protected virtual void Start()
    {
        GameEngine.Instance.Beat += OnBeat;
    }

    protected void OnDestroy()
    {
        GameEngine.Instance.Beat -= OnBeat;
    }

    protected abstract void OnBeat();

    protected Collider2D TryMove(MoveType move)
    {
        var tilemap = GameEngine.Instance.Tilemap;
        var futureCell = tilemap.CellToWorld(tilemap.WorldToCell(transform.position) + MakeStep(move));

        var other = Physics2D.OverlapCircle(new Vector2(futureCell.x + 0.5f, futureCell.y + 0.5f), 0.1f);
        if (other == null || other.gameObject == gameObject)
        {
            transform.position = futureCell;
            return null;
        }
        else
        {
            return other;
        }
    }

    private Vector3Int MakeStep(MoveType stepDir)
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
            default:
                return Vector3Int.zero;
        }
    }
}