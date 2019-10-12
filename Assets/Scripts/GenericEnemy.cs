using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MovingObject
{
    [SerializeField] int _beatsToLive;

    public Action OnDeathEvent;

    private int _beatsAlive;

    protected override void OnBeat()
    {
        if (_beatsToLive > 0 && ++_beatsAlive > _beatsToLive)
        {
            KillEnemy();
            return;
        }
        
        MoveType stepDir = _moveSet[_moveIndex];
        Vector3Int step = MakeStep(stepDir);
        var tilemap = GameEngine.Instance.Tilemap;
        Vector3 future = tilemap.CellToWorld(_myPosition + step);
        Collider2D other = Physics2D.OverlapCircle(new Vector2(future.x + 0.5f, future.y + 0.5f), 0.1f);
        if (other == null || other.gameObject == gameObject)
        {
            transform.Translate(future - tilemap.CellToWorld(_myPosition));
            _myPosition += step;
            _moveIndex = ++_moveIndex % _moveSet.Count;
        }
        else if (other.CompareTag("Player"))
        {
            GameEngine.Instance.LoseHealth();
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
        }
        return Vector3Int.zero;
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
            OnDeathEvent?.Invoke();
            Destroy(gameObject);
    }
}
