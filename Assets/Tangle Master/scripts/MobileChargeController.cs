using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobileChargeController : MonoBehaviour
{
    public GameObject chargingScreen;

    public Image percentageWheel;
    public TextMeshProUGUI percentageText;
    public GameObject sparkEx;
    private void Start()
    {
        EventsManager.instance.TangleSolvedEvent += StartCharging;
    }

    void StartCharging()
    {
        StartCoroutine(ChangeToChargingScreen());
    }
    IEnumerator ChangeToChargingScreen()
    {
        yield return new WaitForSeconds(0.5f);
        sparkEx.SetActive(true);
        chargingScreen.SetActive(true);

        float targetPerc = (float)System.Math.Round(UnityEngine.Random.Range(0.5f, 1.0f), 2);
        float chargePerc = 0f;
        while (chargePerc < targetPerc)
        {
            percentageWheel.fillAmount = chargePerc;
            percentageText.SetText(Mathf.Round(chargePerc * 100).ToString() + "%");
            chargePerc += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
