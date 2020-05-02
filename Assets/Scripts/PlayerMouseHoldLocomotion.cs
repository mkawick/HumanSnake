using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerMouseHoldLocomotion : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }

            Vector3 direction = (hit.point - this.transform.position).normalized;
            //Vector3 moveDir = v * Vector3.forward + h * Vector3.right;

            var control = GetComponent<ThirdPersonCharacter>();
            control.Move(direction, false, false);
        }
    }

    internal void ShowTarget(bool show)
    {
        return;
        if (show == true)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }

}
