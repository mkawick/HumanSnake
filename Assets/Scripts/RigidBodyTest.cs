using System.Collections;
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

    void Update()
    {
        if (isPlayer == true)
        {
            if (Input.GetMouseButton(1) == true)
            {
                Wave();
            }
            else if (Input.GetMouseButton(0) == true)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    FaceMousePosition(hit.point);
                    MoveTowardPosition(hit.point);
                }

                StartRunning();
            }
            else
            {
                GoToIdle();
            }
        }
        else
        {
            if (target == null || target == Vector3.zero)
            {
                    switch(pendingStateChange)
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
                if (MoveTowardPosition(target))
                {
                    StartRunning();
                }
                else
                {
                    GoToIdle();
                }
                FaceMousePosition(target);
            }
        }
    }

    void FaceMousePosition(Vector3 placeToLook)
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
        animator.SetTrigger("Wave");
    }

    void Log(string text)
    {
        if (isLoggingEnabled == true)
            Debug.Log(text + ":" + Time.time);
    }
    bool MoveTowardPosition(Vector3 pos)
    {
        Vector3 dist = pos - transform.position;
        if (dist.magnitude < 0.2f)
        {
            Log("MoveTowardPosition:false");
            return false;
        }
        else
        {
            Log("MoveTowardPosition:true");
            dist.y = 0;
            dist.Normalize();
            dist *= runSpeed;
            transform.position += dist * Time.deltaTime;
            return true;
        }
    }

    public void SetTarget(Transform t)
    {
        target = t.position;
    }
    public void SetTarget(Vector3 pos)
    {
        target = pos;
    }
}
