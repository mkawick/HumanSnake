﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using System;

public class Level : MonoBehaviour
{
    public Transform playerStartPosition;
    public Transform exitLocation;
    public DoorScript[] doors;
    public PeepManager peepManager;
    // Start is called before the first frame update
    void Start()
    {
        doors = GetComponentsInChildren<DoorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        EnableExit();
    }

    public void ResetDoors()
    {
        var listOfPeeps = GetComponentsInChildren<TrappedPerson2>();
        if (doors != null && doors.Length > 0)
        {
            foreach(var door in doors)
            {
                door.Reset();
                door.SetDoorNumber(listOfPeeps.Length);
            }
        }
    }

    public void EnableExit()
    {
        if (peepManager == null)
            return;
        var listOfPeeps = GetComponentsInChildren<TrappedPerson2>();
        if (peepManager.GetNumInSnake() == listOfPeeps.Length)
        {
            if (doors != null && doors.Length > 0)
            {
                foreach (var door in doors)
                {
                    door.EnableTrigger();
                }
            }
        }
    }

    public void SetupPlayerStart(RigidBodyTest go)
    {
        Vector3 pos =( playerStartPosition.position);
        RigidBodyTest nma = go.GetComponent<RigidBodyTest>();
        nma.transform.position = pos;
    }

    public List<TrappedPerson2> GetTrappedPeople()
    {
        var listOfPeeps = GetComponentsInChildren<TrappedPerson2>();

        return listOfPeeps.OfType<TrappedPerson2>().ToList();
    }

    public bool IsLevelComplete()
    {
        var listOfPeeps = GetTrappedPeople();
        foreach (var peep in listOfPeeps)
        {
            if (peep.currentState != TrappedPerson2.State.EndOfLevel)
            {
                return false;
            }
        }
        return true;
    }

    internal void SetPeepManager(PeepManager mgr)
    {
        peepManager = mgr;
    }
}
