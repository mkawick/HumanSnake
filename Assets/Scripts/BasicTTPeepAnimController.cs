using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class BasicTTPeepAnimController : MonoBehaviour
{
    Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            animator.SetTrigger("Run");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetTrigger("Walk");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("IdleA");
        }
        else if (Input.GetKey(KeyCode.V))
        {
            animator.SetTrigger("Wave");
        }
    }
}

