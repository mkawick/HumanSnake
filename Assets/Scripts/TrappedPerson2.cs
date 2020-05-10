using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrappedPerson2 : MonoBehaviour
{
    private GameObject emoticonRoot;
    public SpriteRenderer emoticon;
    internal Transform player;
    internal Vector3 originalPos;

    internal RigidBodyTest control;
    public PeepManager peepManager;

    //public Renderer mesh;

    public float boundingRadius = 0.5f;
    public float wanderRange = 1.5f;

    int indexInSnake = 0;
    float originalForwardSpeedMultiplier = 0;

    public enum State
    {
        Wandering,
        FollowPLayer,
        EndOfLevel,
        Wave, 
        Transitioning/*,
        RunningFromFire,
        HelpingFightFire,
        NumStates*/
    }

    TrappedState[] states;
    internal State currentState = State.Wandering;
    // Start is called before the first frame update
    void Start()
    {
        //SetupInitialState();

        SetupEmoticonSprite();
    }
    void SetupEmoticonSprite()
    {
        emoticonRoot = new GameObject();
        emoticonRoot.name = "Emoticon";
        emoticonRoot.transform.parent = transform;
        emoticonRoot.transform.localPosition = new Vector3(0, 1.75f, 0);
        emoticonRoot.transform.rotation = Quaternion.Euler(-90, 0, 0);
        float scale = 0.8f;
        emoticonRoot.transform.localScale = new Vector3(scale, scale, scale);
        emoticon = emoticonRoot.AddComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled == false || states == null)
            return;

        if(states[(int)currentState].Update(this) == false)
        {
            if(currentState == State.Wandering || currentState == State.Wave)
            {
                switch(UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                        currentState = State.Wandering;
                        peepManager.ChangeState(this, State.Wandering);
                        break;
                    case 1:
                        currentState = State.Wave;
                        peepManager.ChangeState(this, State.Wave);
                        break;
                }
                states[(int)currentState].Init(this);
            }
        }
        UpdateEmoticonRotation();
    }

    void UpdateEmoticonRotation()
    {
        emoticonRoot.transform.forward = Camera.main.transform.forward;
    }

    public void SetEmoticon(Sprite sprite)
    {
        if (emoticon)
        {
            if (sprite == null)
            {
                emoticonRoot.SetActive(false);
            }
            else
            {
                emoticonRoot.SetActive(true);
                emoticon.sprite = sprite;
            }
            
        }
    }

    //----------------------------------------------

    public void SetupInitialState(Transform _player)
    {
        if (states == null)
        {
            var count = Enum.GetNames(typeof(State)).Length;
            states = new TrappedState[count];
            states[(int)State.Wandering] = new StateWander();
            states[(int)State.FollowPLayer] = new StateFollowPlayer();
            states[(int)State.Wave] = new StateWave();
            states[(int)State.EndOfLevel] = new StateEndOfLevel();
            states[(int)State.Transitioning] = new StateTransitioning();
            
        }
        control = GetComponent<RigidBodyTest>();
        if (originalPos == Vector3.zero)// init
            originalPos = transform.position;

        transform.position = originalPos;

        currentState = State.Wandering;
        peepManager.ChangeState(this, State.Transitioning);
        player = _player;

        foreach(var state in states)
        {
            state.Init(this);
        }
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
        protected float randomRange = 5;
        protected float minTimeToWaitBeforeNextLocation = 3;
        protected float timeForNextChange;
        public abstract void Init(TrappedPerson2 tp);
        public abstract bool Update(TrappedPerson2 tp);
        protected void RandomizeTimeForNextChange()
        {
            float randomTime = randomRange * UnityEngine.Random.value - randomRange / 2;
            timeForNextChange = minTimeToWaitBeforeNextLocation + Time.time + randomTime;
        }
    }
    public class StateFollowPlayer : TrappedState
    {
        public override void Init(TrappedPerson2 tp)
        {
        }
        public override bool Update(TrappedPerson2 tp)
        {
            if (tp.IsExitCloseEnough() == true)
            {
                tp.currentState = State.EndOfLevel;
                tp.peepManager.ChangeState(tp, State.Transitioning);
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
        public override void Init(TrappedPerson2 tp)
        {
        }
        public override bool Update(TrappedPerson2 tp)
        {
            //Vector3 pos = tp.transform.position;
            tp.control.SetTarget(tp.peepManager.GetFinalDestination());

            return true;
        }
    }
    class StateWander : TrappedState
    {        

        Transform originalLocation;
        public override void Init(TrappedPerson2 tp)
        {
            if (originalLocation == null)
                originalLocation = tp.transform;

            RandomizeTimeForNextChange();
        }
        public override bool Update(TrappedPerson2 tp)
        {
            if (timeForNextChange < Time.time)
            {
                Vector3 randomLocation = SelectRandomLocation(tp.wanderRange);
                randomLocation = RaycastToPreventHittingObstacles(tp.transform.position, randomLocation, tp.boundingRadius);
                randomLocation.y = originalLocation.position.y;
                tp.control.SetTarget(randomLocation);
                return false;// ready for new state
            }
            else
            {
                //tp.peepManager.ChangeState(tp, State.Transitioning);
            }

            if (tp.IsPlayerCloseEnough() == true)
            {
                tp.currentState = State.FollowPLayer;
                tp.peepManager.ChangeState(tp, State.FollowPLayer);
                tp.indexInSnake = tp.peepManager.AddToSnake(tp.transform);
                tp.control.SetTarget(tp.peepManager.WhomDoIFollow(tp).position);
                return false;
            }

            return true;
        }

        Vector3 SelectRandomLocation(float range)
        {
            RandomizeTimeForNextChange();

            Vector3 position = originalLocation.position;
            Vector3 rand = UnityEngine.Random.onUnitSphere * range;
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
                Vector3 newEnd = hitLocation + normal * boundingRadius;
                return newEnd;
            }
            return end;
        }
    }
    public class StateWave : TrappedState
    {
        public override void Init(TrappedPerson2 tp)
        {
            RandomizeTimeForNextChange();
        }
        public override bool Update(TrappedPerson2 tp)
        {
            if (timeForNextChange < Time.time)
            {
                tp.control.Wave();
                return false;// ready for new state
            }
            else
            {
                //tp.peepManager.ChangeState(tp, State.Transitioning);
            }
            if (tp.IsPlayerCloseEnough() == true)
            {
                tp.currentState = State.FollowPLayer;
                tp.peepManager.ChangeState(tp, State.FollowPLayer);
                tp.indexInSnake = tp.peepManager.AddToSnake(tp.transform);
                tp.control.SetTarget(tp.peepManager.WhomDoIFollow(tp).position);
                return false;
            }

            return true;
        }
    }
    public class StateTransitioning : TrappedState
    {
        public override void Init(TrappedPerson2 tp)
        {
            RandomizeTimeForNextChange();
        }
        public override bool Update(TrappedPerson2 tp)
        {
            //tp.peepManager.ChangeState(tp, State.Transitioning);

            return true;
        }
    }
}
