using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpinghand : MonoBehaviour
{
    public GameObject blueplugin;
    public bool a, b;
    void Start()
    {
        a = true;
        b = false;
    }
    void Update()
    {
        if (blueplugin.GetComponent<moveup>().up == false&&a)
        {
            transform.position = new Vector3(-1.33f, transform.position.y, transform.position.z);
            a = false;
            b = true;
        }
        if (blueplugin.GetComponent<moveup>().up == true && b)
        {
            Destroy(gameObject);
        }
    }
}
