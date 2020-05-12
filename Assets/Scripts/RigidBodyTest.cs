﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyTest : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    //bool running = false;
    public float runSpeed = 1;
    public bool isPlayer = false;
    public bool isLoggingEnabled = false;
    
    //bool pendingAnimationStateChange = false;
    Vector3 target;

    enum AnimStateChange
    {
        Run,
        Idle,
        None
    }
    AnimStateChange currentState = AnimStateChange.Idle, pendingStateChange = AnimStateChange.None;
    float animationTimeGate;
    public float animChangeLagTime = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");
    }

    internal void MovePlayer(Vector3 position)
    {
        if ((position - transform.position).magnitude > 0.1f)
        {
            FacePosition(position);
            MoveTowardPosition(position);
            StartRunning();
        }
    }
    internal void StopPlayer()
    {
        GoToIdle();
    }
    void Update()
    {
        if (isPlayer == false)
        {
            if (target == null || target == Vector3.zero)
            {
                Log("Update: target == null");
                switch (pendingStateChange)
                {
                    case AnimStateChange.Run:
                        StartRunning();
                        break;
                    case AnimStateChange.Idle:
                        GoToIdle();
                        break;
                    case AnimStateChange.None:
                        break;
                }
            }
            else
            {
                Log("Update: target is not null");
                if (MoveTowardPosition(target))
                {
                    StartRunning();
                }
                else
                {
                    GoToIdle();
                }
                FacePosition(target);
            }
        }
    }

    void FacePosition(Vector3 placeToLook)
    {
        Vector3 dist = placeToLook - transform.position;// do not turn for too close
        dist.y = 0;
        if (dist.magnitude > 0.8)
        {
            placeToLook.y = transform.position.y;
            transform.LookAt(placeToLook);
        }
    }

    void StartRunning()
    {
        Log("StartRunning");
        if (animationTimeGate < Time.time)
        {
            if (currentState == AnimStateChange.Idle)
            {
                Log("StartRunning - actual change");
                animator.SetTrigger("Run");

                animationTimeGate = Time.time + animChangeLagTime;
                currentState = AnimStateChange.Run;
                pendingStateChange = AnimStateChange.None;
            }
        }
        else
        {
            pendingStateChange = AnimStateChange.Run;
        }
    }
    void GoToIdle()
    {
        Log("GoToIdle");
        if (animationTimeGate < Time.time)
        {
            if (currentState == AnimStateChange.Run)
            {
                Log("GoToIdle - actual change");
                animator.SetTrigger("Idle");

                animationTimeGate = Time.time + animChangeLagTime;
                currentState = AnimStateChange.Idle;
                pendingStateChange = AnimStateChange.None;
            }
        }
        else
        {
            pendingStateChange = AnimStateChange.Idle;
        }
    }

    internal void Wave()
    {
        Log("Wave");
        animator.SetTrigger("Wave");
    }

    internal void Log(string text)
    {
        if (isLoggingEnabled == true)
            Debug.Log(text + ":" + Time.time);
    }
    bool MoveTowardPosition(Vector3 pos)
    {
        Vector3 dist = pos - transform.position;
        dist.y = 0;// a lot of times, the pos is random and the y is not in the plane
        if (dist.magnitude < 0.4f)
        {
            Log("MoveTowardPosition:false");
            return false;
        }
        else
        {
            //Log("MoveTowardPosition:true");
            //Log("MoveTowardPosition:true dist1 = " + dist);
            dist.Normalize();
            dist *= runSpeed;
            transform.position += dist * Time.deltaTime;
            Log("MoveTowardPosition:true dist2 = " + dist);
            return true;
        }
    }
        
    public void SetTarget(Transform t)
    {
        Log("SetTarget:Transform");
        target = t.position;
    }
    public void SetTarget(Vector3 pos)
    {
        Log("SetTarget:pos");
        target = pos;
    }
}
