using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrappedPerson2 : MonoBehaviour
{
    [SerializeField]
    internal Transform player;
    internal Vector3 originalPos;

    internal RigidBodyTest control;
    public PeepManager peepManager;

    public Renderer mesh;

    public float boundingRadius = 0.5f;

    int indexInSnake = 0;
    float originalForwardSpeedMultiplier = 0;

    public enum State
    {
        Wandering,
        FollowPLayer,
        EndOfLevel,
        RunningFromFire,
        HelpingFightFire,
        NumStates
    }

    TrappedState[] states;
    internal State currentState = State.Wandering;
    // Start is called before the first frame update
    void Start()
    {
        //SetupInitialState();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled == false || states == null)
            return;

        states[(int)currentState].Update(this);
    }

    //----------------------------------------------

    public void SetupInitialState(Transform _player)
    {
        if (states == null)
        {
            states = new TrappedState[(int)State.NumStates];
            states[(int)State.Wandering] = new StateWander();
            states[(int)State.FollowPLayer] = new StateFollowPlayer();
            states[(int)State.EndOfLevel] = new StateEndOfLevel();
        }
        control = GetComponent<RigidBodyTest>();
        if (originalPos == Vector3.zero)// init
            originalPos = transform.position;

      /*  character = GetComponent<ThirdPersonCharacter>();
        if (originalForwardSpeedMultiplier == 0)
            originalForwardSpeedMultiplier = character.ForwardSpeedMultiplier;
        else
            character.ForwardSpeedMultiplier = originalForwardSpeedMultiplier;*/

        transform.position = originalPos;
        //UnityEngine.AI.NavMeshAgent nma = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //nma.Warp(originalPos);

        currentState = State.Wandering;
        //peepManager.ChangeState(this);
        player = _player;
    }

    public bool IsPlayerCloseEnough()
    {
        if ((player.position - transform.position).magnitude < peepManager.DistanceToPlayer(indexInSnake))
        {
            return true;
        }
        return false;
    }
    public bool IsExitCloseEnough()
    {

        if ((peepManager.exitLocation.position - transform.position).magnitude < peepManager.distanceToExit)
        {
            return true;
        }
        return false;
    }
    //----------------------------------------------

    public abstract class TrappedState
    {
        public abstract bool Update(TrappedPerson2 tp);
    }
    public class StateFollowPlayer : TrappedState
    {
        public override bool Update(TrappedPerson2 tp)
        {
            if (tp.IsExitCloseEnough() == true)
            {
                tp.currentState = State.EndOfLevel;
                //tp.peepManager.ChangeState(tp);
                tp.peepManager.RemoveFromSnake(tp.transform);
                tp.indexInSnake = -1;
                return false;
            }
            //  else if (tp.IsPlayerCloseEnough() == true)
            {
                Vector3 playerPos = tp.peepManager.WhomDoIFollow(tp).position;
                Vector3 pos = tp.transform.position;
                Vector3 dist = (pos - playerPos);
                if (dist.sqrMagnitude < 3)
                {
                    tp.control.SetTarget(pos);
                    return true;
                }
                else
                {
                    // we want to stop just shy of the destination.
                    Vector3 dir = (dist).normalized;
                    dir.y = 0;
                    Vector3 dest = playerPos - dir;
                    tp.control.SetTarget(dest);
                }
            }
            /* else
             {
                 tp.currentState = State.Wandering;
                 tp.peepManager.ChangeState(tp);
                 tp.peepManager.RemoveFromSnake(tp.transform);
                 tp.indexInSnake = -1;
                 return false;
             }*/

            return true;
        }
    }
    public class StateEndOfLevel : TrappedState
    {
        public override bool Update(TrappedPerson2 tp)
        {
            //Vector3 pos = tp.transform.position;
            tp.control.SetTarget(tp.peepManager.GetFinalDestination());

            return true;
        }
    }
    class StateWander : TrappedState
    {
        //Time lastTimeChange;
        float minTimeToWaitBeforeNextLocation = 3;
        float randomRange = 5;
        float timeForNextChange;

        Transform originalLocation;
        public override bool Update(TrappedPerson2 tp)
        {
            if (originalLocation == null)
                originalLocation = tp.transform;

            if (timeForNextChange < Time.time)
            {
                Vector3 randomLocation = SelectRandomLocation();
                randomLocation = RaycastToPreventHittingObstacles(tp.transform.position, randomLocation, tp.boundingRadius);
                randomLocation.y = originalLocation.position.y;
                tp.control.SetTarget(randomLocation);
            }

            if (tp.IsPlayerCloseEnough() == true)
            {
                tp.currentState = State.FollowPLayer;
                //tp.peepManager.ChangeState(tp);
                tp.indexInSnake = tp.peepManager.AddToSnake(tp.transform);
                tp.control.SetTarget(tp.peepManager.WhomDoIFollow(tp).position);
                return false;
            }

            return true;
        }

        Vector3 SelectRandomLocation()
        {
            float randomTime = randomRange * Random.value - randomRange / 2;
            timeForNextChange = minTimeToWaitBeforeNextLocation + Time.time + randomTime;

            Vector3 position = originalLocation.position;
            Vector3 rand = Random.onUnitSphere * 3;
            position.x += rand.x;
            position.z += rand.z;
            return position;
        }


        Vector3 RaycastToPreventHittingObstacles(Vector3 start, Vector3 end, float boundingRadius)
        {
            Vector3 dir = end - start;
            RaycastHit hit;
            if (Physics.Raycast(start, dir, out hit, dir.magnitude, LayerMask.GetMask("Maze")))
            {
                Vector3 hitLocation = hit.point;
                Vector3 normal = hit.normal;

                
                Vector3 newEnd = hitLocation + normal * boundingRadius;//normal - Vector3.Project(-dir, normal);
                return newEnd;
            }
            return end;
        }
    }
}
