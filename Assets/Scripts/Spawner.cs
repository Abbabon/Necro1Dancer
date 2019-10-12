using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Tooltip("The enemy to spawn")]
    [SerializeField] GenericEnemy _toSpawn;
    [Tooltip("Offset from spawner")]
    [SerializeField] Vector2Int _spawnOffset;
    [Tooltip("Should spawn at start or wait spawn time")]
    [SerializeField] bool _spawnOnStart;
    [Tooltip("Don't spawn new enemy until the previous died")]
    [SerializeField] bool _waitTillDeath;
    [Tooltip("Beats between spawns/after death/at start")]
    [SerializeField] int _spawnBeats;

    public bool Enabled { get; set; }

    private Vector3 _spawnOffsetVec3;
    private GenericEnemy _spawned;
    private int _beatsSinceLastSpawn;

    protected IEnumerator Start()
    {
        _spawnOffsetVec3 = new Vector3(_spawnOffset.x, _spawnOffset.y);

        GameEngine.Instance.Beat += OnBeat;
        
        if (_spawnOnStart)
        {
            Spawn();
        }

        Enabled = true;
        
        while (true)
        {
            yield return new WaitUntil(ShouldSpawn);
            Spawn();
        }
    }

    private bool ShouldSpawn()
    {
        if (!Enabled) return false;

        if (_waitTillDeath && _spawned != null) return false;

        if (_beatsSinceLastSpawn < _spawnBeats) return false;

        return true;
    }

    private void OnBeat()
    {
        _beatsSinceLastSpawn++;
    }

    private void Spawn()
    {
        _spawned = Instantiate(_toSpawn, 
                               transform.position + _spawnOffsetVec3, 
                               Quaternion.identity);
        _spawned.OnDeathEvent += SpawnedDied;
        _beatsSinceLastSpawn = 0;
    }

    private void SpawnedDied()
    {
        _spawned = null;
        if (_waitTillDeath)
        {
            _beatsSinceLastSpawn = 0;
        }
    }
}
