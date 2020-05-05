using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepManager : MonoBehaviour
{
    List<TrappedPerson> peeps;
    List<Transform> snakeList;
    public Transform player;
    public float defaultDistanceToFollowPlayer = 2.2f;
    public float increasedRangeWithEachFollower = 0.9f;
    
    public float followingPlayerSpeedMultipler = 1.1f;

    internal Transform exitLocation;
    public float distanceToExit = 1.2f;

    public Material[] materialsForPeeps;
    enum PeepStateColors
    {
        WanderingScared,
        FollowingPlayerSafe,
        ExitedBuilding
    }
    void Start()
    {
        peeps = new List<TrappedPerson>();
    }

    public void NewLevel(List<TrappedPerson> peepList)
    {
        peeps = peepList;
        foreach(var p in peeps)
        {
            p.SetupInitialState(player);
        }
        snakeList = new List<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(TrappedPerson tp)
    {
        switch(tp.currentState)
        {
            case TrappedPerson.State.Wandering:
                {
                    tp.mesh.material = materialsForPeeps[(int)PeepStateColors.WanderingScared];
                }
                break;
            case TrappedPerson.State.FollowPLayer:
                {
                    tp.mesh.material = materialsForPeeps[(int)PeepStateColors.FollowingPlayerSafe];
                }
                break;
            case TrappedPerson.State.EndOfLevel:
                {
                    tp.mesh.material = materialsForPeeps[(int)PeepStateColors.ExitedBuilding];
                }
                break;
        }
    }

    public float DistanceToPlayer(int indexInSnake)
    {
        float addedDist = indexInSnake * increasedRangeWithEachFollower;
        if (indexInSnake < 0)
            addedDist = 0;
        else
            addedDist = addedDist;
        return defaultDistanceToFollowPlayer + addedDist;
    }

    public Transform WhomDoIFollow(TrappedPerson tp)
    {
        GameObject go = snakeList[0].gameObject;
        foreach (var v in snakeList)
        {
            if(v.gameObject == tp.gameObject)// always look for the previous item in a chain
            {
                break;
            }
            go = v.gameObject;
        }
        return go.transform;
    }

    int GetIndex(Transform tp)
    {
        int index = 0;
        foreach (var v in snakeList)
        {
            if (v.gameObject == tp.gameObject)// always look for the previous item in a chain
            {
                return index;
            }
            index++;
        }
        return -1;
    }
    public int AddToSnake(Transform tp)
    {
        if(snakeList.Count == 0)
        {
            snakeList.Add(player);
        }
        if(snakeList.Contains(tp) == false)
            snakeList.Add(tp);

        return GetIndex(tp);
    }

    public void RemoveFromSnake(Transform tp)
    {
        snakeList.Remove(tp);
    }
}