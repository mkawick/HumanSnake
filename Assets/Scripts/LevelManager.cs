using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public NavMeshSurface navMesh;
    public Level[] levels;
    public GameObject player;
    int currentLevel = 0;
    public PeepManager peepManager;
    // Start is called before the first frame update
    void Start()
    {
        StartNewLevel();// todo, move
    }

    // Update is called once per frame
    void Update()
    {
        //levels[currentLevel].SetupPlayerStart(player);
    }

    public Level GetCurrentLevel()
    {
        return levels[currentLevel];
    }

    void StartNewLevel()
    {
        //currentLevel = 0;
        for(int i= 0; i<levels.Length; i++)
        {
            if (i == currentLevel)
                continue;

            levels[i].gameObject.SetActive(false);

        }
        levels[currentLevel].gameObject.SetActive(true);
        levels[currentLevel].SetupPlayerStart(player);
        peepManager.NewLevel(levels[currentLevel].GetTrappedPeople());
        peepManager.exitLocation = levels[currentLevel].exitLocation;

        navMesh.BuildNavMesh();
    }

    void FinishLevel()
    {
        currentLevel++;
        // celebration
    }
}
