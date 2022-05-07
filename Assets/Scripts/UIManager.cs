using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public TextMeshProUGUI coinsText;
    public GameObject screenUIParent;
    public GameObject collectibleCoin;
    public RectTransform coinIconRectTransform;

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
        coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
    }

    IEnumerator ShowTutorialPanel()
    {
        yield return new WaitForSeconds(VirtualCameraManager.instance.gameStartTime);
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

    public void CoinCollectionEffect(Vector2 spawnPos)
    {
        GameObject coinInst = Instantiate(collectibleCoin, transform.position, Quaternion.identity);

        RectTransform coinInstRectTransform = coinInst.GetComponent<RectTransform>();
        coinInstRectTransform.transform.parent = screenUIParent.transform;
        coinInstRectTransform.position = spawnPos;
        coinInstRectTransform.DOScale(Vector3.one * 0.5f, 0.3f);
        coinInstRectTransform.DOMove(coinIconRectTransform.position, 0.3f).OnComplete(() =>
        {
            coinInstRectTransform.gameObject.SetActive(false);
            ShopDataHolder.instance.SetCoins(ShopDataHolder.instance.GetCoins() + 10);
            coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
        });
    }
}
