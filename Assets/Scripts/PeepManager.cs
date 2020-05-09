using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepManager : MonoBehaviour
{
    public Sprite[] emoticons;
    List<TrappedPerson2> peeps;
    List<Transform> snakeList;
    public Transform player;
    public float defaultDistanceToFollowPlayer = 2.2f;
    public float increasedRangeWithEachFollower = 0.9f;
    
    public float followingPlayerSpeedMultipler = 1.1f;

    internal Transform exitLocation;
    public float distanceToExit = 1.2f;

    public Material[] materialsForPeeps;
    internal LevelManager levelManager;

    enum PeepStateColors
    {
        WanderingScared,
        FollowingPlayerSafe,
        ExitedBuilding
    }
    enum PeepStateEmoticons
    {
        WanderingScared,
        WavingForHelp,
        FollowingPlayerSafe,
        ExitedBuilding
    }
    void Start()
    {
        peeps = new List<TrappedPerson2>();
    }

    public void NewLevel(List<TrappedPerson2> peepList)
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

    public void ChangeState(TrappedPerson2 tp, TrappedPerson2.State currentState)
    {
        switch(currentState)
        {
            case TrappedPerson2.State.Wandering:
                {
                    //tp.mesh.material = materialsForPeeps[(int)PeepStateColors.WanderingScared];
                    Sprite sprite = emoticons[(int)PeepStateEmoticons.WanderingScared];
                    if(sprite != null)
                        tp.SetEmoticon(sprite);
                }
                break;
            case TrappedPerson2.State.FollowPLayer:
                {
                    //tp.mesh.material = materialsForPeeps[(int)PeepStateColors.FollowingPlayerSafe];
                    Sprite sprite = emoticons[(int)PeepStateEmoticons.FollowingPlayerSafe];
                    if (sprite != null)
                        tp.SetEmoticon(sprite);
                }
                break;
            case TrappedPerson2.State.EndOfLevel:
                {
                    //tp.mesh.material = materialsForPeeps[(int)PeepStateColors.ExitedBuilding];
                    Sprite sprite = emoticons[(int)PeepStateEmoticons.ExitedBuilding];
                    if (sprite != null)
                        tp.SetEmoticon(sprite);
                }
                break;
            case TrappedPerson2.State.Wave:
                {
                    Sprite sprite = emoticons[(int)PeepStateEmoticons.WavingForHelp];
                    if (sprite != null)
                        tp.SetEmoticon(sprite);
                }
                break;
            default:
                {
                    tp.SetEmoticon(null);
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

    public Transform WhomDoIFollow(TrappedPerson2 tp)
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
    public Transform GetFinalDestination()
    {
        return exitLocation;
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

    public int GetNumInSnake(bool includePlayer = false)
    {
        int total = snakeList.Count;
        return includePlayer ? total : total -1;
    }
}