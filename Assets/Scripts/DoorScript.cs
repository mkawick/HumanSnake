using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject swingAnchor;
    private Animator animator;
    bool isOpen = false;

    enum AnimClips
    {
        idle, open, close
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<RigidBodyTest>();
        if(player != null)
        {
            OpenDoor("OpenTrigger");
        }
    }
    private void OnTriggerExit(Collider other)
    {
       /* var player = other.GetComponent<PlayerMouseHoldLocomotion>();
        if (player != null)
        {
            OpenDoor("CloseTrigger");
        }*/
    }

    void OpenDoor(string direction)
    {
        if(direction == "OpenTrigger")
        {
            if (isOpen == true)
                return;
            animator.SetTrigger(direction);
            isOpen = true;
        }
        else if (direction == "CloseTrigger")
        {
            if (isOpen == false)
                return;
            animator.SetTrigger(direction);
            isOpen = false;
        }
        

    }
}
