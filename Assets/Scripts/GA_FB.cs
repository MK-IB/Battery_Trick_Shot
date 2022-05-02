using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;


public class GA_FB : MonoBehaviour
{
    public static GA_FB instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            DestroyImmediate(this.gameObject);

        GameAnalytics.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LevelStart(string levelname)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelname);
       // FlurryStart.instance.LevelStart(levelname);
    }

    public void LevelFail(string levelname)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelname);
      //  FlurryStart.instance.LevelFail(levelname);
    }

    public void LevelComplete(string levelname )
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelname);
     //   FlurryStart.instance.LevelComplete(levelname);
    }

   
}
