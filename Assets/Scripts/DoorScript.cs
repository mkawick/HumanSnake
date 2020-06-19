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

    public bool blocksLevelEnd = true;
    bool isTriggerEnabled = false;
    public bool showDoorNumber = true;
    public bool isClosable = false;

    enum Direction
    {
        open, close
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
            OpenDoor(Direction.open);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerControllerWobbleMan>();
        if (player != null)
        {
            OpenDoor(Direction.close);
        }
    }

    internal void EnableTrigger()
    {
        isTriggerEnabled = true;
    }

    internal void Reset()
    {
        isTriggerEnabled = false;
        OpenDoor(Direction.close, true);
    }

    void OpenDoor(Direction direction, bool ignoreFlags = false)
    {
        if(direction == Direction.open)
        {
            if(animator)
                animator.SetBool("isOpen", true);
        }
        else if (direction == Direction.close)
        {
            if (ignoreFlags == false)
            {
                if (isClosable == false)
                    return;
            }
            if(animator)
                animator.SetBool("isOpen", false);
        }
    }
}
