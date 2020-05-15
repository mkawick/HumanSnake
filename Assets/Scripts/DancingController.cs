using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBodyTest))]
public class DancingController : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeMinInState = 0.25f, timeMaxInState = 0.75f;
    float timeUntilStateChange;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeUntilStateChange < Time.time)
        {
            Array values = Enum.GetValues(typeof(RigidBodyTest.AnimationPlay));
            System.Random random = new System.Random();
            int choice = random.Next(values.Length);
            RigidBodyTest.AnimationPlay randomBar = (RigidBodyTest.AnimationPlay)values.GetValue(choice);

            GetComponent<RigidBodyTest>().PlayAnim(randomBar);

            float range = timeMaxInState - timeMinInState;
            timeUntilStateChange = Time.time;
            timeUntilStateChange += ((float)random.NextDouble()) * range + timeMinInState;
        }
    }
}
