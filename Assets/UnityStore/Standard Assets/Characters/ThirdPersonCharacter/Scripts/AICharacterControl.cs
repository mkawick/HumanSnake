using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public Vector3 targetPosition;                                    // target to aim for


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updateRotation = false;
            agent.updatePosition = true;
        }


        private void Update()
        {
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
           // Debug.Log("SetTarget :");

            this.target = target;
            if (this.target != null)
            {
                //Debug.Log("target != null");
                agent.SetDestination(targetPosition);
            }
            else
            {
                //Debug.Log("target == null");
                try
                {
                    //if (agent.isActiveAndEnabled == false)
                    {
                        this.target = target;
                    }
                    agent.SetDestination(transform.position);
                }
                catch
                {
                    this.target = target;
                }
            }
        }

        public void SetTarget(Vector3 position)
        {
            this.target = null;

            agent.SetDestination(position);
        }
    }
}
