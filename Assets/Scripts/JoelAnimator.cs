using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JoelAnimator : BasicPeepAnimController
{
    //Animation animation;
    protected override void GrabAnimator()
    {
        //animation = GetComponent<Animation>();
    }

    internal override void PlayAnim(AnimationPlay clip)
    {
        switch (clip)
        {
            case AnimationPlay.Walk:
                GetComponent<Animation>().CrossFade("Walk");
                break;
            case AnimationPlay.Hit:
                GetComponent<Animation>().CrossFade("Hit");
                break;
            case AnimationPlay.Attack:
                GetComponent<Animation>().CrossFade("Attack");
                break;
            case AnimationPlay.Jump:
                GetComponent<Animation>().CrossFade("JumpA");
                break;
            // missing a few
            case AnimationPlay.Wave:
                GetComponent<Animation>().CrossFade("Idle");
                break;
            case AnimationPlay.Run:
                GetComponent<Animation>().CrossFade("Run");
                break;
            case AnimationPlay.Idle:
                GetComponent<Animation>().CrossFade("Idle");
                break;
            case AnimationPlay.Dodge:
                GetComponent<Animation>().CrossFade("Dodge");
                break;
            case AnimationPlay.Damage:
                GetComponent<Animation>().CrossFade("Damage");
                break;
            case AnimationPlay.Death:
                GetComponent<Animation>().CrossFade("Death");
                break;
        }
    }

    protected override void ConductTests()
    {
        RunTests();
    }
    void RunTests()
    {
        if (Input.GetKey(KeyCode.R))
        {
            GetComponent<Animation>().CrossFade("Run");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Animation>().CrossFade("Walk");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Animation>().CrossFade("Idle");
        }
        else if (Input.GetKey(KeyCode.V))
        {
            GetComponent<Animation>().CrossFade("Idle");
        }
     /*   else if(Input.GetMouseButton((int)MouseButton.LeftMouse))
        {

        }*/
    }
}
