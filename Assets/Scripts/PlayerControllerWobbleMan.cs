using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerControllerWobbleMan : MonoBehaviour
{
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            var control = GetComponent<RigidBodyTest>();
            if (!Physics.Raycast(ray, out hit))
            {
                if (control != null)
                    control.StopPlayer();

                return;
            }
            if (control != null)
                control.MovePlayer(hit.point);
        }
    }
}