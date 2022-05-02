using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinAndFailUIManager : MonoBehaviour
{
    public static WinAndFailUIManager instance;

    public Image winEmoji;
    public TextMeshProUGUI winComplimentText;

    public List<Sprite> winEmojiList;
    public List<Sprite> failEmojiList;


    List<string> complimentStrings = new List<string> { "Marvelous !",
        "Beautiful !", "Incredible !", "Fabulous !", "Impressive !", "Awesome !"};

    private void Awake()
    {
        instance = this;
    }
    public void SetWinUI()
    {
        int emojiIndex = Random.Range(0, winEmojiList.Count);
        winEmoji.sprite = winEmojiList[emojiIndex];

        int stringIndex = Random.Range(0, complimentStrings.Count);
        winComplimentText.SetText(complimentStrings[stringIndex]);
    }
    
}
