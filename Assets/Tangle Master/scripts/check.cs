using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    private decider ropedecider;
    
    void Start()
    {
        ropedecider = GameObject.Find("ropedecider").GetComponent<decider>();
    }
    void Update()
    {
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "otherrope")
        {
            if (collision.gameObject.transform.parent == transform.parent)
            {
           
            }
            else {
                ropedecider.stillincontact(); 
            }
           
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "otherrope")
        {
            if (collision.gameObject.transform.parent == transform.parent)
            {
              //  Debug.Log(collision.gameObject.transform.parent+"<--->"+transform.parent);
              // Debug.Log("same parent 2.0");
            }
            else
            {
                ropedecider.onincontact();
            }
        }
    }
}
