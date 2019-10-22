using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform _graphicsTransform;
    [SerializeField] private Transform _bodyTranform;
    protected Coroutine _coyoteCoroutine;
    protected bool _facingRight = true;

    [SerializeField] private GameObject _splash;
    private Transform _splashParent;
    private Vector3 _splashRelativePosition;

    protected virtual void Start()
    {
        if (_splash != null)
        {
            _splashParent = _splash.transform.parent;
            _splashRelativePosition = _splash.transform.localPosition;
            _splash.gameObject.SetActive(false);
        }
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

        var other = GameEngine.Instance.//Physics2D.OverlapCircle(new Vector2(futureCell.x + 0.5f, futureCell.y + 0.5f), 0.1f);
        var shouldMove = other == null || other.gameObject == gameObject || other.isTrigger;
        GameEngine.Instance.Populate(futureCell.x);
        GameEngine.Instance.Depopulate(transform.position.x);

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
        var startBodyPos = _bodyTranform.position;
        var startPos = transform.position;
        var endBodyPos = endPos + startBodyPos - startPos;
        if (successful)
        {
            transform.position = endPos;
        }
        _bodyTranform.position = startBodyPos;
        if ((endPos.x > startPos.x && !_facingRight) || (endPos.x < startPos.x && _facingRight))
        {
            Flip();
        }

        if (successful && CheckWater(endPos))
        {
            if (_splash != null)
            {
                _splash.gameObject.SetActive(true);
                _splash.transform.position = endPos + new Vector3(0.5f,0.5f,0);
                _splash.transform.parent = null;
            }
        }

        for (float time = 0; time < moveTime; time += Time.deltaTime)
        {
            var t = time / moveTime;
            if (t > 0.5f && !successful)
            {
                t = 1 - t;
            }
            _bodyTranform.position = Vector3.Lerp(startBodyPos, endBodyPos, t);
            yield return null;
        }

        transform.position = successful ? endPos : startPos;
        _bodyTranform.position = successful ? endBodyPos : startBodyPos;
        AfterMove();

        if (_splash != null)
        {
            _splash.gameObject.SetActive(false);
            _splash.transform.parent = _splashParent;
            _splash.transform.localPosition = _splashRelativePosition;
        }
    }

    protected IEnumerator CoyoteFrames()
    {
        float graceTime = GameEngine.Instance.BeatFraction / 2.5f;
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

    protected bool CheckWater(Vector3 position)
    {
        var floor = GameEngine.Instance.Tilemap.GetTile(GameEngine.Instance.Tilemap.WorldToCell(position));
        return (floor != null && (floor.name.StartsWith("water") && !floor.name.Contains("middle")));// || floor.name.Equals("water_alt") || floor.name.Equals("water_no_swap")));
    }
}