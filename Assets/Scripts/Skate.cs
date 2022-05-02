using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Skate : MonoBehaviour
{
    public Transform destination;
    public float speed;
    public bool move;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile") && !GameManager.instance.gameOver)
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            Transform mobileTransform = other.transform;
            mobileTransform.parent = transform;
            AudioManager.instance.PlayClip(AudioManager.instance.obstacleHit);
            mobileTransform.DOLocalRotate(new Vector3(-90, 0, 180), 0.5f);
            mobileTransform.DOMove(transform.GetChild(0).position, 0.1f);
            //transform.DOMove(new Vector3(destination.position.x, destination.position.y, destination.position.z), 3);
            move = true;
        }
    }

    private void Update()
    {
        if(move)
            transform.position += transform.right * speed * Time.deltaTime;
    }
}
