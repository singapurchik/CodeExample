using FAS.Players;
using UnityEngine;
using Zenject;

public class Rain : MonoBehaviour
{
	[SerializeField] private float _moveInterval = 0.25f;
	
	[Inject] private IPlayerInfo _target;

	private float _nextMoveTime;

	private void Move()
	{
		var targetPosition = _target.Position;
		targetPosition.y = transform.position.y;
		transform.position = targetPosition;
	}

	private void Update()
	{
		if (Time.timeSinceLevelLoad > _nextMoveTime)
		{
			Move();
			_nextMoveTime = Time.timeSinceLevelLoad + _moveInterval;
		}
	}
}
