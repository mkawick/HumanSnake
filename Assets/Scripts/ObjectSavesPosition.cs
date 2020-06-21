using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSavesPosition : MonoBehaviour
{
    Vector3 position;
    Quaternion rotation;
    // Start is called before the first frame update
    void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;
    }
    public void Reset()
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
