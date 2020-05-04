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

    enum LevelState
    {
        Preload,
        Start,
        Gameplay,
        EndLevel
    }
    LevelState levelState = LevelState.Preload;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //levels[currentLevel].SetupPlayerStart(player);
        switch(levelState)
        {
            case LevelState.Preload:
                StartNewLevel();
                break;
            case LevelState.Gameplay:
                NormalGameplay();
                break;
            case LevelState.EndLevel:
                FinishLevel();
                break;

        }
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
        navMesh.BuildNavMesh();

        peepManager.NewLevel(levels[currentLevel].GetTrappedPeople());
        peepManager.exitLocation = levels[currentLevel].exitLocation;
        levels[currentLevel].SetupPlayerStart(player);

        levelState = LevelState.Gameplay;
    }

    void NormalGameplay()
    {
        if(levels[currentLevel].IsLevelComplete() == true)
        {
            levelState = LevelState.EndLevel;
        }
    }

    void FinishLevel()
    {
        currentLevel++;
        // celebration
    }
}
