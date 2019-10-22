using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MovingObject
{
    [SerializeField] int _beatsToLive;
    private MoveType _direction;
    private Transform _transform;

    protected void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void SetDirection(MoveType direction)
    {
        _direction = direction;
        if (_direction == MoveType.Left) Flip();
    }

    protected override void OnBeat()
    {
        if (_beatsToLive-- <= 0)
        {
            Destroy(gameObject);
        }

        var hitOther = TryMove(_direction);
        if (hitOther != null && !hitOther.CompareTag("Respawn") && hitOther.GetComponent<Projectile>() == null)
        {
            var enemy = hitOther.GetComponent<GenericEnemy>();
            if (enemy != null)
            {
                enemy.KillEnemy();
                Destroy(gameObject);
            }
        }
    }
}
