using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public GameObject swingAnchor;
    public TextMeshPro[] numberDisplays;
    private Animator animator;
    bool isOpen = false;
    bool isTriggerEnabled = false;



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

    public void SetDoorNumber(int num)
    {
        if(numberDisplays == null)
            return;
        foreach(var display in numberDisplays)
        {
            display.text = num.ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerEnabled == false)
            return;

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

    internal void EnableTrigger()
    {
        isTriggerEnabled = true;
    }

    internal void Reset()
    {
        isTriggerEnabled = false;
        OpenDoor("CloseTrigger");
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
