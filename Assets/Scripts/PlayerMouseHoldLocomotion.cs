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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            var control = GetComponent<BasicPeepAnimController>();
            if (!Physics.Raycast(ray, out hit))
            {
                if (control != null) 
                    control.StopPlayer();

                return;
            }
            if(control != null)
                control.MovePlayer(hit.point);
        }
    }
}
