using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubesParent;
    public GameObject stonesParent;
    public float stoneYOffset;

    public GameObject[] itemsToPickFrom;
    public GameObject[] stonesToPickFrom;
    public int gridX;
    public int gridZ;
    public float gridSpacingOffset = 1;
    public float stoneSpawnPercent = 10;
    public float whirlSpawnPercent = 5;
    public Vector3 gridOrigin = Vector3.zero;

    int[] randomFixedRotations = new int[4] {0, 90, 180, 270};

    void Start()
    {
        SpawnGrid();
        CheckStoneGrid();
    }

    void SpawnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                PickAndSpawn(spawnPos, Quaternion.identity);
            }
        }
    }

    void CheckStoneGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                RaycastHit hit;

                if(Physics.Raycast(new Vector3(x, 2, z), Vector3.down, out hit))
                {
                    if(hit.collider.gameObject.tag == "Rock") //potentially make layer mask
                    {
                        //Debug.Log($"Rock position: {x},{z}");
                    }
                }
            }
        }
    }

    void PickAndSpawn(Vector3 posToSpawn, Quaternion rotationToSpawn)
    {
        int spawnRotation = Random.Range(0, randomFixedRotations.Length);
        int randomPercent = Random.Range(0, 100);
        int randomIndex = Random.Range(0, itemsToPickFrom.Length);
        int randomStoneIndex = Random.Range(0, stonesToPickFrom.Length);

        if(randomPercent >= stoneSpawnPercent)
        {
            //GameObject clone = Instantiate(itemsToPickFrom[0], posToSpawn, rotationToSpawn);
            //clone.transform.parent = cubesParent.transform; 
        }
        else // instantiate stone
        {
            //GameObject clone = Instantiate(itemsToPickFrom[1], posToSpawn, rotationToSpawn);
            GameObject stoneClone = Instantiate(stonesToPickFrom[randomStoneIndex], posToSpawn + new Vector3(0, stoneYOffset, 0), Quaternion.Euler(0, randomFixedRotations[spawnRotation], 0));

            //clone.transform.parent = cubesParent.transform; 
            stoneClone.transform.parent = stonesParent.transform;
        }

        /*if(randomPercent < stoneSpawnPercent && randomPercent > whirlSpawnPercent)
        {
            GameObject clone = Instantiate(itemsToPickFrom[2], posToSpawn, rotationToSpawn);
        }
        
        if(randomPercent >= stoneSpawnPercent + whirlSpawnPercent)
        {
            GameObject clone = Instantiate(itemsToPickFrom[0], posToSpawn, rotationToSpawn);
        }*/
    }
}
