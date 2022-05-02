using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationSetter : MonoBehaviour
{
    Animator animator;
    public int actionInt;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("actions", actionInt);
    }
}
