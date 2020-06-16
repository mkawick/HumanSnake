using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicPeepAnimController))]
public class RunPersonInCircle : MonoBehaviour
{
    [SerializeField]
    public Transform pointAround;
    //private Quaternion originalRotation;
    //private Vector3 originalForward;
    private Vector3 curl;

    int  shouldSetTarget = 1000;
    //ParticleEffect;
    float dist;

    bool showDebugLines = false;
    float timeToStart = 0;

    GameObject root;
    GameObject bum;
    List<ParticleSystem> particleList;
    
    bool isRunningPeepAPlayer;
    void Start()
    {
        dist = (pointAround.position - transform.position).magnitude;
        //SetupBumPosition();InitForRunning();
        particleList = new List<ParticleSystem>();

    }

    public void InitForRunning(Transform pivot, Transform initialDirection, List<ParticleSystem>  psFailEnding)
    {
        timeToStart = Time.time + 0.2f;

        this.transform.position = initialDirection.position;
        this.transform.rotation = initialDirection.rotation;
        //originalRotation = initialDirection.rotation;
        //originalForward = initialDirection.forward;
        curl = Vector3.Cross(this.transform.position-pivot.position, initialDirection.forward);

        this.enabled = true;

        this.pointAround = pivot;
        isRunningPeepAPlayer = this.GetComponent<BasicPeepAnimController>().isPlayer;
        this.GetComponent<BasicPeepAnimController>().isPlayer = false;

        SetupBumPosition();
        foreach (var ps in psFailEnding)
        {
            this.AttachParticleEffect(ps);
        }
    }

    public void RestoreAfterRunning()
    {
        this.GetComponent<BasicPeepAnimController>().isPlayer = isRunningPeepAPlayer;
        this.GetComponent<BasicPeepAnimController>().EnableControllerComponents(true);

        this.RemoveAllParticleEffects();
        this.enabled = false;
    }

   /* internal void EnableControllerComponents(bool enable)
    {
        var tp = gameObject.GetComponent<TrappedPerson2>();
        if (tp != null)
            tp.enabled = enable;
        var pcwm = gameObject.GetComponent<PlayerControllerWobbleMan>();
        if (pcwm != null)
        {
            if(pcwm.enabled == true)
                wasWobblemanControllerEnabled = true;

            if (enable == true)
                pcwm.enabled = wasWobblemanControllerEnabled;
            else
                pcwm.enabled = enable;
        }
        var pmhl = gameObject.GetComponent<PlayerMouseHoldLocomotion>();
        if (pmhl != null)
        {
            if (pmhl.enabled == true)
                wasPlayerMouseControllerEnabled = true;

            if (enable == true)
                pmhl.enabled = wasPlayerMouseControllerEnabled;
            else
                pmhl.enabled = enable;
        }
        var dancer = gameObject.GetComponent<DancingController>();
        if (dancer != null)
        {
            if (dancer.enabled == true)
                wasDancerEnabled = true;

            if (enable == true)
                dancer.enabled = wasDancerEnabled;
            else
                dancer.enabled = enable;
        }
    }*/
    void SetupBumPosition()
    {
        if(bum != null)
        {
            return;
        }
        bum = Utils.GetChildWithName(transform.gameObject, "Bum");
        if (bum != null)
        {
            return;
        }
        root = Utils.GetChildWithName(transform.gameObject, "Root");
        bum = Utils.GetChildWithName(root, "Bum");
        //bum = root;
        if (bum != null)
        {
            return;
        }
        Debug.Assert(root != null);
        bum = new GameObject();
        bum.transform.parent = root.transform;
        bum.name = "Bum";
        bum.transform.position = new Vector3(0, 0.45f, -0.3f);
    }

    internal void AttachParticleEffect(ParticleSystem ps)
    {
        ParticleSystem newPs = Instantiate(ps, bum.transform.position, bum.transform.rotation);
        newPs.gameObject.transform.parent = bum.transform;
        newPs.Play();
        if (particleList == null)
            particleList = new List<ParticleSystem>();
                
        particleList.Add(newPs);
    }

    internal void RemoveAllParticleEffects()
    {
        var o = bum.gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach(var ps in o)
        {
            Destroy(ps.gameObject);
        }
        foreach(var p in particleList)
        {
            GameObject.Destroy(p.gameObject);
        }
        particleList.Clear();
       /* for (int i= num - 1; i>-1; i--)
        {
            Destroy(bum.gameObject.GetChild(i));
        }*/
    }


    // Update is called once per frame
    void Update()
    {
        SetupBumPosition();
        if (Time.time > timeToStart)
            SetupNextPosition();
        //SetupFX()
    }

    private void SetupNextPosition()
    {
        if (shouldSetTarget == 0)
            return;

        Vector3 centerPoint = pointAround.position;
        centerPoint.y = transform.position.y;// ignoring up/down

        Vector3 vectToCenter = (transform.position - centerPoint);
        vectToCenter.Normalize();
        Vector3 perpVector = new Vector3(-vectToCenter.z, 0, vectToCenter.x);// swap x/z
        //perpVector.Normalize();

        Vector3 tempCurl = Vector3.Cross(vectToCenter, perpVector);
        // we would run in the opposite direction from facing
        if (Vector3.Dot(curl, tempCurl) <= 0)
        {
            perpVector = -perpVector;
        }
        // perpVector *= 3; // just sticking out a little
        perpVector.y = transform.position.y;
        Vector3 newPosition = transform.position + perpVector;
        newPosition.y = transform.position.y;

        // we need to stay in the same range and the new location is too far away.
        Vector3 rangedVector = newPosition - centerPoint; 
        rangedVector.y = transform.position.y;
        Vector3 actualRangedPosition = (rangedVector).normalized * dist + centerPoint;


        var rbt = GetComponent<BasicPeepAnimController>();
        rbt.SetTarget(actualRangedPosition);


        if (showDebugLines)
        {
            float duration = 3;
            Vector3 pos = transform.position;
            Vector3 Offset1 = new Vector3(1, 0, -1);
            Vector3 Offset2 = new Vector3(-1, 0, 1);
            Debug.DrawLine(Vector3.zero, transform.forward, Color.magenta);
            Debug.DrawLine(pos - Offset1, pos + Offset1, Color.magenta);
            Debug.DrawLine(pos - Offset2, pos + Offset2, Color.magenta);
            //(newPosition, transform.forward * 3 + newPosition, Color.cyan, duration); // newPosition
            Debug.DrawLine(newPosition, transform.forward * 3 + newPosition, Color.cyan, duration); // newPosition
            Debug.DrawLine(transform.forward + transform.position, transform.forward * 3 + transform.position, Color.red, duration);// forward
            Debug.DrawLine(centerPoint, pos, Color.grey, duration);
            Debug.DrawLine(actualRangedPosition, actualRangedPosition + Vector3.up * 3, Color.white, duration);// final spot
            Debug.DrawLine(actualRangedPosition, centerPoint, Color.yellow, duration);
        }

        Debug.DrawLine(centerPoint, centerPoint+Vector3.up, Color.grey, 1);

        //shouldSetTarget --;
    }
}
