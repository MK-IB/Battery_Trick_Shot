using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("mobile"))
        {
            print("player collision");
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
