using System.Collections.Generic;
using UnityEngine;


//In this class, the map has been created.
//You have to edit GetRelativeBlock section to calculate current relative block to cast player rope to hold on
//Update Block Position section to make infinite map.
public class BlockCreator : MonoBehaviour {

    private static BlockCreator singleton = null;
    private GameObject[] blockPrefabs;
    private GameObject pointPrefab;
    private GameObject pointObject;
    public int blockCount;

    private List<GameObject> blockPool = new List<GameObject>();
    private float lastHeightUpperBlock = 10;
    private int difficulty = 1;

    public static BlockCreator GetSingleton()
    {
        if(singleton == null)
        {
            singleton = new GameObject("_BlockCreator").AddComponent<BlockCreator>();
        }
        return singleton;
    }

    public void Initialize(int bCount, GameObject[] bPrefabs, GameObject pPrefab)
    {
        blockCount = bCount;
        blockPrefabs = bPrefabs;
        pointPrefab = pPrefab;
        InstantiateBlocks();
    }
    
	
    public void InstantiateBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            lastHeightUpperBlock = Random.Range(lastHeightUpperBlock - difficulty, lastHeightUpperBlock + difficulty);
            float randomHeightLowerBlock = Random.Range(lastHeightUpperBlock - 20, lastHeightUpperBlock - 20 + difficulty * 3);
            GameObject newUpperBlock = Instantiate(blockPrefabs[i % blockPrefabs.Length], new Vector3(0, lastHeightUpperBlock, i + 1), Quaternion.identity);
            GameObject newLowerBlock = Instantiate(blockPrefabs[i % blockPrefabs.Length], new Vector3(0, randomHeightLowerBlock, i + 1), Quaternion.identity);
            blockPool.Add(newUpperBlock);
            blockPool.Add(newLowerBlock);
            if(i==15)
            {
                pointObject = Instantiate(pointPrefab, new Vector3(0, (lastHeightUpperBlock + randomHeightLowerBlock) / 2f, i + 1), Quaternion.Euler(90,0,0));
            }
        }
    }
	void Update () {
		
	}

    public int Difficulty
    {
        get
        {
            return difficulty;
        }

        set
        {
            difficulty = value;
        }
    }

    public Transform GetRelativeBlock(float playerPosZ)
    {
        //You may need this type of getter to which block are we going to cast our rope into
        return null;
    }

    public void UpdateBlockPosition(int blockIndex)
    {
        //Block Pool has been created. Find a proper way to make infite map when it is needed
    }
}
