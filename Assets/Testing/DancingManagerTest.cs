using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBodyTest))]
public class DancingManagerTest : MonoBehaviour
{
    public RigidBodyTest[] dancers;
    public Transform centerPoint;

    void Start()
    {
        foreach (var dancer in dancers)
        {
            dancer.GetComponent<DancingController>().enabled = false;
        }
    }

    void BeginDancing()
    {
        foreach (var dancer in dancers)
        {
            dancer.GetComponent<RunPersonInCircle>().enabled = false;
            dancer.GetComponent<TrappedPerson2>().enabled = false;
            dancer.GetComponent<DancingController>().enabled = true;

            dancer.GetComponent<DancingController>().StartDancing();

            dancer.transform.forward = centerPoint.forward;
            float range = 15;
            dancer.transform.Rotate(Vector3.up, Random.Range(-range, range));

            Vector3 randomOffset = Random.onUnitSphere; randomOffset.y = 0;

            dancer.transform.position = centerPoint.position + randomOffset*Random.Range(1, 9);
        }
    }
    void EndDancing()
    {
        foreach (var dancer in dancers)
        {
            dancer.GetComponent<RunPersonInCircle>().enabled = false;
            dancer.GetComponent<TrappedPerson2>().enabled = false;
            dancer.GetComponent<DancingController>().enabled = false;
            dancer.GetComponent<DancingController>().StopDancing();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            BeginDancing();
        }
        if (Input.GetMouseButtonDown(1))
        {
            EndDancing();
        }
    }
}
