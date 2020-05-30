using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlockSystem;
public class SolidBlockSystemGenerator : BlockSystemGenerator
{
    public GameObject BlockPrefab;

    public int Size;
    
    public override Dictionary<Int3, Block> GenerateBlocks()
    {
        Dictionary<Int3, Block> blocks = new Dictionary<Int3, Block>();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int z = 0; z < Size; z++)
                {
                    //Note - this seems dubious to need to remember to do this in every generator.
                    GameObject block = Instantiate(BlockPrefab);
                    blocks.Add(new Int3(x, y, z), block.GetComponent<Block>());
                    block.name = $"Stone Block {x} {y} {z}";
                }
            }
        }
        return blocks;
    }
}
