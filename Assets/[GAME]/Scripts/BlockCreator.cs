using System.Collections.Generic;
using UnityEngine;


//In this class, the map has been created.
//You have to edit GetRelativeBlock section to calculate current relative block to cast player rope to hold on
//Update Block Position section to make infinite map.
public class BlockCreator : Singleton<BlockCreator> 
{
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int blockCount;

    private List<GameObject> blockPool = new List<GameObject>();
    private float _lastHeightUpperBlock = 10;
    private float _lastBlockZPos = 0;
    private int _lastChangedBlockIndex = 0;
    private int _difficulty = 1;
    public int Difficulty { get { return _difficulty; } set { _difficulty = value; } }

    private void Start()
    {
        InstantiateBlocks();
    }

    public void InstantiateBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            _lastHeightUpperBlock = Random.Range(_lastHeightUpperBlock - Difficulty, _lastHeightUpperBlock + Difficulty);
            float randomHeightLowerBlock = Random.Range(_lastHeightUpperBlock - 20, _lastHeightUpperBlock - 20 + Difficulty * 3);
            GameObject newUpperBlock = Instantiate(blockPrefabs[i % blockPrefabs.Length], new Vector3(0, _lastHeightUpperBlock, i + 1), Quaternion.identity);
            GameObject newLowerBlock = Instantiate(blockPrefabs[i % blockPrefabs.Length], new Vector3(0, randomHeightLowerBlock, i + 1), Quaternion.identity);
            blockPool.Add(newUpperBlock);
            blockPool.Add(newLowerBlock);
            if (i % 15 == 0 && i != 0)
            {
                Instantiate(pointPrefab, new Vector3(0, (_lastHeightUpperBlock + randomHeightLowerBlock) / 2f, i + 1), Quaternion.Euler(90, 0, 0));
            }
            _lastBlockZPos++;
        }
    }

    void Update () 
    {
        if(GameManager.Instance.isGameStarted)
            UpdateBlockPosition();
    }

    public Transform GetRelativeBlock(float playerPosZ)
    {
        //You may need this type of getter to which block are we going to cast our rope into
        Transform relativeBlock = null;
        for (int i = 0; i < blockPool.Count; i += 2)
        {
            if (blockPool[i].transform.position.z <= playerPosZ + 4 && blockPool[i].transform.position.z >= playerPosZ + 3)
                relativeBlock = blockPool[i].transform;
        }
        Debug.Log("RelativeBlock: " + relativeBlock.position);
        return relativeBlock;
    }

    public void UpdateBlockPosition()
    {
        //Block Pool has been created. Find a proper way to make infite map when it is needed
        if (_lastBlockZPos - CharacterManager.Instance.Player.transform.position.z >= 15)
            return;
        _lastHeightUpperBlock = Random.Range(_lastHeightUpperBlock - Difficulty, _lastHeightUpperBlock + Difficulty);
        float randomHeightLowerBlock = Random.Range(_lastHeightUpperBlock - 20, _lastHeightUpperBlock - 20 + Difficulty * 3);
        blockPool[_lastChangedBlockIndex++].transform.position = new Vector3(0, _lastHeightUpperBlock, _lastBlockZPos + 1);
        blockPool[_lastChangedBlockIndex++].transform.position = new Vector3(0, randomHeightLowerBlock, _lastBlockZPos + 1);
        if (_lastBlockZPos % 15 == 0)
        {
            Instantiate(pointPrefab, new Vector3(0, (_lastHeightUpperBlock + randomHeightLowerBlock) / 2f, _lastBlockZPos + 1), Quaternion.Euler(90, 0, 0));
        }
        _lastBlockZPos++;
        if (_lastChangedBlockIndex >= blockCount * 2)
            _lastChangedBlockIndex = 0;
    }
}
