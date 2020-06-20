using System.Collections;
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
    public float playSuccessTime = 5.0f;
    public GameObject WellDone;
    public ParticleSystem[] psFailEnding;
    public ParticleSystem successFanfare;
    private Vector3 normalCameraPosition;
    private Quaternion normalCameraRotation;

    public Transform runAroundPosition, runAroundStartPosition, cameraOffsetToPlayFail;

    public GameObject failureText, successText;
    public Camera successCamera, failureCamera;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        normalCameraPosition = mainCamera.transform.position;
        normalCameraRotation = mainCamera.transform.rotation;

        StartNewLevel();
        
        peepsForFailure = new List<RunPersonInCircle>();

        
        Debug.Assert(successCamera != null, "successCamera not set");
        Debug.Assert(failureCamera != null, "failureCamera not set");
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
        Utils.DisableAllCameras();
        successCamera.gameObject.SetActive(true);
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
        Utils.DisableAllCameras();
        mainCamera.gameObject.SetActive(true);
    }


   /* internal void PlayFail(TrappedPerson2 peep)
    {
        RunPersonInCircle person;
       // TrappedPerson2 temp = peep;//.GetComponent<TrappedPerson2>();
        if (temp == null)
        {
            temp = peepManager.GetRandomPeep();
        }
        person = temp.GetComponent<RunPersonInCircle>();

        PlayFail(person);
    }*/

    internal void PlayFail()
    {
        Utils.DisableAllCameras();
        failureCamera.gameObject.SetActive(true);

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
        /* Camera.main.transform.position = normalCameraPosition;
         Camera.main.transform.rotation = normalCameraRotation;*/
    }
}
