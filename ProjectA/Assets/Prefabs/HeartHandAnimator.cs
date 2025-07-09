using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class HeartHandAnimator : MonoBehaviour
{
    Animator handAnimator;
    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }
    public void ResetAnimation()
    {
        handAnimator.SetBool("isAttacking", false);
    }
}
