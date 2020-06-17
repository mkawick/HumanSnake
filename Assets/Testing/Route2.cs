using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Route2 : MonoBehaviour
{
    [SerializeField]
    Transform[] controlPoints = null;
    private Vector3 gizmosPosition;

    public enum PathType
    {
        Loop,
        Patrol
    }
    [SerializeField]
    PathType pathType = PathType.Patrol;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if (controlPoints.Length < 2)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.color = new Color(0.9f, 1.0f, 0f);

        for(int i=0; i<controlPoints.Length-1; i++)
            Gizmos.DrawLine(controlPoints[i].position, controlPoints[i+1].position);

        if (pathType == PathType.Loop)
        {
            Gizmos.DrawLine(controlPoints[0].position, controlPoints[controlPoints.Length-1].position);
        }
    }

    public class TrackingValues
    {
        public int lastWaypoint, destWaypoint;
        public float t;
        public int dir;
        public float speed;
        public bool needsInit;

        public TrackingValues()
        {
            t = 0;
            dir = 1;
            speed = 1;
            lastWaypoint = 0;
            destWaypoint = 1;
            needsInit = true;
        }
    }

    public void SetupInitValues(ref TrackingValues tv)
    {
        if (tv.needsInit == true)
        {
            tv.needsInit = false;
            tv.t = 0;
            if (tv.dir == -1 && pathType == PathType.Loop)
            {
                tv.lastWaypoint = 0;
                tv.destWaypoint = controlPoints.Length - 1;
            }
            else 
            {
                tv.lastWaypoint = 0;
                tv.destWaypoint = 1;
            }
        }
    }

    public void UpdateDestination(ref TrackingValues tv)
    {
        int temp = tv.lastWaypoint;
        tv.lastWaypoint = tv.destWaypoint;
        if (pathType == PathType.Loop)
        {
            tv.destWaypoint += tv.dir;
            if (tv.destWaypoint >= controlPoints.Length)
                tv.destWaypoint = 0;
            else if (tv.destWaypoint < 0)
            {
                tv.destWaypoint = controlPoints.Length - 1;
            }
        }
        else if (pathType == PathType.Patrol)
        {
            tv.destWaypoint += tv.dir;
            if (tv.destWaypoint >= controlPoints.Length)
            {
                tv.destWaypoint = temp;
                tv.dir = -tv.dir;
            }
            else if (tv.destWaypoint < 0)
            {
                tv.destWaypoint = 1;
                tv.dir = -tv.dir;
            }
        }

        tv.t = 0;
    }
    public Vector3 GetNext(ref TrackingValues tv, Vector3 currentPos)
    { 
        if (tv.t >= 1.0f || (controlPoints[tv.destWaypoint].position - currentPos).magnitude < 0.1f )
        {
            UpdateDestination(ref tv);
        }

        Vector3 dir = controlPoints[tv.destWaypoint].position - controlPoints[tv.lastWaypoint].position;
        float length = dir.magnitude;
        tv.t += (tv.speed / length) * Time.deltaTime;// simplified projection, calculate a percentage of speed

        dir *= tv.t; // animate along the length
        dir += controlPoints[tv.lastWaypoint].position;

        return dir;
    }
}
