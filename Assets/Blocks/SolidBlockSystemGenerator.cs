using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlockSystem;
public class SolidBlockSystemGenerator : BlockSystemGenerator
{
    public GameObject BlockPrefab;

    public int Size;
    
    public override Block[,,] GenerateBlocks()
    {
        Block[,,] blocks = new Block[Size, Size, Size];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int z = 0; z < Size; z++)
                {
                    GameObject block = Instantiate(BlockPrefab);
                    blocks[x, y, z] = block.GetComponent<Block>();
                }
            }
        }
        return blocks;
    }
}
