﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerMouseHoldLocomotion : MonoBehaviour
{
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
            var control = GetComponent<ThirdPersonCharacter>();
            control.Move(direction, false, false);
        }
    }
}
