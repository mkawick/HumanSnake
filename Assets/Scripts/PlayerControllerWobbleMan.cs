using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerControllerWobbleMan : MonoBehaviour
{
    Vector3 originalClick;
    bool isMouseHeld = false;
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld == true)
        {
            if(isMouseHeld == true)
            {
                MoveInDirection();
            }
            else
            {
                originalClick = GetFingerPosition();
                isMouseHeld = true;
            }
        }
        else
        {
            isMouseHeld = false;
            StopInPlace();
        }
    }

    void MoveInDirection()
    {
        Vector3 vect = GetFingerPosition();
        var control = GetComponent<BasicPeepAnimController>();
        if (vect == originalClick)
        {
            if (control != null)
                control.StopPlayer();

            return;
        }
        if (control != null)
        {
            Vector3 dir = (vect - originalClick).normalized;
            control.MovePlayer(transform.position + dir*2);
        }
    }

    Vector3 GetFingerPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
        {
            return originalClick;
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