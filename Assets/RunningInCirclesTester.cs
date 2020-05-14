using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningInCirclesTester : MonoBehaviour
{
    public TrappedPerson2 peepForFailure;

    public ParticleSystem[] effectsToAttach;
    private Vector3 normalCameraPosition;
    public Vector3 cameraOffsetToPlayFail = new Vector3(0, 10f, -8.5f);
    public Transform runAroundPosition, runAroundStartPosition;
    float isWaitingForSequenceGateTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1) == true)
        {
            PlayFail();
        }
    }

    public void PlayFail()
    {

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

        raic.pointAround = runAroundPosition;
        raic.InitForRunning();

        // AttachParticlEffect
        foreach (var ps in effectsToAttach)
        {
            raic.AttachParticleEffect(ps);
        }
        // slight delay
        // zoom camera
        normalCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = runAroundPosition.position + cameraOffsetToPlayFail;
        // play for 8 seconds
        isWaitingForSequenceGateTime = Time.time + 8;

        // reset level
    }
}
