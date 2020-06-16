using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public GameObject swingAnchor;
    List<TextMeshPro> numberDisplays;
    private Animator animator;
    bool isOpen = false;

    public bool blocksLevelEnd = true;
    bool isTriggerEnabled = false;
    public bool showDoorNumber = true;
    public bool isClosable = false;

    enum AnimClips
    {
        idle, open, close
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        var numbers = GetComponentsInChildren<TextMeshPro>();
        numberDisplays = new List<TextMeshPro>();
        foreach (var number in numbers)
        {
            numberDisplays.Add(number);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(showDoorNumber == false)
        {
            foreach (var display in numberDisplays)
            {
                display.enabled = false;
            }
        }
        else
        {
            foreach (var display in numberDisplays)
            {
                display.enabled = true;
            }
        }
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
        // we don't care about enabling the trigger if we are not blocking
        if (isTriggerEnabled == false && blocksLevelEnd == true)
            return;

        var player = other.GetComponent<PlayerControllerWobbleMan>();
        if(player != null)
        {
            OpenDoor("OpenTrigger");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerControllerWobbleMan>();
        if (player != null)
        {
            OpenDoor("CloseTrigger");
        }
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
            if (isOpen == true )
                return;
            animator.SetTrigger(direction);
            isOpen = true;
        }
        else if (direction == "CloseTrigger")
        {
            if (isOpen == false || isClosable == false)
                return;
            animator.SetTrigger(direction);
            isOpen = false;
        }
        

    }
}
