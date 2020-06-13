using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoelAnimator : BasicPeepAnimController
{
    Animation animation;
    protected override void GrabAnimator()
    {
        animation = GetComponent<Animation>();
    }

    internal override void PlayAnim(AnimationPlay clip)
    {
        switch (clip)
        {
            case AnimationPlay.Walk:
                animation.CrossFade("Walk");
                break;
            case AnimationPlay.Hit:
                animation.CrossFade("Hit");
                break;
            case AnimationPlay.Attack:
                animation.CrossFade("Attack");
                break;
            case AnimationPlay.Jump:
                animation.CrossFade("JumpA");
                break;
            // missing a few
            case AnimationPlay.Wave:
                animation.CrossFade("Wave");
                break;
            case AnimationPlay.Run:
                animation.CrossFade("Run");
                break;
            case AnimationPlay.Idle:
                animation.CrossFade("Idle");
                break;
            case AnimationPlay.Dodge:
                animation.CrossFade("Dodge");
                break;
            case AnimationPlay.Damage:
                animation.CrossFade("Damage");
                break;
            case AnimationPlay.Death:
                animation.CrossFade("Death");
                break;
        }
    }
    /*
     * GetComponent<Animation>().CrossFade("Dodge");
        else if (Input.GetMouseButtonDown(1))
            GetComponent<Animation>().CrossFade("Damage");
        else if (Input.GetMouseButtonDown(2))
            GetComponent<Animation>().CrossFade("Death");
     * */

    void RunTests()
    {
        if (Input.GetKey(KeyCode.R))
        {
            animation.CrossFade("Run");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animation.CrossFade("Walk");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animation.CrossFade("Idle");
        }
        else if (Input.GetKey(KeyCode.V))
        {
            animation.CrossFade("Wave");
        }
    }
}
