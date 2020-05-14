using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyTrap : MonoBehaviour
{
    //public GameManager gameManager;
    public Level levelThatOwnsMe;

    private void OnCollisionEnter(Collision hit)
    {
        //Debug.Log("entered");
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
}
