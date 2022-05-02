using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class levelnumber : MonoBehaviour
{
    private TextMeshProUGUI levelNumText;
    private int levelnum;
    
    void Start()
    {
        levelNumText = GetComponent<TextMeshProUGUI>();
        int currentLevel = PlayerPrefs.GetInt("levelnumber", 1);
        levelNumText.text = "Level " + currentLevel.ToString();
    }
    void Update()
    {
        
    }
}
