using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyTrap : MonoBehaviour
{
    //public GameManager gameManager;
    public Level levelThatOwnsMe;

    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log("entered");
        var person = hit.gameObject.GetComponent<TrappedPerson2>();
        if(levelThatOwnsMe != null && person != null)
        {
            //gameManager.peepManager.IDied(person);
            levelThatOwnsMe.PersonDied(person);
        }
    }

    internal void SetLevel(Level level)
    {
        levelThatOwnsMe = level;
    }
}
