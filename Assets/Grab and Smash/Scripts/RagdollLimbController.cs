using DG.Tweening;
using UnityEngine;

public class RagdollLimbController : MonoBehaviour
{
	private RagdollController _parent;
	private HostageController _hostage;
	
	private Rigidbody _rb;
	
	private void Start()
	{
		if (!transform.root.TryGetComponent(out _parent))
			_hostage = transform.root.GetComponent<HostageController>();

		_rb = GetComponent<Rigidbody>();
	}

	public bool TellParent()
	{
		DropContainer();
		
		if(_parent.touchToKill)
		{
			_parent.GoRagdoll(Vector3.zero);
			//tell palm to clear targets in a delay
			GameEvents.Only.InvokePunchHit();
			return false;
		}
		
		return _parent.TryHoldInAir();
	}

	public void DisableRagdolling() => _parent.isAttackerSoCantRagdoll = true;
	
	public void GetPunched(Vector3 direction, float punchForce)
	{
		if (_parent)
			_parent.GoRagdoll(direction);
		else
			_hostage.GoRagdoll(direction);
		
		_rb.AddForce(direction * punchForce + Vector3.up * punchForce / 3, ForceMode.Impulse);
	}

	private void DropContainer()
	{
		return;
	}
	
	public Rigidbody AskParentForHook() => _parent.chest;
	public bool IsRaghuRagdolling() => _parent.isRagdoll;
	public bool IsRaghuWaitingForPunch() => _parent.isWaitingForPunch;

	public void Attack(Vector3 endPos)
	{
		if (_parent.isRagdoll) return;

		endPos.y = transform.root.position.y;
		_parent.transform.DOMove(endPos, 0.5f);
		_parent.AttackEnemy();
		GameEvents.Only.InvokeEnemyKillPlayer();
	}
	
	private void OnCollisionEnter(Collision other)
	{
		if(!_parent) return;
	
		if(!_parent.isRagdoll) return;

		if(other.transform.root == transform.root) return;
		if (!other.collider.CompareTag("Target")) return;
		
		var direction = other.transform.position - transform.position;
		if (_rb.velocity.sqrMagnitude < 4f)
		{
			if(_parent.isAttackerSoCantRagdoll) return;
			
			GetPunched(-direction, 0.25f);
			return;
		}
		if (other.gameObject.TryGetComponent(out RagdollLimbController raghu) && !raghu._parent.isWaitingForPunch)
		{
			if (raghu._parent.IsInPatrolArea())
				raghu.GetPunched(direction, direction.magnitude);
		}
		else
		{
			if(!other.gameObject.TryGetComponent(out PropController prop)) return;
			
			if(prop.IsACompositeProp)
				prop.GetTouchedComposite(prop.transform.position - transform.position, true);
			
			if(prop.shouldExplode)
				prop.Explode();
		}
	}
}
