using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class BasicPeepAnimController : MonoBehaviour
{
    protected Animator animator;
    //bool running = false;
    public float runSpeed = 1;
    public float minimumMoveRange = 0.1f;
    public float maximumRotationPerFrame = 0.01f;
    public bool isPlayer = false;
    public bool isLoggingEnabled = false;
    public bool slowRotation = true;
    bool wasWobblemanControllerEnabled = false;
    bool wasPlayerMouseControllerEnabled = false;
    bool wasRootPerFrameEnabled = false;
    bool wasDancerEnabled = false;
    bool wasTrappedPersonEnabled = false;
    bool hadSlowedRotation = false;

    //bool pendingAnimationStateChange = false;
    Vector3 target;

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
        Idle,
        PanicIdle,
        PanicRun,
        Throw
    }

    AnimStateChange currentState = AnimStateChange.Idle, pendingStateChange = AnimStateChange.None;
    float animationTimeGate;
    public float animChangeLagTime = 0.25f;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");
        SaveInitialState();
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

    void SaveInitialState()
    {
        var tp = gameObject.GetComponent<TrappedPerson2>();
        if (tp != null)
        {
            wasTrappedPersonEnabled = tp.enabled;
        }
        var bpac = gameObject.GetComponent<BasicPeepAnimController>();
        if (bpac != null)
        {
            hadSlowedRotation = bpac.slowRotation;
        }
        var pcwm = gameObject.GetComponent<PlayerControllerWobbleMan>();
        if (pcwm != null)
        {
            wasWobblemanControllerEnabled = pcwm.enabled;
        }
        var pmhl = gameObject.GetComponent<PlayerMouseHoldLocomotion>();
        if (pmhl != null)
        {
            wasPlayerMouseControllerEnabled = pmhl.enabled;
        }
        var pccr = gameObject.GetComponent<PlayerControllerChangeRootPerFrame>();
        if (pccr != null)
        {
            wasRootPerFrameEnabled = pccr.enabled;
        }

        var dancer = gameObject.GetComponent<DancingController>();
        if (dancer != null)
        {
            wasDancerEnabled = dancer.enabled;
        }
    }
    internal void StopPlayer()
    {
        GoToIdle();
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
        var bpac = gameObject.GetComponent<BasicPeepAnimController>();
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
        var pmhl = gameObject.GetComponent<PlayerMouseHoldLocomotion>();
        if (pmhl != null)
        {
            if (enable == true)
                pmhl.enabled = wasPlayerMouseControllerEnabled;
            else
                pmhl.enabled = enable;
        }
        var pccr = gameObject.GetComponent<PlayerControllerChangeRootPerFrame>();
        if (pccr != null)
        {
            if (enable == true)
                pccr.enabled = wasRootPerFrameEnabled;
            else
                pccr.enabled = enable;
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

    internal virtual void PlayAnim(AnimationPlay clip)
    {
        switch(clip)
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
            case AnimationPlay.Falling:
                animator.SetTrigger("Falling");
                break;
            case AnimationPlay.RunJumpL:
                animator.SetTrigger("RunJumpL");
                break;
            case AnimationPlay.RunJumpR:
                animator.SetTrigger("RunJumpR");
                break;
            case AnimationPlay.Wave:
                animator.SetTrigger("Wave");
                break;
            case AnimationPlay.Run:
                animator.SetTrigger("Run");
                break;
            case AnimationPlay.Idle:
                animator.SetTrigger("Idle");
                break;
        }
    }
    void FacePosition(Vector3 placeToLook)
    {
        if(slowRotation)
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
