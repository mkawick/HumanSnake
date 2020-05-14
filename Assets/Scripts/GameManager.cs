using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PeepManager peepManager;
    RunPersonInCircle peepForFailure;
    
    public LevelManager levelManager;
    float isWaitingForSequenceGateTime;

    [Header("End Game FX")]
    public GameObject WellDone;
    public ParticleSystem[] psFailEnding;
    public ParticleSystem successFanfare;
    private Vector3 normalCameraPosition;
    //public Vector3 cameraOffsetToPlayFail = new Vector3(0, 5.5f, 4.9f);
    public Transform runAroundPosition, runAroundStartPosition, cameraOffsetToPlayFail;
    bool isRunningPeepAPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartNewLevel();
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

    public void PlayEnd()
    {
        var main = successFanfare.main;
        main.duration = 1.0f;
        successFanfare.enableEmission = true;
        WellDone.gameObject.SetActive(true);
        //ps.MainModule.Duration = 2.0f;
        successFanfare.Play();
    }

    public void StartNewLevel()
    {
        var main = successFanfare.main;
        successFanfare.enableEmission = false;
        main.playOnAwake = false;
        WellDone.gameObject.SetActive(false);
        successFanfare.Stop();
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
        if (person == null)
        {
            person = peepForFailure.gameObject.AddComponent<RunPersonInCircle>();
        }

        PlayFail(person);
    }

    internal void PlayFail(RunPersonInCircle person)
    {
        peepForFailure = person;
        isRunningPeepAPlayer = peepForFailure.GetComponent<RigidBodyTest>().isPlayer;
        peepForFailure.GetComponent<RigidBodyTest>().isPlayer = false;
        peepForFailure.EnableControllerComponents(false);

        // move peep to location
        peepForFailure.transform.position = runAroundStartPosition.position;
        peepForFailure.transform.rotation = runAroundStartPosition.rotation;

        // add component (remember to remove it)

        person.enabled = true;

        person.pointAround = runAroundPosition;
        person.InitForRunning();

        // AttachParticlEffect
        foreach (var ps in psFailEnding)
        {
            person.AttachParticleEffect(ps);
        }
        // slight delay
        // zoom camera
        normalCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = cameraOffsetToPlayFail.position;
        // play for 8 seconds
        isWaitingForSequenceGateTime = Time.time + 8;
    }

    void EndOfPlayFail()
    {
        peepForFailure.GetComponent<RigidBodyTest>().isPlayer = isRunningPeepAPlayer;
        peepForFailure.EnableControllerComponents(true);

        peepForFailure.RemoveAllParticleEffects();
        peepForFailure.enabled = false;

        levelManager.ResetLevel();
        Camera.main.transform.position = normalCameraPosition;
    }
}
