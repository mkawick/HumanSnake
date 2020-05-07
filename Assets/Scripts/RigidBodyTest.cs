using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyTest : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    bool running = false;
    public float runSpeed = 1;
    public bool isPlayer = false;
    Vector3 target;

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
            //if(Input.GetMouseButtonDown(1) == true) 
            if (Input.GetMouseButton(0) == true)
            {
                //rb.velocity += new Vector3(0f, 0, 0.5f);

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    FaceMousePosition(hit.point);
                    MoveTowardPosition(hit.point);
                }

                StartRunning();
            }
        }
        else
        {
            if (target == null || target == Vector3.zero)
            {
                GoToIdle();
            }
            else
            {
                if (MoveTowardPosition(target))
                    StartRunning();
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
        if (running == false)
        {
            animator.SetTrigger("Run");
            running = true;
        }
    }
    void GoToIdle()
    {
        if (running == true)
        {
            animator.SetTrigger("Idle");
            running = false;
        }
    }
    bool MoveTowardPosition(Vector3 pos)
    {
        Vector3 dist = pos - transform.position;
        if (dist.magnitude < 0.05f)
        {
            GoToIdle();
            return false;
        }
        else
        {
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
