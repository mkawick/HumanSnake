using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class FlamingWall : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] psflames = null;
    [SerializeField, Range(0.21f, 3f)]
    float spawnDensity = 1;
    float markerRadius = 0.15f;

    List<ParticleSystem> spawnedFlames;
    void Start()
    {
        if (!Application.isEditor)
        {
            markerRadius = 0;
            
        }
        SpawnAllOfTheEffects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnValidate()// in editor
    {
        //Field = field;
    }

    void DeleteSpawnedFlames()
    {
        if (spawnedFlames != null)
        {
            foreach (var flame in spawnedFlames)
            {
                Destroy(flame);
            }
        }
        spawnedFlames = new List<ParticleSystem>();

    }

    int GetRandomNumberOfEffects()
    {
        float value = UnityEngine.Random.value;
        if (value < 0.6f)
            return 1;
        if (value < 0.9f)
            return 2;
        if (value < 0.95f)
            return 3;
        return 0;
    }
    void SpawnAllOfTheEffects()
    {
        var spots = CalcSpots();
        DeleteSpawnedFlames();

        int numOptions = psflames.Length;
        if (numOptions == 0)
            return;

        foreach (var spot in spots)
        {
            int numEffects = GetRandomNumberOfEffects();
            for (int i = 0; i < numEffects; i++)
            {
                int which = UnityEngine.Random.Range(0, numOptions);
                Vector3 offset = UnityEngine.Random.insideUnitSphere * 0.02f;
                var newObstacle = Instantiate<ParticleSystem>(psflames[which], spot+offset , psflames[which].transform.rotation);
                spawnedFlames.Add(newObstacle);
                newObstacle.transform.parent = this.transform;
                newObstacle.gameObject.SetActive(true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var spots = CalcSpots();
        Gizmos.color = new Color(1.0f, 0.8f, 0) ;

        foreach(var spot in spots)
        {
            Gizmos.DrawSphere(spot, markerRadius);
        }
    }
    List<Vector3> CalcSpots()
    {
        Vector3  dim = transform.localScale;
        float radX = dim.x;
        float radZ = dim.z;
        float longest = Math.Max(radX, radZ);

        float numSpawnsPer = spawnDensity;
        float numfires = longest * numSpawnsPer + 1; // 1 for center

        List<Vector3> spots = new List<Vector3>();
        Vector3 center = transform.position;
        float verticalOffset = dim.y / 2 + markerRadius;
        center.y += verticalOffset;

        Vector3 startPos, endPos;

        if (radX < radZ)
        {
            Vector3 forwardFace = transform.forward *0.95f;
            startPos = transform.position + (forwardFace * radZ/2);
            endPos = transform.position - (forwardFace * radZ/2);
        }
        else
        {
            Vector3 rightFace = -transform.right * 0.95f;
            startPos = transform.position + (rightFace * radX / 2);
            endPos = transform.position - (rightFace * radX / 2);
        }

        Vector3 totalLength = endPos - startPos;

        float distanceBetweenEach = totalLength.magnitude / numfires;
        Vector3 diff = totalLength.normalized;
        startPos.y += verticalOffset;
        endPos.y += verticalOffset;
        diff *= distanceBetweenEach;
        for (int i = 0; i <= numfires; i++)
        {
            spots.Add(startPos);
            startPos += diff;
        }

        return spots;
    }
}
/*
 * public void ChunkAdded(GameObject chunk, bool isFirst = false)
    {
        if (shouldGenerateObstacles == false || isFirst == true)
            return;

        Vector3 scale = chunk.GetComponent<MeshCollider>().bounds.size;
        NewChunkWasGenerated(chunk.transform, scale.x / 2, scale.z / 2);
    }
    void NewChunkWasGenerated(Transform chunkTransform, float boundsX, float boundsZ)
    {
        float rangeMin = currentDifficulty * 3;
        float rangeMax = currentDifficulty * 5;

        if (rangeMax > 100)
            rangeMax = 100;
        float numObstaclesToGenerate = UnityEngine.Random.Range(rangeMin, rangeMax);
        var center = chunkTransform.position;

        float margin = 0.5f;
       // float positionMinX = center.x - boundsX + margin;
      //  float positionMaxX = center.x + boundsX - margin;
float positionMinZ = center.z - boundsZ;
float positionMaxZ = center.z + boundsZ;
GameObject container = GetChildWithName(chunkTransform.gameObject, "ObjectsContainer");

float oneTenth = (positionMaxZ - positionMinZ) / 10;
float startingPosition = positionMinZ + oneTenth;
        for (int i = 0; i<numObstaclesToGenerate; i++)
        {
            int whichReward = i % rewardPrefabs.Length;
int whichObstacle = i % obstaclePrefabs.Length;
float angle = UnityEngine.Random.Range(0, 85);
Quaternion q = rewardPrefabs[whichReward].transform.rotation;
q *= Quaternion.Euler(Vector3.up* angle);

            float x = 0;// UnityEngine.Random.Range(positionMinX, positionMaxX);
float z = UnityEngine.Random.Range(positionMinZ, positionMaxZ);


CreateObstacle(whichReward, whichObstacle, container.transform, x, z, q);
        }
    }

    GameObject CreateObstacle(int whichReward, int whichObstacle, Transform parent, float x, float z, Quaternion rotation)
{
    //Vector3 scale = obstaclePrefabs[whichObstacle].transform.localScale;
    Vector3 obstacleDimensions = obstaclePrefabs[whichObstacle].GetComponent<Renderer>().bounds.size;
    Vector3 pos2 = new Vector3(x, obstacleDimensions.y / 2, z);
    GameObject newObstacle = Instantiate(obstaclePrefabs[whichObstacle], pos2, rotation);


    //Vector3 rewardSCale = rewardPrefabs[whichReward].transform.localScale;
    Vector3 rewardDimensions = rewardPrefabs[whichReward].GetComponent<BoxCollider>().bounds.size;
    Vector3 rewardPos = new Vector3(x, obstacleDimensions.y + rewardDimensions.y / 2, z);

    GameObject newReward = Instantiate(rewardPrefabs[whichReward], rewardPos, rotation);
    var prize = newReward.GetComponent<CollectablePrize>();
    if (prize != null)
    {
        prize.chestManager = this;
    }


    newObstacle.SetActive(true);
    newReward.SetActive(true);

    newObstacle.transform.parent = parent;
    newReward.transform.parent = parent;

    return newObstacle;
}
 * */
