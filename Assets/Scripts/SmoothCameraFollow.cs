using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour 
{
	[SerializeField] Transform _followTarget;
	[SerializeField] float _followLerp;

	private Vector3 _posOffset;

	private Transform _transform;

	protected void Awake()
	{
		_transform = GetComponent<Transform>();
	}

	protected void Start()
	{
		_posOffset = _transform.position - _followTarget.position;
	}

	protected void LateUpdate()
	{
		var newFollowPosition = _followTarget.position + _posOffset;

		var newPosition = Vector3.Lerp(_transform.position, newFollowPosition, _followLerp);
		_transform.position = newPosition;
	}
}
