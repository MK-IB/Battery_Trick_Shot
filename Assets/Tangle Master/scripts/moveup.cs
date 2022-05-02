using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public class moveup : MonoBehaviour
{
    public bool up, keepmoveing, xmov, dragon, swiped;
    public bool upmoveing, downmoveing;
    RaycastHit down;
    public Vector3 upvalue, downvalue;
    public Vector3 xmove;
    public Vector3 presentpos, droppos;
    public selecter selecter;
    public decider decider;
    public bool upp, downn;
    public int speed;
    public AudioSource aS;
    bool playAudio = false;
    void Start()
    {
        speed = 5;
        upvalue = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
        decider = GameObject.Find("ropedecider").GetComponent<decider>();
        selecter = GameObject.Find("Main Camera").GetComponent<selecter>();
        keepmoveing = false;
        dragon = false;
        xmov = true;
        up = true;
        aS = ui.Instance.sounds;
        //Audiomanager.Instance.BgAudio.enabled = true;
    }
    void Update()
    {
        if (Input.GetMouseButton(0)&&upp==false)
        {
         

            if (dragon)
            {
               
                Physics.Raycast(transform.position, Vector3.down, out down, 10f);
                Debug.DrawRay(transform.position, Vector3.down * 10, Color.yellow);
                float x = Input.GetAxis("Mouse X") * 5 * Mathf.Deg2Rad;
                x = Mathf.Clamp(x, -0.5f, 0.5f);
                transform.position += new Vector3(x, 0, 0);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.5f, 2.5f), transform.position.y, transform.position.z);
                if (presentpos.x == transform.position.x)
                {
                }
                else
                {
                    swiped = true;
                    if (down.collider != null)
                    {
                        if (down.collider.tag == "drop")
                        {
                            droppos = down.collider.transform.position;
                        }
                    }
                    else
                    {
                        droppos = presentpos;
                    }
                }
              

            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragon = false;
        //    Debug.Log("drag off");
            if (swiped)
            {
                dropdown(droppos);
            }
           
        }

        if (keepmoveing)
        {
            if (xmov)
            {
                transform.position = Vector3.MoveTowards(transform.position, xmove, speed * Time.deltaTime);
                if (transform.position == xmove)
                {
                  //  Debug.Log("x reached");
                    Physics.Raycast(transform.position, Vector3.down, out down, 10f);
                    if (down.collider.tag == "pin")
                    {
                        dropdown(presentpos);
                    }
                    else
                    {
                        downvalue = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);
                        downn = true;
                        reseting();
                    }
                }
            }
        }
        if (downn)
        {
         //   print("going down");
            transform.position = Vector3.MoveTowards(transform.position, downvalue, speed * Time.deltaTime);
            if (transform.position == downvalue)
            {
           //     print("down");
                downn = false;
                downvalue = new Vector3(0, 0, 0);
                xmove = new Vector3(0, 0, 0);
                upvalue = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
                decider.dropeed();
                ui.Instance.plugInSound.Play();
            }
        }

        if (upp)
        {
            transform.position = Vector3.MoveTowards(transform.position, upvalue, speed * Time.deltaTime);
            if (transform.position == upvalue)
            {
             //   Debug.Log("reached draging");
                up = false;
                upp = false;
            }
        }
    }
    private void OnMouseDown()
    {
            aS.Play();
    }
    public void reseting()
    {
        keepmoveing = false;
        dragon = false;
        xmov = true;
        up = true;
        swiped = false;
        upmoveing = false;
        presentpos = new Vector3(0, 0, 0);
        droppos = new Vector3(0, 0, 0);
    }
    public void movetop()
    {
        if (up)
        {
            presentpos = transform.position;
            dragon = true;
            upp = true;
            decider.lefteed();
        }
    }
    public void dropdown(Vector3 next)
    {
        selecter.resetting();
        xmove = new Vector3(next.x, transform.position.y, transform.position.z);
        keepmoveing = true;
        xmov = true;
        dragon = false;
    }
}