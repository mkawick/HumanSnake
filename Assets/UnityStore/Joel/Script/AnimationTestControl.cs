using UnityEngine;
using System.Collections;

public class AnimationTestControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

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
        else if (Input.GetMouseButtonDown(0))
            GetComponent<Animation>().CrossFade("Dodge");
        else if (Input.GetMouseButtonDown(1))
            GetComponent<Animation>().CrossFade("Damage");
        else if (Input.GetMouseButtonDown(2))
            GetComponent<Animation>().CrossFade("Death");
        if (!GetComponent<Animation>().isPlaying)
            GetComponent<Animation>().CrossFade("Idle");
    }
}
