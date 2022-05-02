using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupHit : MonoBehaviour
{
    public GameObject cupBreak1, cupBreak2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile"))
        {
            GetComponent<Collider>().enabled = false;
            
            cupBreak1.SetActive(true);
            cupBreak2.SetActive(true);
            Vibration.Vibrate(27);
            gameObject.SetActive(false);
        }
    }
}
