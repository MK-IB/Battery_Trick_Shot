using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Jumper : MonoBehaviour
{
    public float forceAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("mobile"))
        {
            other.GetComponent<SplineFollower>().enabled = false;
            other.GetComponent<Collider>().isTrigger = false;
            Rigidbody mobileRb = other.GetComponent<Rigidbody>();
            mobileRb.isKinematic = false;
            mobileRb.AddForce(new Vector3(0, 0.2f, 1) * forceAmount, ForceMode.Impulse);
            Vibration.Vibrate(17);
        }
    }
}
