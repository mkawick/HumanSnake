using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class TrappedPerson : MonoBehaviour
{
    [SerializeField]
    internal Transform player;
    internal Transform thisTransform;

    internal AICharacterControl control;
    public PeepManager peepManager;

    public enum State
    {
        Wandering,
        FollowPLayer,
        RunningFromFire,
        HelpingFightFire,
        NumStates
    }

    TrappedState[] states;
    internal State currentState = State.Wandering;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<AICharacterControl>();
        thisTransform = this.transform;
        states = new TrappedState[(int)State.NumStates];
        //foreach(var s in State)
        states[(int)State.Wandering] = new StateWander();
        states[(int)State.FollowPLayer] = new StateFollowPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        states[(int)currentState].Update(this);
    }

    //----------------------------------------------

    public bool IsPlayerCloseEnough()
    {
        if ((player.position - transform.position).magnitude < 2.5f)
        {
            return true;
        }
        return false;
    }
    public abstract class TrappedState
    {
        public abstract bool Update(TrappedPerson tp);
    }
    public class StateFollowPlayer : TrappedState
    {
        public override bool Update(TrappedPerson tp)
        {
            if (tp.IsPlayerCloseEnough() == false)
            {
                tp.currentState = State.Wandering;
                tp.peepManager.RemoveFromSnake(tp.transform);
                return false;
            }

            return true;
        }
    }
    class StateWander : TrappedState
    {
        //Time lastTimeChange;
        float maxTimeToWaitBeforeNextLocation = 5;
        float timeForNextChange;

        Transform originalLocation;
        public override bool Update(TrappedPerson tp)
        {
            if (originalLocation == null)
                originalLocation = tp.transform;

            if(timeForNextChange < Time.time)
            {
                timeForNextChange = maxTimeToWaitBeforeNextLocation + Time.time;

                Vector3 position = originalLocation.position;
                Vector3 rand = Random.onUnitSphere * 3;
                position.x += rand.x;
                position.z += rand.z;
                tp.control.SetTarget(position);
            }

            if(tp.IsPlayerCloseEnough() == true)
            {
                tp.currentState = State.FollowPLayer;
                tp.peepManager.AddToSnake(tp.transform);
                tp.control.SetTarget(tp.peepManager.WhomDoIFollow(tp));
                //tp.control.SetTarget(tp.player.transform);
                return false;
            }

            return true;
        }
    }
}
