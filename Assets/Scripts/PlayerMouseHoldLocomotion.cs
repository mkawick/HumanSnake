using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMouseHoldLocomotion : MonoBehaviour
{
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld)
        {
            var control = GetComponent<BasicPeepAnimController>();
            Vector3 currentPosition = this.transform.position;
            Vector3 newPosition = GetClickPosition(currentPosition);
            if (currentPosition == newPosition)
            {
                if (control != null) 
                    control.StopPlayer();

                return;
            }
            if(control != null)
                control.MovePlayer(newPosition);
        }
    }
    Vector3 GetClickPosition(Vector3 currentPosition)
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
