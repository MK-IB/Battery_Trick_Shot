using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopHitReaction : MonoBehaviour
{
    public GirlHitReaction girlHitReaction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile"))
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(girlHitReaction.StopMobile(other.gameObject));
            other.GetComponent<PhoneMovement>().BreakScreen();
            AudioManager.instance.bgAudioSource.enabled = false;
            Vibration.Vibrate(27);
        }
    }
}
