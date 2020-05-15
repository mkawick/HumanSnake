using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBodyTest))]
public class DancingManagerTest : MonoBehaviour
{
    public RigidBodyTest[] dancers;
    void Start()
    {
        foreach (var dancer in dancers)
        {
            dancer.GetComponent<DancingController>().enabled = false;
        }
    }

    void BeginDancing()
    {
        // disable all working components
        /*RunPersonInCircle;
        TrappedPerson2;
        DancingController;*/
    }
    void EndDancing()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            foreach(var dancer in dancers)
            {
                dancer.GetComponent<DancingController>().enabled = true;
            }
        }
    }
}
