using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{


    public static LevelManager sharedInstance;
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();
    public Transform levelStartPosition;
    public float levelBlockOffset = 3f;

    // Awake is called before the first frame
    void Awake() 
    {
        if (sharedInstance == null) {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add new level block
    public void AddLevelBlock(){
            
        // random block index, new level block, spawn postition
        int randomIdx = Random.Range(0, allTheLevelBlocks.Count - 1);
        LevelBlock block;
        Vector3 spawnPosition = Vector3.zero;

        if (currentLevelBlocks.Count == 0) {
            // create first Level Block
            block = Instantiate(allTheLevelBlocks[0]);
            spawnPosition = levelStartPosition.position;
        } else {
            // Creating random block 
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            // retriving end position of the last generated level block
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count -1].endPoint.position;
        }

        // Set level manager as parent 
        block.transform.SetParent(this.transform, false);

        Vector3 correction = new Vector3 (
            spawnPosition.x - block.startPoint.position.x,
            (spawnPosition.y - block.startPoint.position.y) + ((currentLevelBlocks.Count == 0)?2:levelBlockOffset),
            0
        );

        block.transform.position = correction;

        currentLevelBlocks.Add(block);
        

    }
    
    // Remove level block
    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    // Remove all level blocks
    public void RemoveAllLevelBlock() 
    {
        while(currentLevelBlocks.Count>0){
            RemoveLevelBlock();
        }
    }

    // Generate the initial blocks for start a new game
    public void GenerateInitialBlocks() 
    {
        for (int i=0 ; i<2 ; i++ ){
            AddLevelBlock();
        }

    }    
}
