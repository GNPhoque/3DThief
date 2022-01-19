using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resident : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    void Start()
    {
        animator.SetBool("Sleep", true);
    }
}
