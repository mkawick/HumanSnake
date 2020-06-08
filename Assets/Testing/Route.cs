using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField]
    Transform[] controlPoints;
    private Vector3 gizmosPosition;

    [SerializeField]
    [Range(0.03f, 0.12f)]
    float ballGranularity = 0.05f;
    [SerializeField]
    [Range(1.5f, 6f)]
    float lineFineness = 1.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if (controlPoints.Length < 4)
            return;

        Gizmos.color = Color.yellow;
        float dim = ballGranularity;

        Gizmos.DrawSphere(this.transform.position, dim);

        float addPerIter = lineFineness / 100;
        for (float t=0; t<=1; t+= addPerIter)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                            3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                            3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                            Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, dim);
        }

        Gizmos.color = new Color(0.9f, 0, 0.9f);

        Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);
        Gizmos.DrawLine(controlPoints[2].position, controlPoints[3].position);
        /*  Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
          new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));

          Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
             new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));*/
    }
}
