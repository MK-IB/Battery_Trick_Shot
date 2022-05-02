using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RemoveAnimRig : MonoBehaviour
{
    public GameObject loverCallout;
    public GameObject millionaireCallout;
    public GameObject breakupCallout;
    public GameObject scoldCallout;
    public GameObject heartParticle;
    public void RemoveRigAnd()
    {
        GetComponent<RigBuilder>().enabled = false;
        loverCallout.SetActive(false);
        millionaireCallout.SetActive(false);
        breakupCallout.SetActive(true);
        heartParticle.SetActive(false);
    }
    
    public void Scold(Transform lover)
    {
        lover.position = new Vector3(lover.position.x, 0.5f, lover.position.z);
        GetComponent<RigBuilder>().enabled = false;
        scoldCallout.SetActive(true);
        loverCallout.SetActive(false);
        millionaireCallout.SetActive(false);
        heartParticle.SetActive(false);
    }
}
