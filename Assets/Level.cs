using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour
{
    public Transform playerStartPosition;
    public Transform exitLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupPlayerStart(GameObject go)
    {
        go.transform.position = playerStartPosition.position;
    }

    public List<TrappedPerson> GetTrappedPeople()
    {
        var listOfPeeps = GetComponentsInChildren<TrappedPerson>();

        return listOfPeeps.OfType<TrappedPerson>().ToList();
    }

    public bool IsLevelComplete()
    {
        var listOfPeeps = GetTrappedPeople();
        foreach (var peep in listOfPeeps)
        {
            if (peep.currentState != TrappedPerson.State.EndOfLevel)
            {
                return false;
            }
        }
        return true;
    }
}
