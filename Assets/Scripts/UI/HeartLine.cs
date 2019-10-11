using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLine : MonoBehaviour
{
    public Transform Destination;
    
    void Start(){
        LeanTween.moveX(gameObject, Destination.position.x, GameEngine.Instance.BeatFraction * 4).setOnComplete(() => Destroy(gameObject));
    }
}
