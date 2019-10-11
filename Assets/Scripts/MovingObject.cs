using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MovingObject : MonoBehaviour
    {
        public bool IsCollectable;
        [SerializeField] public List<MoveType> _moveset;
        private int _moveIndex; 
        
        private void Start()
        {
            GameEngine.Instance.Beat += OnBeat;
        }


        public virtual void OnBeat()
        {
               
        }
    }
}