using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Route2 : MonoBehaviour
{
    [SerializeField]
    Transform[] controlPoints;
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
        //public Vector3 lastPosition;
    }
    public Vector3 GetNext(ref TrackingValues tv, Vector3 currentPos)
    {
        /* if(tv.lastPosition == null)
         {
             tv.t = 0;
             tv.dir = 1;
             tv.speed = 1;
             //tv.lastPosition = controlPoints[0].position;
             tv.lastWaypoint = 0;
             tv.destWaypoint = 1;
         }*/

        if ((controlPoints[tv.destWaypoint].position - currentPos).magnitude < 0.1f ||
            tv.t >= 1.0f)
        {
            // have we reached the end of the loop?
            /* int temp = tv.destWaypoint;
             tv.destWaypoint = tv.lastWaypoint;
             tv.lastWaypoint = temp;*/
            int temp = tv.lastWaypoint;
            tv.lastWaypoint = tv.destWaypoint;
            if(pathType == PathType.Loop)
            {
                tv.destWaypoint += tv.dir;
                if (tv.destWaypoint >= controlPoints.Length)
                    tv.destWaypoint = 0;
            }
            else if(pathType == PathType.Patrol)
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
                //else
                    //tv.destWaypoint = temp;
                    
            }
            
            tv.t = 0;
        }

        Vector3 dir = (controlPoints[tv.destWaypoint].position - controlPoints[tv.lastWaypoint].position);
        float length = dir.magnitude;
        tv.t += (tv.speed / length) * Time.deltaTime;
        //tv.t += 0.02f;
        dir *= tv.t;
        //dir *= tv.speed * Time.deltaTime;
        dir += controlPoints[tv.lastWaypoint].position;

        return dir;
        //Vector3 lastWp = controlPoints[tv.lastWaypoint];


    }
}
