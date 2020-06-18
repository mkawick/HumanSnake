using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicPeepAnimController))]
public class DancingController : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeMinInState = 0.25f, timeMaxInState = 0.75f;
    float timeUntilStateChange;
    void Start()
    {
        
    }

    public void StartDancing()
    {
        float range = timeMaxInState - timeMinInState;
        timeUntilStateChange = Time.time + UnityEngine.Random.value * range + timeMinInState;
    }
    public void StopDancing()
    {
        GetComponent<BasicPeepAnimController>().PlayAnim(BasicPeepAnimController.AnimationPlay.Idle);
    }
    // Update is called once per frame
    void Update()
    {
        if(timeUntilStateChange < Time.time)
        {
            Array values = Enum.GetValues(typeof(BasicPeepAnimController.AnimationPlay));
            int choice = (int)(UnityEngine.Random.value * (float)values.Length);
            BasicPeepAnimController.AnimationPlay randomBar = (BasicPeepAnimController.AnimationPlay)values.GetValue(choice);

            GetComponent<BasicPeepAnimController>().PlayAnim(randomBar);

            float range = timeMaxInState - timeMinInState;
            timeUntilStateChange = Time.time + UnityEngine.Random.value * range + timeMinInState;
        }
    }

    internal void PrepToDance(Vector3 position, Vector3 facingLocation, float timeForDance)
    {
        this.transform.position = position;
        this.transform.LookAt(facingLocation);
        this.StartDancing();
    }
}
