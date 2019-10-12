using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationRelayer : MonoBehaviour
{
    [SerializeField] PlayerMovementController _papa;

    public void DoneJumping()
    {
        _papa.DoneJumping();
    }
}
