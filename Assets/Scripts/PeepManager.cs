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
    float timeWhenDancingEnds = 0;

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
        if(Input.GetMouseButtonUp(1) == true)
        {
            gameManager.PlayFail(GetRandomPeep());
        }
    }

    public void PersonDied(TrappedPerson2 person)
    {
        gameManager.PlayFail(person);
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
                        //tp.mesh.material = materialsForPeeps[(int)PeepStateColors.WanderingScared];
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.WanderingScared];
                        if (sprite != null)
                            tp.SetEmoticon(sprite);
                    }
                    break;
                case TrappedPerson2.State.FollowPLayer:
                    {
                        //tp.mesh.material = materialsForPeeps[(int)PeepStateColors.FollowingPlayerSafe];
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.FollowingPlayerSafe];
                       // CIC if (sprite != null)
                            tp.SetEmoticon(sprite);
                    }
                    break;
                case TrappedPerson2.State.EndOfLevel:
                    {
                        //tp.mesh.material = materialsForPeeps[(int)PeepStateColors.ExitedBuilding];
                        Sprite sprite = emoticons[(int)PeepStateEmoticons.ExitedBuilding];
                        //CIC if (sprite != null)
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
        else
        {
            tp.SetEmoticon(null);
        }
    }

    internal void MakeEveryoneDance(Vector3 centerSpot, Vector3 facingLocation, List<TrappedPerson2> peepList, float timeEnds)
    {
        peeps = peepList;
        foreach (var peep in peeps)
        {
            Vector3 position = UnityEngine.Random.onUnitSphere;
            position.y = 0;
            float dist = Random.Range(1, 4);
            position *= dist; 
            position += centerSpot;
            DancingController dc = peep.GetComponent<DancingController>();
            if (dc)
            {
                peep.GetComponent<BasicPeepAnimController>().EnableControllerComponents(false);
                dc.enabled = true;
                dc.PrepToDance(position, facingLocation, timeEnds);
            }
        }
        timeWhenDancingEnds = timeEnds;
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