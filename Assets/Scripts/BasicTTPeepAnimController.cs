using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BasicTTPeepAnimController : BasicPeepAnimController
{
    internal override void PlayAnim(AnimationPlay clip)
    {
        switch (clip)
        {
            case AnimationPlay.Walk:
                animator.SetTrigger("Walk");
                break;
            case AnimationPlay.Hit:
                animator.SetTrigger("Hit");
                break;
            case AnimationPlay.Attack:
                animator.SetTrigger("Attack");
                break;
            case AnimationPlay.Jump:
                animator.SetTrigger("Jump");
                break;
            // missing a few
            case AnimationPlay.Wave:
                animator.SetTrigger("Wave");
                break;
            case AnimationPlay.Run:
                animator.SetTrigger("Run");
                break;
            case AnimationPlay.Idle:
                animator.SetTrigger("Idle");
                break;
            case AnimationPlay.Throw:
                animator.SetTrigger("Throw");
                break;
            case AnimationPlay.PanicIdle:
                animator.SetTrigger("PanicIdle");
                break;
            case AnimationPlay.PanicRun:
                animator.SetTrigger("PanicRun");
                break;
        }
    }
    void RunTests()
    {
        if (Input.GetKey(KeyCode.R))
        {
            animator.SetTrigger("Run");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetTrigger("Walk");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("Idle");
        }
        else if (Input.GetKey(KeyCode.V))
        {
            animator.SetTrigger("Wave");
        }
    }
}
/*[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class BasicTTPeepAnimController : MonoBehaviour
{
    Animator animator;
    Vector3 target;
    [SerializeField]
    bool slowRotation = false;
    [SerializeField]
    float runSpeed = 6;

    float maximumRotationPerFrame = 0.01f;
    float animationTimeGate = 0;
    float animChangeLagTime = 0.25f;

    bool wasWobblemanControllerEnabled = false;
    bool wasDancerEnabled = false;
    bool wasTrappedPersonEnabled = false;
    bool hadSlowedRotation = false;

    enum AnimStateChange
    {
        Run,
        Idle,
        None
    }
    public enum AnimationPlay
    {
        Walk,
        Hit,
        Attack,
        Jump,
        Falling,
        RunJumpL,
        RunJumpR,
        Wave,
        Run,
        Idle
    }

    AnimStateChange currentState = AnimStateChange.Idle, pendingStateChange = AnimStateChange.None;

    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");
        SaveInitialState();
    }
    void SaveInitialState()
    {
        var tp = gameObject.GetComponent<TrappedPerson2>();
        if (tp != null)
        {
            wasTrappedPersonEnabled = tp.enabled;
        }
        var bpac = gameObject.GetComponent<BasicTTPeepAnimController>();
        if (bpac != null)
        {
            hadSlowedRotation = bpac.slowRotation;
        }
        var pcwm = gameObject.GetComponent<PlayerControllerWobbleMan>();
        if (pcwm != null)
        {
            wasWobblemanControllerEnabled = pcwm.enabled;
        }
        var dancer = gameObject.GetComponent<DancingController>();
        if (dancer != null)
        {
            wasDancerEnabled = dancer.enabled;
        }
    }
    internal void EnableControllerComponents(bool enable)
    {
        var tp = gameObject.GetComponent<TrappedPerson2>();
        if (tp != null)
        {
            if (enable == true)
                tp.enabled = wasTrappedPersonEnabled;
            else
                tp.enabled = enable;
        }
        var bpac = gameObject.GetComponent<BasicTTPeepAnimController>();
        if (bpac != null)
        {
            if (enable == true)
                bpac.slowRotation = hadSlowedRotation;
            else
                bpac.slowRotation = enable;
        }
        var pcwm = gameObject.GetComponent<PlayerControllerWobbleMan>();
        if (pcwm != null)
        {
            if (enable == true)
                pcwm.enabled = wasWobblemanControllerEnabled;
            else
                pcwm.enabled = enable;
        }
        var dancer = gameObject.GetComponent<DancingController>();
        if (dancer != null)
        {
            if (enable == true)
                dancer.enabled = wasDancerEnabled;
            else
                dancer.enabled = enable;
        }
    }

    private void Update()
    {
        //RunTests();
        if (target == null || target == Vector3.zero)
        {
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
    void StartRunning()
    {
        if (animationTimeGate < Time.time)
        {
            if (currentState == AnimStateChange.Idle)
            {
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
    internal void MovePlayer(Vector3 position)
    {
        if ((position - transform.position).magnitude > 0.1f)
        {
            FacePosition(position);
            MoveTowardPosition(position);
            StartRunning();
        }
    }
    bool MoveTowardPosition(Vector3 pos)
    {
        Vector3 dist = pos - transform.position;
        dist.y = 0;// a lot of times, the pos is random and the y is not in the plane
        if (dist.magnitude < 0.4f)
        {
            return false;
        }
        else
        {
            dist.Normalize();
            dist *= runSpeed;
            transform.position += dist * Time.deltaTime;
            return true;
        }
    }
    void FacePosition(Vector3 placeToLook)
    {
        if (slowRotation)
        {
            Quaternion lookOnLook =
                    Quaternion.LookRotation(placeToLook - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * maximumRotationPerFrame);
        }
        else
        {
            Vector3 dist = placeToLook - transform.position;// do not turn for too close
            dist.y = 0;
            if (dist.magnitude > 0.8)
            {
                placeToLook.y = transform.position.y;
                transform.LookAt(placeToLook);
            }
        }
    }
    void GoToIdle()
    {
        if (animationTimeGate < Time.time)
        {
            if (currentState == AnimStateChange.Run)
            {
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

    void RunTests()
    {
        if (Input.GetKey(KeyCode.R))
        {
            animator.SetTrigger("Run");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetTrigger("Walk");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("Idle");
        }
        else if (Input.GetKey(KeyCode.V))
        {
            animator.SetTrigger("Wave");
        }
    }
}
*/
