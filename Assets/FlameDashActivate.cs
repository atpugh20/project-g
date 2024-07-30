using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDashActivate : MonoBehaviour{
    private Animator animator;

    void Start() => animator = GetComponent<Animator>();
    public void FlameDash() => animator.SetBool("isDashing", true);

    public void FlameDashOff() => animator.SetBool("isDashing", false);
}
