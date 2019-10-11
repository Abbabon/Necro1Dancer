using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MovingObject
{
    protected override void OnBeat()
    {
        MoveType stepDir = _moveSet[_moveIndex];

        _moveIndex = ++_moveIndex % _moveSet.Count;
    }
}
