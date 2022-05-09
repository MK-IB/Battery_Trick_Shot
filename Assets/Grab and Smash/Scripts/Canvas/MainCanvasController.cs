using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainCanvasController : MonoBehaviour
{
	[SerializeField] private int lastRegularLevel;
	
	[SerializeField] private GameObject holdToAim, victory, defeat, nextLevel, retry, constantRetryButton, skipLevel;
	[SerializeField] private TextMeshProUGUI levelText, instructionText;
	public TextMeshProUGUI levelNumText;
	[SerializeField] private Image red;
	[SerializeField] private Toggle abToggle;
	[SerializeField] private string tapInstruction, swipeInstruction;

	[SerializeField] private Button nextLevelButton;

	private bool _hasTapped, _hasLost;

	private void OnEnable()
	{
		GameEvents.Only.EnemyKillPlayer += OnEnemyReachPlayer;
		GameEvents.Only.GameEnd += OnGameEnd;
	}

	private void OnDisable()
	{
		GameEvents.Only.EnemyKillPlayer -= OnEnemyReachPlayer;
		GameEvents.Only.GameEnd -= OnGameEnd;
	}

	private void Awake()
	{
		CommonUIEventsManager.instance.StartLevelStartEvent();
	}

	private void Start()
	{
		var levelNo = PlayerPrefs.GetInt("levelNo", 1);
		levelText.text = "Level " + levelNo;
		abToggle.isOn = PlayerPrefs.GetInt("controlMechanic", 0) != 0;
		instructionText.text = abToggle.isOn ? tapInstruction : swipeInstruction;
		
		if(levelNo < 5)
		{
			skipLevel.SetActive(false);
		}
		int currentLevel = PlayerPrefs.GetInt("levelnumber", 1);
		levelNumText.text = "Level " + currentLevel.ToString();

		PlayerPrefs.SetInt("lastRegularLevel", lastRegularLevel);
		
		if(GAScript.Instance)
			GAScript.Instance.LevelStart(PlayerPrefs.GetInt("levelNo", 0).ToString());
	}

	private void Update()
	{
		if(_hasTapped) return;
		
		if (!InputExtensions.GetFingerDown()) return;
		
		if(!EventSystem.current) { print("no event system"); return; }
		
		if(!EventSystem.current.IsPointerOverGameObject(InputExtensions.IsUsingTouch ? Input.GetTouch(0).fingerId : -1))
			TapToPlay();
	}

	IEnumerator EnableVictoryObjects()
	{
		if(!defeat.activeSelf)
		{
			victory.SetActive(true);
			constantRetryButton.SetActive(false);

			nextLevel.SetActive(true);
			CommonUIEventsManager.instance.StartLevelCompleteEvent();

			AudioManagerGrabAndSmash.instance.Play("Win");

			yield return new WaitForSeconds(3.5f);
			ISManager.instance.ShowInterstitialAds();
		}
	}

	private void EnableLossObjects()
	{
		if(victory.activeSelf) return;

		if (_hasLost) return;
		
		red.enabled = true;
		var originalColor = red.color;
		red.color = Color.clear;
		red.DOColor(originalColor, 1f);

		defeat.SetActive(true);
		retry.SetActive(true);
		skipLevel.SetActive(true);
		constantRetryButton.SetActive(false);
		_hasLost = true;
		
		AudioManagerGrabAndSmash.instance.Play("Lose");
	}

	public void EnableNextLevel()
	{
		nextLevelButton.interactable = true;
	}
	
	private void TapToPlay()
	{
		_hasTapped = true;
		holdToAim.SetActive(false);
		skipLevel.SetActive(false);
		
		if(GameEvents.Only)
			GameEvents.Only.InvokeTapToPlay();
	}

	public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		AudioManagerGrabAndSmash.instance.Play("Button");
		if (ISManager.instance)
		{
			ISManager.instance.ShowInterstitialAds();
		}
	}
	
	public void NextLevel()
	{
		if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
		{
			SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCountInBuildSettings - 1));
			PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
		}
		PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
		
		AudioManagerGrabAndSmash.instance.Play("Button");
		Vibration.Vibrate(15);
	}

	public void ABToggle(bool status)
	{
		InputHandler.Only.ShouldUseTapAndPunch(status);
	}

	private void OnEnemyReachPlayer()
	{
		/*if(GAScript.Instance)
			GAScript.Instance.LevelFail(PlayerPrefs.GetInt("levelNo").ToString());*/
		
		Invoke(nameof(EnableLossObjects), 1.5f);
		if (GAScript.Instance)
		{
			GAScript.Instance.LevelFail(PlayerPrefs.GetInt("levelnumber", 1).ToString());
		}
		if (ISManager.instance)
		{
			ISManager.instance.ShowInterstitialAds();
		}
	}
	public bool skipLvl;
	public void SkipLevel()
	{
		if(ISManager.instance)
			ISManager.instance.ShowInterstitialAds();
		NextLevel();
		//ISManager.instance.RewardCallBacks();
		/*if (skipLvl || !ISManager.instance)
			return;

		skipLvl = true;
		ISManager.instance.ShowRewardedVideo();*/
	}
	private void OnGameEnd()
	{
		/*if(GAScript.Instance)
			GAScript.Instance.LevelCompleted(PlayerPrefs.GetInt("levelNo").ToString());*/

		StartCoroutine(EnableVictoryObjects());
		if (GAScript.Instance)
		{
			GAScript.Instance.LevelCompleted(PlayerPrefs.GetInt("levelnumber", 1).ToString());
		}
	}

	private void AdRewardRecieveBehaviour()
	{
		if (PlayerPrefs.GetInt("levelNo", 1) < lastRegularLevel + 1)
		{
			var x = PlayerPrefs.GetInt("levelNo", 1) + 1;
			PlayerPrefs.SetInt("lastBuildIndex", x);
			SceneManager.LoadScene(x);
		}
		else
		{
			var x = Random.Range(5, lastRegularLevel + 1);
			PlayerPrefs.SetInt("lastBuildIndex", x);
			SceneManager.LoadScene(x);
		}

		PlayerPrefs.SetInt("levelNo", PlayerPrefs.GetInt("levelNo", 1) + 1);

		ShopStateController.ShopStateSerializer.SaveCurrentState();

		AudioManagerGrabAndSmash.instance.Play("Button");
		Vibration.Vibrate(15);
	}
}
