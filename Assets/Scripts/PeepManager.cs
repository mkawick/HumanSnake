using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepManager : MonoBehaviour
{
    public Sprite[] emoticons;
    [SerializeField]
    bool showEmoticons = false;
    [SerializeField]
    float oddsOfSettingEmoticon = 1.0f;
    List<TrappedPerson2> peeps;
    List<Transform> snakeList;
    public Transform player;
    public GameManager gameManager;
    public float defaultDistanceToFollowPlayer = 2.2f;
    public float increasedRangeWithEachFollower = 0.9f;

    public float followingPlayerSpeedMultipler = 1.1f;

    internal Transform exitLocation;
    public float distanceToExit = 1.2f;

    public Material[] materialsForPeeps;
    internal LevelManager levelManager;

    [SerializeField]
    GameObject floorCircle = null;

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

    bool isWaitingForDancingToEnd = false;
    bool isWaitingForFailureToEnd = false;
    float timeWhenStateEnds = 0;
    void Start()
    {
        peeps = new List<TrappedPerson2>();
    }

    public TrappedPerson2 GetRandomPeep()
    {
        int index = Random.Range(0, peeps.Count);
        return peeps[index];
    }

    public void NewLevel(List<TrappedPerson2> peepList)
    {
        peeps = peepList;
        foreach(var peep in peeps)
        {
            peep.SetupInitialState(player);
            if (floorCircle != null)
            {
                if (peep.floorCircle)
                    Destroy(peep.floorCircle);

                var obj = Utils.GetChildWithName(peep.gameObject, "FloorCircle");

                if (obj)
                {
                    peep.floorCircle = Instantiate<GameObject>(floorCircle, obj.transform);
                }

            }
        }
        snakeList = new List<Transform>();
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(1) == true)// testing
        {
            //gameManager.PlayFail();
            levelManager.FinishLevel();
        }
        if(isWaitingForDancingToEnd == true)
        {
            if(timeWhenStateEnds < Time.time)
            {
                timeWhenStateEnds = 0;
                isWaitingForDancingToEnd = false;
                CleanupFromDancing();
            }
        }
        if (isWaitingForFailureToEnd == true)
        {
            if (timeWhenStateEnds < Time.time)
            {
                timeWhenStateEnds = 0;
                isWaitingForFailureToEnd = false;
                CleanupFromFailure();
            }
        }
    }

    public void PersonDied()
    {
        gameManager.PlayFail();
        GameObject joelDancingSpot = null;
        levelManager.GetFailSpots(ref joelDancingSpot);
        player.GetComponent<BasicPeepAnimController>().EnableControllerComponents(false);
        player.GetComponent<BasicPeepAnimController>().StopPlayer();

        player.transform.position = joelDancingSpot.transform.position;
        player.transform.rotation = joelDancingSpot.transform.rotation;
        timeWhenStateEnds = gameManager.playFailTime + Time.time;
        isWaitingForFailureToEnd = true;
    }
    internal void CleanupFromFailure()
    {
        player.GetComponent<BasicPeepAnimController>().EnableControllerComponents(true);
        player.GetComponent<BasicPeepAnimController>().StopPlayer();
    }

    public void ChangeState(TrappedPerson2 tp, TrappedPerson2.State currentState)
    {
        if (showEmoticons == false)
            return;
        if (Random.value < oddsOfSettingEmoticon)
        {
            switch (currentState)
            {
                case TrappedPerson2.State.Wandering:
                    {
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.WanderingScared];
                        if (sprite != null)
                            tp.SetEmoticon(sprite);
                    }
                    break;
                case TrappedPerson2.State.FollowPLayer:
                    {
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.FollowingPlayerSafe];
                         tp.SetEmoticon(sprite);
                    }
                    break;
                case TrappedPerson2.State.EndOfLevel:
                    {
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.ExitedBuilding];
                        tp.SetEmoticon(sprite);
                    }
                    break;
                case TrappedPerson2.State.Wave:
                    {
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.WavingForHelp];
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
        else
        {
            tp.SetEmoticon(null);
        }
    }

    internal void MakeEveryoneDance(List<Transform> spots, List<TrappedPerson2> peepList, JoelAnimator player, Transform joelDancingSpot, float timeEnds)
    {
        peeps = peepList;
        int index = 0;
        foreach (var peep in peeps)
        {
            DancingController dc = peep.GetComponent<DancingController>();
            if (dc)
            {
                peep.GetComponent<BasicPeepAnimController>().EnableControllerComponents(false);
                dc.enabled = true;
                dc.PrepToDance(spots[index].position, spots[index].rotation, timeEnds);
            }
            index++;
        }

        player.GetComponent<BasicPeepAnimController>().EnableControllerComponents(false);
        player.transform.position = joelDancingSpot.position;
        player.transform.rotation = joelDancingSpot.rotation;
        timeWhenStateEnds = timeEnds + Time.time;
        isWaitingForDancingToEnd = true;

    }

    internal void CleanupFromDancing()
    {
        foreach (var peep in peeps)
        {
            DancingController dc = peep.GetComponent<DancingController>();
            if (dc)
            {
                peep.GetComponent<BasicPeepAnimController>().EnableControllerComponents(true);
                dc.enabled = false;
            }
        }
        player.GetComponent<BasicPeepAnimController>().EnableControllerComponents(true);
    }

    public float DistanceToPlayer(int indexInSnake)
    {
        float addedDist = indexInSnake * increasedRangeWithEachFollower;
        if (indexInSnake < 0)
            addedDist = 0;
      /*  else
            addedDist = addedDist;*/
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
        if (snakeList == null)
            return 0;
        int total = snakeList.Count;
        return includePlayer ? total : total -1;
    }
}