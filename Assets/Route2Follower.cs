using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route2Follower : MonoBehaviour
{
    Route2.TrackingValues tracking = new Route2.TrackingValues();
    public Route2 routeToFollow;
    void Start()
    {
        //tracking.needsInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(routeToFollow != null)
        {
            Vector3 newPos = routeToFollow.GetNext(ref tracking, this.transform.position);
            this.transform.position = newPos;
        }
    }
}
