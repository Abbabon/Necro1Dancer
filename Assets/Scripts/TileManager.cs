using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileManager : MonoBehaviour
{
    [Serializable]
    private class TilePair
    {
        public Tile tile1;
        public Tile tile2;
    }

    [SerializeField] TilePair[] _swapPairs;
    private bool _invertSwap;

    private Tilemap _tilemap;

    protected void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        GameEngine.Instance.Beat += OnBeat;        
    }

    private void OnBeat()
    {
        foreach (var pair in _swapPairs)
        {
            if (pair.tile1 == null || pair.tile2 == null) continue;

            var tile1 = _invertSwap ? pair.tile2 : pair.tile1;
            var tile2 = _invertSwap ? pair.tile1 : pair.tile2;

            _tilemap.SwapTile(tile1, tile2);
        }

        _invertSwap = !_invertSwap;
    }
}
