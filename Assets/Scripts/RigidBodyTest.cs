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
            else
            {
                GoToIdle();
            }
            /*if (Input.GetMouseButtonDown(0) == true)
            {
                rb.velocity += new Vector3(0f, 0, 0.5f);
            }*/
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
    void MoveTowardPosition(Vector3 pos)
    {
        Vector3 dist = pos - transform.position;
        dist.y = 0;
        if (dist.magnitude < 1)
        {
            GoToIdle();
        }
        else
        {
            dist.Normalize();
            dist *= runSpeed;
            transform.position += dist * Time.deltaTime;
        }
    }

    public void SetTarget(Transform t)
    {
        if(t == null)
        {
            GoToIdle();
        }
        else
        {
            MoveTowardPosition(t.position);
            FaceMousePosition(t.position);
        }
    }
    public void SetTarget(Vector3 pos)
    {
        MoveTowardPosition(pos);
        FaceMousePosition(pos);
    }
}
