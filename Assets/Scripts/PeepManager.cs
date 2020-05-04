using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepManager : MonoBehaviour
{
    List<TrappedPerson> peeps;
    List<Transform> snakeList;
    public Transform player;
    public float defaultDistanceToFollowPlayer = 2.5f;
    public float increasedRangeWithEachFollower = 0.2f;
    void Start()
    {
        peeps = new List<TrappedPerson>();
        NewLevel();
    }

    void NewLevel()
    {
        snakeList = new List<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public float DistanceToPlayer(int indexInSnake)
    {
        float addedDist = indexInSnake * increasedRangeWithEachFollower;
        if (indexInSnake < 0)
            addedDist = 0;
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