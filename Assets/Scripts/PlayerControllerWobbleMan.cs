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
    bool isButtonHeld = false;
    private Vector3 currentDirection = new Vector3();
    private void Start()
    {
        control = GetComponent<BasicPeepAnimController>();
    }
    private void Update()
    {
        isButtonHeld = Input.GetMouseButton(0);
    }

    private void FixedUpdate()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld == true)
        {
            if (isMouseHeld == true)
            {
                Vector3 vect = GetFingerPosition();
                //Debug.Log(vect);
                MoveInDirection(vect);
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
        vect.y = 0;// this.transform.position.y;
        if ((vect - lastFingerPosition).magnitude < minimalRangeCheckToMove) // todo, turn into a range check... slow moving or small movements should be ignored
        {

            // CIC changing the controller to continue moving on direction even if mouse is down
            control.MovePlayer(transform.position + currentDirection );
            
            //StopInPlace();

            return;
        }
        Vector3 dir = (vect - lastFingerPosition).normalized;
        currentDirection = dir;
        control.MovePlayer(transform.position + dir*2);
        lastFingerPosition = vect;
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
        control.StopPlayer();
    }

}