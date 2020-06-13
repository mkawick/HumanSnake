using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoelAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Animation>().CrossFade("Walk");
        GetComponent<Animation>().Play("Walk");
        //GetComponent<Animation>().Play("Walk", AnimationPlayMode.Queue);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                GetComponent<Animation>().CrossFade("Run");
            else
                GetComponent<Animation>().CrossFade("Walk");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                GetComponent<Animation>().CrossFade("JumpA");
            else
                GetComponent<Animation>().CrossFade("JumpB");
        }
        if (!GetComponent<Animation>().isPlaying || Input.GetKeyDown(KeyCode.I))
            GetComponent<Animation>().CrossFade("Idle");
    }
}
