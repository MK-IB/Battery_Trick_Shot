using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public void CollectionEffect()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rotation>().speed = 400;
        transform.DOMove(transform.position + Vector3.up * 8, 0.5f);
        transform.DOScale(Vector3.one * 0.5f, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        AudioManager.instance.PlayClip(AudioManager.instance.coin);
    }
}
