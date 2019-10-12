using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GenericEnemy : MovingObject
{
    [FormerlySerializedAs("IsCollectable")]
    [SerializeField] bool _isCollectable;
    public bool IsCollectable { get { return _isCollectable; } }
    [SerializeField] List<MoveType> _moveSet;
    [SerializeField] int _beatsToLive;

    public Action OnDeathEvent;

    private int _moveIndex = 0;
    private int _beatsAlive;

    protected override void OnBeat()
    {
        if (_beatsToLive > 0 && ++_beatsAlive > _beatsToLive)
        {
            KillEnemy();
            return;
        }
        
        var hitOther = TryMove(_moveSet[_moveIndex]);
        if (hitOther == null)
        {
            _moveIndex = ++_moveIndex % _moveSet.Count;
        }
        else if (hitOther.CompareTag("Player"))
        {
            GameEngine.Instance.LoseHealth();
        }
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
