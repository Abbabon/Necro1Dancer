using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MovingObject
{
    public MoveType Direction { get; set; }
    private Transform _transform;

    protected void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    protected override void OnBeat()
    {
        var hitOther = TryMove(Direction);
        if (hitOther != null && !hitOther.CompareTag("Respawn"))
        {
            var enemy = hitOther.GetComponent<GenericEnemy>();
            if (enemy != null)
            {
                enemy.KillEnemy();
            }
            Destroy(gameObject);
        }
    }
}
