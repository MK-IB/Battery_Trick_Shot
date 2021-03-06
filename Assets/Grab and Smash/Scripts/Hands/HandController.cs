using System;
using DG.Tweening;
//using TMPro;
using UnityEngine;

public enum CarriedObjectType
{
	Ragdoll, Prop, Car
}

public class HandController : MonoBehaviour
{
	public bool isLeftHand;
	public Transform palm;
	[SerializeField] private Transform ragdollHoldingLocation, propHoldingLocation;
	[SerializeField] private float moveSpeed, returnSpeed, punchForce, carPunchForce;

	[SerializeField] private ParticleSystem windLines;
	
	public static CarriedObjectType CurrentObjectCarriedType;
	public bool isWaitingToGivePunch;
	
	[Header("Weapon Skins"), SerializeField] private WeaponType currentWeaponsSkin;
	[SerializeField] private GameObject phone;

	[Header("Arms Skins"), SerializeField] private MeshRenderer myArm;
	[SerializeField] private Material poppy;

	public static PlayerSoundController Sounds;

	private Animator _myAnimator;
	private static Animator _rootAnimator;
	private static RopeController _rope;
	private float _appliedMoveSpeed, _appliedReturnSpeed;
	private static bool _isCarryingBody;

	private Transform _lastTarget, _lastTargetRoot;
	private static RagdollController _lastRaghu;
	private Vector3 _palmInitLocalPos;
	private bool _isHandMoving;

	private static Vector3 _targetInitPos;
	private static bool _initPosSet;

	//private TextMeshProUGUI _text;
	//private string _testString;
	
	#region Animator Hashes
	private static readonly int Attack = Animator.StringToHash("attack");
	private static readonly int IsUsingHandsHash = Animator.StringToHash("isUsingHands");
	private static readonly int ChangeWeapon = Animator.StringToHash("changeWeapon");
	
	private static readonly int OpenFingers = Animator.StringToHash("openFingers");
	private static readonly int OpenAndCloseFingers = Animator.StringToHash("openAndCloseFingers");
	private static readonly int IsPunching = Animator.StringToHash("isPunching");
	private static readonly int IsHoldingAPhone = Animator.StringToHash("isHoldingAPhone");

#endregion
		
	#region  Helpers and Getters
	
	public AimController GetAimController() => transform.root.GetComponent<AimController>();
	public static void UpdateRope() => _rope.UpdateRope();

#endregion
	
	private void OnEnable()
	{
		if(!isLeftHand) //only for right/punching hand
		{
			GameEvents.Only.WeaponSelect += OnWeaponPurchased;

			GameEvents.Only.EnterHitBox += OnEnterHitBox;
		}
		
		GameEvents.Only.SkinSelect += OnSkinPurchased;
		GameEvents.Only.GiantPickupProp += OnGiantPickupCar;
		GameEvents.Only.PunchHit += OnPunchHit;
	}
	
	private void OnDisable()
	{
		if (!isLeftHand) //only for right/punching hand
		{
			GameEvents.Only.WeaponSelect -= OnWeaponPurchased;

			GameEvents.Only.EnterHitBox -= OnEnterHitBox;
		}
		
		GameEvents.Only.SkinSelect -= OnSkinPurchased;
		GameEvents.Only.GiantPickupProp -= OnGiantPickupCar;
		GameEvents.Only.PunchHit -= OnPunchHit;
	}
	
	private void Start()
	{
        _myAnimator = GetComponent<Animator>();

		if (isLeftHand)
				_rope = GetComponent<RopeController>();

		if(!_rootAnimator)
			_rootAnimator = transform.root.GetComponent<Animator>();
		
		Sounds = _rootAnimator.GetComponent<PlayerSoundController>();
		
		_initPosSet = false;
		_palmInitLocalPos = palm.localPosition;
		_lastRaghu = null;
		ResetPalmParent();
		ClearInitTargetPos();
		StopPunching();
		
		if(isLeftHand)
		{
			_appliedMoveSpeed = moveSpeed * (1f + ShopStateController.CurrentState.GetCurrentSpeedLevel() / 10f);
			_appliedReturnSpeed = returnSpeed * (1f + ShopStateController.CurrentState.GetCurrentSpeedLevel() / 10f);
		}
		
		UpdateEquippedArmsSkin();
		
		if (isLeftHand) return;
		
		UpdateEquippedWeaponsSkin();

		//_text = GameObject.FindGameObjectWithTag("AimCanvas").transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
	}

	public void MoveRopeEndTowards(RaycastHit hit, bool goHome = false)
	{
		if (goHome)
		{
			if (_isCarryingBody)
			{
				palm.localPosition =
					Vector3.MoveTowards(palm.localPosition, Vector3.zero, _appliedReturnSpeed * Time.deltaTime);

				return;
			}
			
			palm.localPosition = 
				Vector3.MoveTowards(palm.localPosition,
				Vector3.zero, 
				returnSpeed * Time.deltaTime);
		}
		else
		{
			if (!_initPosSet)
			{
				_lastTarget = hit.transform;
				_lastTargetRoot = _lastTarget.root;
				_lastTargetRoot.TryGetComponent(out _lastRaghu);
				_targetInitPos = hit.transform.position;
				_initPosSet = true;
				
				Sounds.PlaySound(Sounds.ziplineLeave, 1f);
			}
			
			palm.position =
				Vector3.MoveTowards(palm.position,
					_lastTarget.position,
				 _appliedMoveSpeed * Time.deltaTime);
		}
	}

	public void HandReachTarget(Transform other)
	{
		if (_isCarryingBody) return;
		if(isLeftHand)
		{
			if (!InputHandler.Only.CanSwitchToTargetState()) return;
			
			StartCarryingBody(other);
			if (other.TryGetComponent(out RagdollLimbController raghu))
			{
				if (!raghu.TellParent())
				{
					//punch sfx
					InputHandler.AssignNewState(new InTransitState(true, InputStateBase.EmptyHit, false));
					Vibration.Vibrate(15);

					ClearStateInfo();
					return;
				}

				ClearStateInfo();
				StartCarryingBody(other);
			}
			else if (CurrentObjectCarriedType == CarriedObjectType.Car)
				other.GetComponent<CarController>().StopMoving();
			if (other.TryGetComponent(out PropController prop)) 
				prop.hasBeenInteractedWith = true;
		
			
			InputHandler.AssignNewState(new InTransitState(true, InputStateBase.EmptyHit, 
				true));
		}
		else
		{
			InputHandler.AssignNewState(new InTransitState(true, InputStateBase.EmptyHit, false));
			_targetInitPos.y = other.position.y;
			if(CurrentObjectCarriedType == CarriedObjectType.Ragdoll)
			{
				other.GetComponent<RagdollLimbController>().GetPunched((
					(LevelFlowController.only.TryGetCurrentThrowTarget(out var target)
						? target.position : _targetInitPos) - transform.position).normalized, punchForce);
			}
			else
			{
				_targetInitPos.y = other.position.y;
				
				if(!other.root.TryGetComponent(out PropController prop))
					prop = other.GetComponent<PropController>();

				var direction =
					(LevelFlowController.only.TryGetCurrentThrowTarget(out var target)
						? target.position
						: _targetInitPos) - other.root.position;

				prop.GetPunched(direction.normalized, 
						CurrentObjectCarriedType == CarriedObjectType.Car ? carPunchForce : punchForce);
			}
		}

		Vibration.Vibrate(15);
	}
	
	public void WaitForPunch(Transform other)
	{
		if (!other) return;
		
		InputHandler.Only.StopCarryingBody();

		var root = other.root;

		if (CurrentObjectCarriedType == CarriedObjectType.Ragdoll)
		{
			root.transform.DOMove(ragdollHoldingLocation.position, 0.2f);
			root.transform.DORotateQuaternion(Quaternion.LookRotation(transform.root.position - root.position) * Quaternion.Euler(Vector3.left * 20f), 0.2f);
		}
		else
		{
			root.transform.DOMove(propHoldingLocation.position, 0.2f);
		}

		_myAnimator.SetBool(IsPunching, true);
		isWaitingToGivePunch = true;
		_rope.ReturnHome();
		windLines.Play();
	}

	public void HandReachHome()
	{
		if(!InputHandler.IsInDisabledState())
			InputHandler.AssignNewState(InputHandler.IdleState);
	}

	private void StartCarryingBody(Transform target)
	{
		_isCarryingBody = true;
		
		if(target.TryGetComponent(out RagdollLimbController raghu))
		{
			raghu.AskParentForHook().transform.root.parent = palm;
			CurrentObjectCarriedType = CarriedObjectType.Ragdoll;
		}
		else
		{
			target.transform.parent = palm;
			CurrentObjectCarriedType = target.TryGetComponent(out CarController _) ? CarriedObjectType.Car : CarriedObjectType.Prop;
		}
	}

	public void StopCarryingBody()
	{
		ResetPalmParent();
	}

	public void UpdatePullingSpeed(int level)
	{
		_appliedMoveSpeed = moveSpeed * (1f + level / 10f);
		_appliedReturnSpeed = returnSpeed * (1f + level / 10f);
	}

	public void UpdateEquippedWeaponsSkin(bool initialising = true, int newWeapon = -1)
	{
		//_text.text = "" + SkinLoader.GetSkinName() + $", number {PlayerPrefs.GetInt("currentWeaponSkinInUse", 0)} out of {SkinLoader.only.GetSkinCount()}"; 
		currentWeaponsSkin = (WeaponType) (newWeapon == -1 ? ShopStateController.CurrentState.GetCurrentWeapon() : newWeapon);
		for (var i = 1; i < phone.transform.parent.childCount; i++)
			phone.transform.parent.GetChild(i).gameObject.SetActive(false);

		if(!initialising)
		{
			_rootAnimator.SetTrigger(ChangeWeapon);
			_myAnimator.SetTrigger(ChangeWeapon);
		}
		
		switch (currentWeaponsSkin)
		{
			case WeaponType.Punch:
				_myAnimator.SetTrigger(OpenAndCloseFingers);
				_rootAnimator.SetTrigger(IsUsingHandsHash);
				break;
			case WeaponType.Phone:
				phone.SetActive(true);
				_myAnimator.SetTrigger(OpenFingers);
				_rootAnimator.SetTrigger(IsHoldingAPhone);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void UpdateEquippedArmsSkin()
	{
		myArm.material = (ArmsType) ShopStateController.CurrentState.GetCurrentArmsSkin() switch
		{
			ArmsType.Poppy => poppy,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	public bool TryGivePunch()
	{
		if (!isWaitingToGivePunch) return false;
		
		isWaitingToGivePunch = false;
		_rootAnimator.SetTrigger(Attack);
		Sounds.PlaySound(Sounds.clickForPunch, 1);

		return true;
	}

	private static void ClearInitTargetPos()
	{
		_initPosSet = false;
	}
	
	public void StopPunching()
	{
		_myAnimator.SetBool(IsPunching, false);
		ClearInitTargetPos();
		
		
		_lastRaghu = null;
		_lastTarget = null;
		_lastTargetRoot = null;
		ResetPalmParent();
		ClearInitTargetPos();
	}

	private void ResetPalmParent()
	{
		if(!isLeftHand) return;
		
		if(palm.childCount > 2)
			palm.GetChild(2).parent = null;
		
		palm.DOLocalMove(Vector3.zero, 0.2f).OnComplete(() => palm.localPosition = _palmInitLocalPos);
		_isCarryingBody = false;
	}
	
	public void InformAboutRagdollDeath(RagdollController ragdollController)
	{
		if (_lastRaghu != ragdollController) return;
		ClearStateInfo();
	}

	private void ClearStateInfo()
	{
		ClearInitTargetPos();
		ResetPalmParent();
		InputHandler.AssignReturnTransitState();
	}

	private void OnWeaponPurchased(int index, bool shouldDeductCoins)
	{
		if (isLeftHand) return;
		
		UpdateEquippedWeaponsSkin(false, index);
	}

	private void OnSkinPurchased(int index, bool shouldDeductCoins)
	{
		DOVirtual.DelayedCall(0.05f, UpdateEquippedArmsSkin);
	}
	
	private void OnGiantPickupCar(Transform car)
	{
		if (_lastTarget != car) return;
		
		_isCarryingBody = false;
		ClearStateInfo();
	}
	
	private void OnEnterHitBox(Transform target)
	{
		if(!isLeftHand) return;

		ResetPalmParent();
	}

	public void OnPropDestroyed()
	{
		_isCarryingBody = false;
		ClearStateInfo();
	}
	
	private void OnPunchHit()
	{
		windLines.Stop();
	}

	private void OnGameEnd()
	{
		OnPropDestroyed();
	}
}