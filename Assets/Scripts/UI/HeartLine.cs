using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLine : MonoBehaviour
{
    public Transform Destination;
    
    void Start()
    {
        float tweenTime = GameEngine.Instance.BeatFraction * 4;
        LeanTween.moveX(gameObject, Destination.position.x, tweenTime);
        LeanTween.scaleY(gameObject, 1f, tweenTime+0.1f).setOnComplete(() => Destroy(gameObject));
    }
}
