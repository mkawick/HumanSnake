using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrappedPerson2 : MonoBehaviour
{
    private GameObject emoticonRoot;
    internal GameObject floorCircle;
    public SpriteRenderer emoticon;
    internal Transform player;
    internal Vector3 originalPos;

    internal BasicPeepAnimController control;
    internal PeepManager peepManager;

    //public Renderer mesh;

    public float boundingRadius = 0.5f;
    public float wanderRange = 1.5f;

    int indexInSnake = 0;

    public enum State
    {
        Wandering,
        FollowPLayer,
        EndOfLevel,
        Wave, 
        Transitioning
    }

    TrappedState[] states;
    internal State currentState = State.Wandering;
    void Start()
    {
        GrabAnimConroller();
        SetupEmoticonSprite();
        SetupStateTree();
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

    void CreateStateTree()
    {
        var count = Enum.GetNames(typeof(State)).Length;
        states = new TrappedState[count];
        states[(int)State.Wandering] = new StateWander();
        states[(int)State.FollowPLayer] = new StateFollowPlayer();
        states[(int)State.Wave] = new StateWave();
        states[(int)State.EndOfLevel] = new StateEndOfLevel();
        states[(int)State.Transitioning] = new StateTransitioning();
        foreach (var state in states)
        {
            state.Init(this);
        }
    }
    void SetupStateTree()
    {
        if (states == null)
        {
            CreateStateTree();
        }
        GrabAnimConroller();

        currentState = State.Wandering;
        if (peepManager)
            peepManager.ChangeState(this, State.Transitioning);
    }

    void GrabAnimConroller()
    {
        control = GetComponent<BasicPeepAnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled == false || states == null)
            return;

        if(states[(int)currentState].Update() == false)
        {
            if(currentState == State.Wandering || currentState == State.Wave)
            {
                switch(UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                        currentState = State.Wandering;
                        if(peepManager)
                            peepManager.ChangeState(this, State.Wandering);
                        break;
                    case 1:
                        currentState = State.Wave;
                        if (peepManager)
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
        Vector3 camera = Camera.main.transform.position;
        /*  Vector3 cameraDir = Camera.main.transform.forward;
          Vector3 root = emoticonRoot.transform.forward;
          //root.y = cameraDir.y;
          root.y = cameraDir.y;
          root.z = cameraDir.z;
          //root.y = 0;
          //root.z = 0;
          emoticonRoot.transform.forward = root;*/
        /* Vector3 clampedTargetPos = new Vector3(camera.x, transform.position.y, camera.z);
         transform.LookAt(clampedTargetPos);*/
        Vector3 dir = camera - gameObject.transform.position;
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            emoticonRoot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
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
        SetupStateTree();
        control = GetComponent<BasicPeepAnimController>();
        if (originalPos == Vector3.zero)// init
            originalPos = transform.position;
        control.SetTarget(originalPos);
        transform.position = originalPos;
        
        player = _player;
    }

    public bool IsPlayerCloseEnough()
    {
        if (player == null || peepManager == null)
            return false;
        if ((player.position - transform.position).magnitude < peepManager.DistanceToPlayer(indexInSnake))
        {
            return true;
        }
        return false;
    }
    public bool IsExitCloseEnough()
    {
        if (peepManager == null)
            return false;

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
        protected TrappedPerson2 person;
        public abstract void Init(TrappedPerson2 tp);
        public abstract bool Update();
        protected void RandomizeTimeForNextChange()
        {
            float randomTime = randomRange * UnityEngine.Random.value - randomRange / 2;
            timeForNextChange = minTimeToWaitBeforeNextLocation + Time.time + randomTime;
            if(person.control != null)
                person.control.Log("TimePlannedForChange:" + timeForNextChange + ", CT: " + Time.time);
        }
    }
    public class StateFollowPlayer : TrappedState
    {
        public override void Init(TrappedPerson2 tp)
        {
            person = tp;
            

        }
        public override bool Update()
        {
            if (person.IsExitCloseEnough() == true)
            {
                person.currentState = State.EndOfLevel;
                if (person.peepManager)
                {
                    person.peepManager.ChangeState(person, State.Transitioning);
                    person.peepManager.RemoveFromSnake(person.transform);
                }
                person.indexInSnake = -1;
                return false;
            }
            //  else if (person.IsPlayerCloseEnough() == true)
            {
                Vector3 playerPos = Vector3.zero;
                if (person.peepManager != null)
                {
                    playerPos = person.peepManager.WhomDoIFollow(person).position;
                }
                Vector3 pos = person.transform.position;
                Vector3 dist = (pos - playerPos);
                if (dist.sqrMagnitude < 3)
                {
                    person.control.SetTarget(pos);
                    return true;
                }
                else
                {
                    // we want to stop just shy of the destination.
                    Vector3 dir = (dist).normalized;
                    dir.y = 0;
                    Vector3 dest = playerPos - dir;
                    person.control.SetTarget(dest);
                }
            }
            /* else
             {
                 person.currentState = State.Wandering;
                 person.peepManager.ChangeState(tp);
                 person.peepManager.RemoveFromSnake(person.transform);
                 person.indexInSnake = -1;
                 return false;
             }*/

            return true;
        }
    }
    public class StateEndOfLevel : TrappedState
    {
        public override void Init(TrappedPerson2 tp)
        {
            person = tp;
           
        }
        public override bool Update()
        {
            if (person.peepManager)
                person.control.SetTarget(person.peepManager.GetFinalDestination());

            return true;
        }
    }
    class StateWander : TrappedState
    {
        Transform startingLocation;
        public override void Init(TrappedPerson2 tp)
        {
            person = tp;

            if (startingLocation == null)
                startingLocation = person.transform;
            RandomizeTimeForNextChange();
        }
        public override bool Update()
        {
            if (timeForNextChange < Time.time)
            {
                Vector3 randomLocation = SelectRandomLocation(person.wanderRange);
                randomLocation = RaycastToPreventHittingObstacles(person.transform.position, randomLocation, person.boundingRadius);
                randomLocation.y = startingLocation.position.y;
                person.control.SetTarget(randomLocation);
                return false;// ready for new state
            }
            else
            {
                //person.peepManager.ChangeState(tp, State.Transitioning);
                //person.control.ta
            }

            if (person.IsPlayerCloseEnough() == true)
            {
                person.currentState = State.FollowPLayer;
                if (person.peepManager)
                {
                    person.peepManager.ChangeState(person, State.FollowPLayer);
                    person.indexInSnake = person.peepManager.AddToSnake(person.transform);
                    person.control.SetTarget(person.peepManager.WhomDoIFollow(person).position);
                    //CIC [Capi is Coding]
                    person.floorCircle.SetActive(false);
                }
                return false;
            }

            return true;
        }

        Vector3 SelectRandomLocation(float range)
        {
            RandomizeTimeForNextChange();

            Vector3 position = startingLocation.position;
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
            person = tp;
            RandomizeTimeForNextChange();
        }
        public override bool Update()
        {
            if (timeForNextChange < Time.time)
            {
                person.control.Wave();
                return false;// ready for new state
            }
            else
            {
                //person.peepManager.ChangeState(tp, State.Transitioning);
            }
            if (person.IsPlayerCloseEnough() == true)
            {
                person.currentState = State.FollowPLayer;
                if (person.peepManager)
                {
                    person.peepManager.ChangeState(person, State.FollowPLayer);
                    person.indexInSnake = person.peepManager.AddToSnake(person.transform);
                    person.control.SetTarget(person.peepManager.WhomDoIFollow(person).position);
                    //CIC [Capi is Coding]
                    person.floorCircle.SetActive(false);
                }
                return false;
            }

            return true;
        }
    }
    public class StateTransitioning : TrappedState
    {
        public override void Init(TrappedPerson2 tp)
        {
            person = tp;
            RandomizeTimeForNextChange();
        }
        public override bool Update()
        {
            //person.peepManager.ChangeState(tp, State.Transitioning);

            return true;
        }
    }
}
