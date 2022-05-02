using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selecter : MonoBehaviour
{
    public GameObject lefted;
    public bool leftingon, leftingoff;
    RaycastHit hit;
    void Start()
    {
        leftingon = true;
        leftingoff = false;
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, Vector3.forward * 10, Color.yellow);
            if (Physics.Raycast(ray, out hit, 20))
            {
                if (lefted == null)
                {
                    if (hit.collider.tag == "pin")
                    {
                        lefted = hit.collider.gameObject;
                        lefted.GetComponent<moveup>().movetop();
                    }
                }
            }
            if (Physics.Raycast(ray, out hit, 20))
            {
                if (lefted != null)
                {
                    if (hit.collider.tag == "drop")
                    {
                        Vector3 next = hit.collider.gameObject.transform.position;
                        lefted.GetComponent<moveup>().dropdown(next);
                        lefted = null;
                    }
                }
            }
        }
    }
    public void resetting()
    {
        leftingon = true;
        leftingoff = false;
        lefted = null;
    }
}
