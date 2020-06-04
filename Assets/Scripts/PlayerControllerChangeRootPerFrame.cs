using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControllerChangeRootPerFrame : MonoBehaviour
{
    BasicPeepAnimController control;
    private void Start()
    {
        control = GetComponent<BasicPeepAnimController>();
    }
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (control != null)
        {
            if (isButtonHeld)
            {
                Vector3 currentPosition = this.transform.position;
                Vector3 newPosition = GetFingerPosition(currentPosition);
                if (currentPosition != newPosition)
                {
                    control.MovePlayer(newPosition);
                    return;
                }
            }
            control.StopPlayer();
        }
    }
    Vector3 GetFingerPosition(Vector3 currentPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
        {
            return currentPosition;
        }
        return hit.point;
    }
}

