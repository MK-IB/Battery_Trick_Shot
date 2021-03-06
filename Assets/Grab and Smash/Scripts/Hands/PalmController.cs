using DG.Tweening;
using UnityEngine;

public class PalmController : MonoBehaviour
{
	[SerializeField] private HandController myHand;

	[SerializeField] private float punchWaitTime = 1f;

	private static Transform _lastPickedTarget;
	private static bool _canAdopt = true;
	private static int _punchIndex;
	
	private void OnEnable()
	{
		if(myHand.isLeftHand)
		{
			GameEvents.Only.PropDestroyed += OnPropDestroyed;
			GameEvents.Only.GiantPickupProp += OnPropDestroyed;
			GameEvents.Only.DropArmor += OnDropArmor;
		}
		else
		{
			GameEvents.Only.PunchHit += OnPunchHit;
		}
	}

	private void OnDisable()
	{
		if(myHand.isLeftHand)
		{
			GameEvents.Only.PropDestroyed -= OnPropDestroyed;
			GameEvents.Only.GiantPickupProp -= OnPropDestroyed;
			GameEvents.Only.DropArmor -= OnDropArmor;
		}
		else
		{
			GameEvents.Only.PunchHit -= OnPunchHit;
		}
	}

	private void Start()
	{
		_lastPickedTarget = null;
		_canAdopt = true;
	}

	private void EnablePunching() => myHand.StopPunching();

	private void ResetAdoptability() => _canAdopt = true;

	private void OnTriggerEnter(Collider other)
	{
		if(!_canAdopt) return;
		if (!myHand.isLeftHand) return;
		if(!other.CompareTag("Target")) return;

		if (other.TryGetComponent(out PropController prop))
		{
			prop.PlayerPicksUp();
			prop.TryShowAds();
			if (prop.IsACompositeProp)
			{
				prop.MakeKinematic();
				prop.GetTouchedComposite(Vector3.up, false);
			}
		}

		if(HasTargetTransform())
		{
			InputHandler.AssignReturnTransitState();
			DOVirtual.DelayedCall(punchWaitTime, EnablePunching);
			DOVirtual.DelayedCall(0.5f, ResetAdoptability);
			return;
		}
		
		SetCurrentTransform(other.transform);
		myHand.HandReachTarget(other.transform);
		_canAdopt = false;
	}

	private void OnDropArmor()
	{
		SetCurrentTransform(null);
		Invoke(nameof(ResetAdoptability), 0.5f);
	}
	
	private void OnPunchHit()
	{
		if (!GetCurrentTransform())
		{
			InputHandler.Only.AssignIdleState();
			
			DOVirtual.DelayedCall(punchWaitTime, EnablePunching);
			DOVirtual.DelayedCall(0.5f, ResetAdoptability);
			return;
		}
		
		myHand.HandReachTarget(GetCurrentTransform());

		var trans = GetCurrentTransform();

		SetCurrentTransform(null);
		
		DOVirtual.DelayedCall(punchWaitTime, EnablePunching);
		DOVirtual.DelayedCall(0.5f, ResetAdoptability);
		HandController.Sounds.PlaySound(HandController.Sounds.punch[_punchIndex++ % HandController.Sounds.punch.Length], 1f);
		
		//this is for climber level
		ShatterableParent.AddToPossibleShatterers(trans.root);
	}

	private void OnPropDestroyed(Transform target)
	{
		if(target != GetCurrentTransform()) return;
		
		SetCurrentTransform(null);
		myHand.OnPropDestroyed();
		Invoke(nameof(ResetAdoptability), 0.5f);
	}

	private static bool HasTargetTransform() => _lastPickedTarget;
	private static Transform GetCurrentTransform() => _lastPickedTarget;
	private static void SetCurrentTransform(Transform newT) => _lastPickedTarget = newT;
}