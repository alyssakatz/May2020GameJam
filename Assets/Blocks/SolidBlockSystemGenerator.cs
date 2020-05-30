using UnityEngine;

public class SolidBlockSystemGenerator : BlockSystemGenerator
{
    public GameObject BlockPrefab;

    public int Size;
    
    public override Block[,,] GenerateBlocks(BlockSystem localBlockSystem)
    {
        Block[,,] blocks = new Block[Size, Size, Size];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int z = 0; z < Size; z++)
                {
                    Block block = Instantiate(BlockPrefab).GetComponent<Block>();
                    block.Initialize(localBlockSystem, new Int3(x, y, z));
                    blocks[x, y, z] = block;
                }
            }
        }
        return blocks;
    }
}
