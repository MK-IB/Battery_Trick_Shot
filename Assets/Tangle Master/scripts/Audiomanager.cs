using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Audiomanager Instance;
    public AudioSource BgAudio;
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            DestroyImmediate(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
