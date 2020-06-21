using System.Collections;
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
    private DoorScript[] doors;
    public PeepManager peepManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    internal void Init()
    {
        doors = GetComponentsInChildren<DoorScript>();
        SetupTraps();
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
                if (door.blocksLevelEnd == false)
                {
                    door.EnableTrigger();
                }
            }
        }
    }
    public void ResetObjects()
    {
        var objectesToRestore = GetComponentsInChildren<ObjectSavesPosition>();
        if (objectesToRestore != null && objectesToRestore.Length > 0)
        {
            foreach (var objectToRestore in objectesToRestore)
            {
                objectToRestore.Reset();
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
                foreach (var door in doors)// unlock all doors
                {
                    door.EnableTrigger();
                }
            }
        }
    }

    public void SetupPlayerStart(JoelAnimator go)
    {
        go.SetPosition(playerStartPosition.position);
        go.SetTarget(playerStartPosition.position);
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
        var peeps = GetTrappedPeople();
        foreach(var peep in peeps)
        {
            peep.peepManager = mgr;
        }
    }

    void SetupTraps()
    {
        var trapsNode = Utils.GetChildWithName(this.gameObject, "Traps");
        if (trapsNode == null)
            return;

        var listOfTraps = trapsNode.GetComponentsInChildren<DeadlyTrap>();
        foreach(var trap in listOfTraps)
        {
            trap.SetLevel(this);
        }
    }

    internal void PersonDied()
    {
        peepManager.PersonDied();
    }
}
