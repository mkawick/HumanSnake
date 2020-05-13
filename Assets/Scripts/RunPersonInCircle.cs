using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPersonInCircle : MonoBehaviour
{
    [SerializeField]
    public Transform pointAround;

    int  shouldSetTarget = 1000;
    //ParticleEffect;
    float dist;

    bool showDebugLines = false;
    float timeToStart = 0;

    GameObject root;
    GameObject bum;
    void Start()
    {
        dist = (pointAround.position - transform.position).magnitude;
        InitForRunning();
    }

    public void InitForRunning()
    {
        timeToStart = Time.time + 0.2f;
        SetupBumPosition();
    }
    void SetupBumPosition()
    {
        if(bum != null)
        {
            return;
        }
        if(GetChildWithName(transform.gameObject, "Bum") != null)
        {
            return;
        }
        root = GetChildWithName(transform.gameObject, "Root");
        Debug.Assert(root != null);
        bum = new GameObject();
        bum.transform.parent = root.transform;
        bum.name = "Bum";
        bum.transform.position = new Vector3(0, 0.45f, -0.3f);
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

    internal void AttachParticlEffect(ParticleSystem ps)
    {
        ParticleSystem newPs = Instantiate(ps, new Vector3(0, 0, 0), Quaternion.identity);
        newPs.gameObject.transform.parent = bum.transform;
        newPs.Play();
    }

    internal void RemoveAllParticleEffects()
    {
        var o = bum.gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach(var ps in o)
        {
            Destroy(ps);
        }
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
        centerPoint.y = transform.position.y;

        Vector3 vectToCenter = (centerPoint - transform.position);
        Vector3 perpVector = new Vector3(-vectToCenter.z, 0, vectToCenter.x);// swap x/z
        perpVector.Normalize();

        if (Vector3.Dot(perpVector, transform.forward) < 0)// we would run in the opposite direction from facing
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

        //float len = rangedVector.magnitude;
        //float len2 = actualRangedPosition.magnitude;
        //sif (shouldSetTarget)
        {
            var rbt = GetComponent<RigidBodyTest>();
            rbt.SetTarget(actualRangedPosition);
        }

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
