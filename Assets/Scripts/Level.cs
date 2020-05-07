using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

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

    public void SetupPlayerStart(RigidBodyTest go)
    {
        Vector3 pos =( playerStartPosition.position);
        NavMeshAgent nma = go.GetComponent<NavMeshAgent>();
        nma.Warp( pos);
        //Debug.Log(go.gameObject.transform.position);
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
