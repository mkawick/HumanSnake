﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PeepManager peepManager;
    List<RunPersonInCircle> peepsForFailure;
    
    public LevelManager levelManager;
    float isWaitingForSequenceGateTime;

    [Header("End Game FX")]
    public float playFailTime = 3.0f;
    public GameObject WellDone;
    public ParticleSystem[] psFailEnding;
    public ParticleSystem successFanfare;
    private Vector3 normalCameraPosition;
    private Quaternion normalCameraRotation;

    public Transform runAroundPosition, runAroundStartPosition, cameraOffsetToPlayFail;

    public GameObject failureText, successText;

    // Start is called before the first frame update
    void Start()
    {
        normalCameraPosition = Camera.main.transform.position;
        normalCameraRotation = Camera.main.transform.rotation;

        StartNewLevel();
        
        peepsForFailure = new List<RunPersonInCircle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaitingForSequenceGateTime != 0)
        {
            if (isWaitingForSequenceGateTime < Time.time)
            {
                isWaitingForSequenceGateTime = 0;
                EndOfPlayFail();
                return;
            }
        }
    }

    public void PlayEnd(GameObject tempCameraSpot)
    {
        if(tempCameraSpot != null)
        {
            Camera.main.transform.position = tempCameraSpot.transform.position;
            Camera.main.transform.rotation = tempCameraSpot.transform.rotation;
        }
        /*
        var main = successFanfare.main;
        main.duration = 1.0f;
        successFanfare.enableEmission = true;
        //WellDone.gameObject.SetActive(true);
        //ps.MainModule.Duration = 2.0f;
        //successFanfare.Play();
        successText.gameObject.SetActive(true);
        */
    }

    public void StartNewLevel()
    {
        var main = successFanfare.main;
        successFanfare.enableEmission = false;
        main.playOnAwake = false;
        WellDone.gameObject.SetActive(false);
        successFanfare.Stop();

        failureText.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
        Camera.main.transform.position = normalCameraPosition;
        Camera.main.transform.rotation = normalCameraRotation;
    }


    internal void PlayFail(TrappedPerson2 peep)
    {
        RunPersonInCircle person;
        TrappedPerson2 temp = peep;//.GetComponent<TrappedPerson2>();
        if (temp == null)
        {
            temp = peepManager.GetRandomPeep();
        }
        person = temp.GetComponent<RunPersonInCircle>();

        PlayFail(person);
    }

    internal void PlayFail(RunPersonInCircle person)
    {
        failureText.gameObject.SetActive(true);
        if (person)
        {
            if (peepsForFailure.Find(item => item==person ) == null)
            {
                peepsForFailure.Add(person);
                person.InitForRunning(runAroundPosition, runAroundStartPosition, psFailEnding.ToList());
                person.GetComponent<BasicPeepAnimController>().EnableControllerComponents(false);
            }
        }

        // slight delay
        // zoom camera

        Camera.main.transform.position = cameraOffsetToPlayFail.position;
        // play for 8 seconds
        isWaitingForSequenceGateTime = Time.time + playFailTime;
    }

    void EndOfPlayFail()
    {
        foreach (var peep in peepsForFailure)
        {
            peep.RestoreAfterRunning();
        }
        peepsForFailure.Clear();

        levelManager.ResetLevel();
        Camera.main.transform.position = normalCameraPosition;
        Camera.main.transform.rotation = normalCameraRotation;
    }
}
