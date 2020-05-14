using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PeepManager peepManager;
    TrappedPerson2 peepForFailure;
    private Vector3 normalCameraPosition;
    public LevelManager levelManager;
    float isWaitingForSequenceGateTime;

    [Header("End Game FX")]
    public GameObject WellDone;
    public ParticleSystem[] psFailEnding;
    public ParticleSystem successFanfare;
    public Vector3 cameraOffsetToPlayFail = new Vector3(0, 5.5f, 4.9f);
    public Transform runAroundPosition, runAroundStartPosition;

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
            //if
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


    public void PlayFail(TrappedPerson2 peep)
    {
        if (peep != null)
        {
            peepForFailure = peep;
        }
        else
        {
            peepForFailure = peepManager.GetRandomPeep();
        }

        // move peep to location
        peepForFailure.transform.position = runAroundStartPosition.position;

        // add component (remember to remove it)
        var raic = peepForFailure.GetComponent<RunPersonInCircle>();
        if (raic == null)
        {
            raic = peepForFailure.gameObject.AddComponent<RunPersonInCircle>();
        }
        raic.enabled = true;
        peepForFailure.gameObject.GetComponent<TrappedPerson2>().enabled = false;
       /* var trapped = peepForFailure.GetComponent<TrappedPerson2>();
        if (raic == null)
        {

        }*/

            raic.pointAround = runAroundPosition;
        raic.InitForRunning();

        // AttachParticlEffect
        foreach (var ps in psFailEnding)
        {
            raic.AttachParticlEffect(ps);
        } 
        // slight delay
        // zoom camera
        normalCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = raic.pointAround.position - cameraOffsetToPlayFail;
        // play for 8 seconds
        isWaitingForSequenceGateTime = Time.time + 8;

        // reset level
    }

    void EndOfPlayFail()
    {
        /*foreach(var ps in psFailEnding)
        {
            ps.gameObject.transform.parent = null;
            ps.Stop();
        }*/
        peepForFailure.gameObject.GetComponent<TrappedPerson2>().enabled = true;
        var raic = peepForFailure.GetComponent<RunPersonInCircle>();
        //Destroy(raic);
        raic.RemoveAllParticleEffects();
        raic.enabled = false;

        levelManager.ResetLevel();
        Camera.main.transform.position = normalCameraPosition;
    }
}
