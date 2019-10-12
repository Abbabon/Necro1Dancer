using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform _graphicsTransform;
    protected Coroutine _coyoteCoroutine;
    protected bool _facingRight = true;

    protected virtual void Start()
    {
        GameEngine.Instance.Beat += OnBeat;
    }

    protected void OnDestroy()
    {
        GameEngine.Instance.Beat -= OnBeat;
    }

    protected abstract void OnBeat();

    protected Collider2D TryMove(MoveType move)
    {
        var tilemap = GameEngine.Instance.Tilemap;
        var futureCell = tilemap.CellToWorld(tilemap.WorldToCell(transform.position) + MakeStep(move));

        var other = Physics2D.OverlapCircle(new Vector2(futureCell.x + 0.5f, futureCell.y + 0.5f), 0.1f);
        var shouldMove = other == null || other.gameObject == gameObject || other.isTrigger;
        StartCoroutine(MoveCoroutine(futureCell, shouldMove));
        return other;
    }

    private Vector3Int MakeStep(MoveType stepDir)
    {
        switch (stepDir)
        {
            case (MoveType.Down):
                return new Vector3Int(0, -1, 0);
            case (MoveType.Left):
                return new Vector3Int(-1, 0, 0);
            case (MoveType.Right):
                return new Vector3Int(1, 0, 0);
            case (MoveType.Up):
                return new Vector3Int(0, 1, 0);
            default:
                return Vector3Int.zero;
        }
    }

    private IEnumerator MoveCoroutine(Vector3 endPos, bool successful)
    {
        var moveTime = GameEngine.Instance.BeatFraction / 3f;
        var startPos = transform.position;

        for (float time = 0; time < moveTime; time += Time.deltaTime)
        {
            var t = time / moveTime;
            if (t > 0.5f && !successful)
            {
                t = 1 - t;
            }
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = successful ? endPos : startPos;
        AfterMove();
    }

    protected IEnumerator CoyoteFrames()
    {
        float graceTime = GameEngine.Instance.BeatFraction / 3.5f;
        for (float time = 0; time < graceTime; time += Time.deltaTime)
        {
            yield return null;
        }
        AfterMove();
        _coyoteCoroutine = null;
    }

    protected virtual void AfterMove() { }

    protected void Flip()
    {
        _graphicsTransform.localScale = new Vector3(_graphicsTransform.localScale.x * -1, _graphicsTransform.localScale.y, _graphicsTransform.localScale.z);
        _facingRight = !_facingRight;
    }
}