using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLinesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _heartLinePrefab;
    [SerializeField] private Transform _destination;
    
    void Start()
    {
        GameEngine.Instance.Beat += OnBeat;
    }

    private void OnBeat() {
        GameObject heartLine = Instantiate(_heartLinePrefab, transform.position, Quaternion.identity);
        heartLine.transform.parent = transform;
        heartLine.GetComponent<HeartLine>().Destination = _destination;
        
    }
}
