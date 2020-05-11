using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class LevelManager : MonoBehaviour
{
   // public NavMeshSurface navMesh;
    public Level[] levels;
    public GameObject player;
    int currentLevel = 0;
    public PeepManager peepManager;
    public GameManager gameManager;

    float timeWhenICanTransition = 0;
    public bool enableTransitionToNewLevels = true;
    // Start is called before the first frame update

    enum LevelState
    {
        Preload,
        Start,
        Gameplay,
        EndLevel
    }
    LevelState levelState = LevelState.Start;
    void Start()
    {
        peepManager.levelManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        //levels[currentLevel].SetupPlayerStart(player);
        switch(levelState)
        {
            case LevelState.Preload:
                FinishLevelTransitionToNewLevel();
                break;
            case LevelState.Start:
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

        levels[currentLevel].ResetDoors();
        levels[currentLevel].SetupPlayerStart(player.GetComponent<RigidBodyTest>());
        levels[currentLevel].gameObject.SetActive(true);
        levels[currentLevel].SetPeepManager(peepManager);
        // navMesh.BuildNavMesh();

        peepManager.NewLevel(levels[currentLevel].GetTrappedPeople());
        peepManager.exitLocation = levels[currentLevel].exitLocation;

        gameManager.StartNewLevel();
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
        // lock level
        timeWhenICanTransition = Time.time + 5;
        
        // celebration
        levelState = LevelState.Preload;
        gameManager.PlayEnd();
    }

    void FinishLevelTransitionToNewLevel()
    {
        if (timeWhenICanTransition < Time.time)
        {
            if(enableTransitionToNewLevels == true)
                currentLevel++;
            if (currentLevel >= levels.Length)
                currentLevel = levels.Length - 1;
            levelState = LevelState.Start;
        }
    }
}
