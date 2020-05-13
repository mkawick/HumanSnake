using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject WellDone;
    public ParticleSystem[] psBadEnding;
    public PeepManager peepManager;
    public Transform runAroundPosition, runAroundStartPosition;

    TrappedPerson2 peepForFailure;

    // Start is called before the first frame update
    void Start()
    {
        StartNewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnd()
    {
        var main = ps.main;
        main.duration = 1.0f;
        ps.enableEmission = true;
        WellDone.gameObject.SetActive(true);
        //ps.MainModule.Duration = 2.0f;
        ps.Play();
    }

    public void StartNewLevel()
    {
        var main = ps.main;
        ps.enableEmission = false;
        main.playOnAwake = false;
        WellDone.gameObject.SetActive(false);
        ps.Stop();
    }


    public void PlayFail()
    {

        // select random peep
        peepForFailure = peepManager.GetRandomPeep();

        // move peep to location
        peepForFailure.transform.position = runAroundStartPosition.position;

        // add component (remember to remove it)
        var raic = peepForFailure.GetComponent<RunPersonInCircle>();
        if (raic == null)
        {
            raic = peepForFailure.gameObject.AddComponent<RunPersonInCircle>();
        }

        // set peep run around position
        raic.pointAround = runAroundPosition;

        // AttachParticlEffect
        foreach (var ps in psBadEnding)
        {
            raic.AttachParticlEffect(ps);
        }
        // slight delay
        // zoom camera
        // play for 8 seconds
        // reset level
    }

    void EndPlayFail()
    {
        foreach(var ps in psBadEnding)
        {
            ps.gameObject.transform.parent = null;
            ps.Stop();
        }
        var raic = peepForFailure.GetComponent<RunPersonInCircle>();
        Destroy(raic);
    }
}
