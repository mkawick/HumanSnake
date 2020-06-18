using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyTrap : MonoBehaviour
{
    //public GameManager gameManager;
    public Level levelThatOwnsMe;
    public GameObject trapCircle;

    [SerializeField]
    bool showMesh = true;

    private void Start()
    {
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = showMesh;
        }
        foreach(var mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = showMesh;
        }
    }
    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log("entered");
        var person = hit.gameObject.GetComponent<TrappedPerson2>();
        if(levelThatOwnsMe != null && person != null)
        {
            levelThatOwnsMe.PersonDied(person);
        }
        var circlePerson = hit.gameObject.GetComponent<RunPersonInCircle>();
        if (levelThatOwnsMe != null && circlePerson != null)
        {
            levelThatOwnsMe.PersonDied(circlePerson);
        }
    }

    internal void SetLevel(Level level)
    {
        levelThatOwnsMe = level;
    }

    private void Update()
    {
        UpdateTrapCircle();
    }
    void UpdateTrapCircle()
    {
        if (trapCircle == null)
            return;

        Vector3 position = this.transform.position;
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(position, Vector3.down, out hitInfo))
        {
            Vector3 hit = hitInfo.point;
            hit.y += 0.1f;
            trapCircle.transform.position = hit;

            trapCircle.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
        }
        
    }
}
