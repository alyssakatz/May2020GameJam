using UnityEngine;

public class SolidBlockSystemGenerator : BlockSystemGenerator
{
    public GameObject BlockPrefab;

    public int Size;
    
    public override Block[,,] GenerateBlocks(BlockSystem blockSystem)
    {
        Block[,,] blocks = new Block[Size, Size, Size];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int z = 0; z < Size; z++)
                {
                    Block block = Instantiate(BlockPrefab).GetComponent<Block>();
                    block.Location = new Int3(x, y, z);
                    block.BlockSystem = blockSystem;
                    blocks[x, y, z] = block;
                }
            }
        }
        return blocks;
    }
}
