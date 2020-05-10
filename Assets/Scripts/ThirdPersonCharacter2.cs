using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace UnityStandardAssets.Characters.ThirdPerson
//{
//[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter2 : MonoBehaviour
{
    private Animator animator;
    //[SerializeField] float m_MovingTurnSpeed = 360;
    //[SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_AnimSpeedMultiplier = 1.0f;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float forwardSpeedMultiplier = 1.0f;

    //Rigidbody rigidbody;
    //Vector3 m_GroundNormal;
    float m_TurnAmount;
    float m_ForwardAmount;

    float lastClickedAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //m_Animator = GetComponent<Animator>();
        //rigidbody = GetComponent<Rigidbody>();
        animator.applyRootMotion = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //m_Capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        {
           // animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            animator.speed = m_AnimSpeedMultiplier;
            if (Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
                v.x = 1.10f;
                v.z = 1.10f;

                // we preserve the existing y part of the current velocity.
                v.y = GetComponent<Rigidbody>().velocity.y;
                GetComponent<Rigidbody>().velocity = v;
                GetComponent<Rigidbody>().position += new Vector3(0, 0, 1);

                transform.rotation = Quaternion.AngleAxis(lastClickedAngle * Time.deltaTime, Vector3.up);
                //gameObject.transform.position += new Vector3(0, 0, 1);
            }
        }
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        float dist = move.magnitude;
        if (dist == 0)
            return;

        if (dist > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        //move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);

        m_ForwardAmount = move.z * forwardSpeedMultiplier;

        animator.SetTrigger("Run");
        lastClickedAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
        
        //gameObject.transform.rotation.
    }
   /* public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (Time.deltaTime > 0)
        {
            Vector3 v = (animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
            v.x = 0.01f;

            // we preserve the existing y part of the current velocity.
            v.y = rigidbody.velocity.y;
            rigidbody.velocity = v;
        }
    }*/
}
//}
