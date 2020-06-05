using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerControllerWobbleMan : MonoBehaviour
{
    Vector3 lastFingerPosition;
    bool isMouseHeld = false;
    BasicPeepAnimController control;
    public float minimalRangeCheckToMove = 0.01f;
    private void Start()
    {
        control = GetComponent<BasicPeepAnimController>();
    }
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld == true)
        {
            if (isMouseHeld == true)
            {
                Vector3 vect = GetFingerPosition();
                MoveInDirection(vect);
                lastFingerPosition = vect;
            }
            else
            {
                lastFingerPosition = GetFingerPosition();
                isMouseHeld = true;
            }
        }
        else
        {
            isMouseHeld = false;
            StopInPlace();
        }
    }

    void MoveInDirection(Vector3 vect)
    {
        if ((vect - lastFingerPosition).magnitude < minimalRangeCheckToMove) // todo, turn into a range check... slow moving or small movements should be ignored
        {
            if (control != null)
                control.StopPlayer();

            return;
        }
        if (control != null)
        {
            Vector3 dir = (vect - lastFingerPosition).normalized;
            control.MovePlayer(transform.position + dir*2);
        }
    }

    Vector3 GetFingerPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
        {
            return lastFingerPosition;
        }
        return hit.point;
    }

    void StopInPlace()
    {
        var control = GetComponent<BasicPeepAnimController>();
        if (control != null)
            control.StopPlayer();
    }

}