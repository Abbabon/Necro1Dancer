using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Direction { get; set; }
    private Transform _transform;

    protected void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    protected void Start()
    {
        GameEngine.Instance.Beat += MoveTile;
    }

    private void MoveTile()
    {
        var tilemap = GameEngine.Instance.Tilemap;

        var step = Vector3Int.right * Direction;
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
            enemy.KillEnemy();
        }
    }
}
