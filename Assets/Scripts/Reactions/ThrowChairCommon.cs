using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrowChairCommon : MonoBehaviour
{
    public static ThrowChairCommon instance;

    public GameObject chair;

    private void Awake()
    {
        instance = this;
    }

    /*public void ThrowChair()
    {
        chair.SetActive(true);
        chair.transform.DOMove(Camera.main.transform.position, 0.5f).OnComplete(() =>
        {
            chair.SetActive(false);
            UIManager.instance.glassBreakPanel.SetActive(true);
            GameManager.instance.LevelFailed(2);
        });
    }*/
}
