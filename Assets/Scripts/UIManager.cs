using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject skipButton;
    public TextMeshProUGUI levelNumText;
    public GameObject tutorialPanel;
    public GameObject glassBreakPanel;
    public GameObject tapScalePanel;

    public bool isTapScaleLevel;

    [HideInInspector] public bool hideTutorial;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("levelnumber", 1);
        levelNumText.text = "Level " + currentLevel.ToString();
        if (currentLevel == 1 && tutorialPanel)
        {
            StartCoroutine(ShowTutorialPanel());
        }
        if(isTapScaleLevel) tapScalePanel.SetActive(true);
    }

    IEnumerator ShowTutorialPanel()
    {
        yield return new WaitForSeconds(VirtualCameraManager.instance.gameStartTime + 0.5f);
        tutorialPanel.SetActive(true);
        hideTutorial = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hideTutorial)
        {
            tutorialPanel.SetActive(false);
            hideTutorial = false;
        }
    }
}
