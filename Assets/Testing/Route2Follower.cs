using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route2Follower : MonoBehaviour
{
    Route2.TrackingValues tracking = new Route2.TrackingValues();
    [SerializeField]
    Route2 routeToFollow;
    public float speed = 1;

    [Tooltip("direction: either 1 or -1"), Range(-1,1)]
    public int dir = 1;
    void Start()
    {
        if (dir != -1 && dir != 1)
            dir = 1;

        tracking.speed = speed;
        tracking.dir = dir;

        if(routeToFollow != null)
            routeToFollow.SetupInitValues(ref tracking);
    }

    void Update()
    {
        if(routeToFollow != null)
        {
            tracking.speed = speed;// allows for dynamic changing in editor at runtime
            Vector3 newPos = routeToFollow.GetNext(ref tracking, this.transform.position);
            this.transform.position = newPos;
        }
    }
}
