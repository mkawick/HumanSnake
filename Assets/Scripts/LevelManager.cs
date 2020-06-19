using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Linq;

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
    [SerializeField]
    bool levelsLoop = false;
    // Start is called before the first frame update
    GameObject celebrationSet;

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
        celebrationSet = Utils.GetChildWithName(this.gameObject, "HappyEndingSet");
        Debug.Assert(celebrationSet != null, "the happyendingset must be placed under levels for the game to work");
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

        levels[currentLevel].gameObject.SetActive(true);

        levels[currentLevel].ResetDoors();
        levels[currentLevel].ResetObjects();
        levels[currentLevel].SetupPlayerStart(player.GetComponent<JoelAnimator>());
        
        levels[currentLevel].SetPeepManager(peepManager);

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
    public void ResetLevel()
    {
        levelState = LevelState.Start;
        levels[currentLevel].SetupPlayerStart(player.GetComponent<JoelAnimator>());
    }

    void FinishLevel()
    {
        GameObject celebrationCameraSpot = Utils.GetChildWithName(celebrationSet, "EndOfSceneCameraSpot");
        Debug.Assert(celebrationSet != null, "missing camera spot");
        // lock level
        timeWhenICanTransition = Time.time + 5;
        
        // celebration
        levelState = LevelState.Preload;
        gameManager.PlayEnd(celebrationCameraSpot);

        GameObject celebrationDancingSpot = Utils.GetChildWithName(celebrationSet, "DancingSpots");
        var spots = GrabSpots(celebrationDancingSpot);
        Debug.Assert(spots.Count != 0, "missing dancing spots");

        GameObject joelDancingSpot = Utils.GetChildWithName(celebrationSet, "JoelDancingSpot");
        Debug.Assert(joelDancingSpot != null, "missing joel dancing spot");

        peepManager.MakeEveryoneDance(
            spots, 
            levels[currentLevel].GetTrappedPeople(), 
            player.GetComponent<JoelAnimator>(),
            joelDancingSpot.transform,
            timeWhenICanTransition);
    }

    List<Transform> GrabSpots(GameObject parent)
    {
        List<Transform> spots = new List<Transform>();
        foreach (Transform t in parent.GetComponentsInChildren<Transform>())
        {
            if (t != parent.gameObject.transform)
            {
                spots.Add(t);
            }
        }
        return spots;
    }

    void FinishLevelTransitionToNewLevel()
    {
        if (timeWhenICanTransition < Time.time)
        {
            if(enableTransitionToNewLevels == true)
                currentLevel++;
            if (currentLevel >= levels.Length)
            {
                if (levelsLoop == true)
                    currentLevel = 0;
                else
                    currentLevel = levels.Length - 1;
            }
            
            levelState = LevelState.Start;
            peepManager.CleanupFromDancing();
        }
    }
}
