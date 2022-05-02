using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerController : MonoBehaviour
{
	private HealthController _health;
	
	private void OnEnable()
	{
		GameEvents.Only.EnemyHitPlayer += OnEnemyHitPlayer;
		GameEvents.Only.ReachNextArea += OnReachNextArea;
	}

	private void OnDisable()
	{
		GameEvents.Only.EnemyHitPlayer -= OnEnemyHitPlayer;
		GameEvents.Only.ReachNextArea -= OnReachNextArea;
	}

	private void Start()
	{
		_health = GetComponent<HealthController>();
		_health.VisibilityToggle(false);
	}

	private void OnEnemyHitPlayer(Transform hitter)
	{
		if(!_health.AddHit()) return;

		CameraController.only.ScreenShake(3f);
		
		Vibration.Vibrate(20);

		if (!_health.IsDead()) return;
		
		GameEvents.Only.InvokeEnemyKillPlayer();
	}

	private void OnReachNextArea()
	{
		if(!LevelFlowController.only.IsInGiantFight()) return;
		_health.VisibilityToggle(true);
		_health.hitsRequiredToKill = 3;
	}
}