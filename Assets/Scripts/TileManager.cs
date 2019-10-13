using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileManager : MonoBehaviour
{
    [Serializable]
    private class TileSequence
    {
        public TileBase[] tiles;
        [HideInInspector] public int currentIndex;
    }

    [SerializeField] TileSequence[] _swapSequences;

    private Tilemap _tilemap;

    protected void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        GameEngine.Instance.Beat += OnBeat;        
    }

    private void OnBeat()
    {
        foreach (var sequence in _swapSequences)
        {
            var nextIndex = sequence.currentIndex + 1;
            if (nextIndex == sequence.tiles.Length) nextIndex = 0;

            var tile1 = sequence.tiles[sequence.currentIndex];
            var tile2 = sequence.tiles[nextIndex];

            _tilemap.SwapTile(tile1, tile2);

            sequence.currentIndex = nextIndex;
        }
    }
}
