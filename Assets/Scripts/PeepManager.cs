using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepManager : MonoBehaviour
{
    List<TrappedPerson> peeps;
    List<Transform> snakeList;
    public Transform player;
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


    public void AddToSnake(Transform tp)
    {
        if(snakeList.Count == 0)
        {
            snakeList.Add(player);
        }
        if(snakeList.Contains(tp) == false)
            snakeList.Add(tp);
    }

    public void RemoveFromSnake(Transform tp)
    {
        snakeList.Remove(tp);
    }
}