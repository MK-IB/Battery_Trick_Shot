using DG.Tweening;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
	public bool isDead;

	public void OnDeath()
	{
		GetComponent<Collider>().enabled = false;
		isDead = true;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (isDead) return;

		if (!CompareTag("EnemyAttack")) return;
		
		if (!(other.gameObject.CompareTag("HitBox") || other.gameObject.CompareTag("Arm") ||
		      other.gameObject.CompareTag("Player"))) return;

		GameEvents.Only.InvokeEnemyHitPlayer(transform);

		transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => gameObject.SetActive(false));
		return;
	}
}
