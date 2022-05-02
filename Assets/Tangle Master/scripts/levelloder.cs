using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelloder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
