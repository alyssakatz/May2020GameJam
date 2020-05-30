using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public BlockSystemGenerator blockGenerator;

    public const int BlockSize = 1;

    public bool AreBlocksGenerated = false;
    
    Block[,,] blocks;
    
    public Int3 GetDimensions()
    {
        return new Int3(blocks.GetLength(0), blocks.GetLength(1), blocks.GetLength(2));
    }

    public Block GetBlockByLocation(Int3 location)
    {
        return GetBlockByLocation(location.x, location.y, location.z);
    }

    public Block GetBlockByLocation(int x, int y, int z)
    {
        return blocks[x, y, z];
    }

    public Vector3 GetBlockWorldPositionByLocation(Int3 location)
    {
        return GetBlockWorldPositionByLocation(location.x, location.y, location.z);
    }
    public Vector3 GetBlockWorldPositionByLocation(int x, int y, int z)
    {
        return transform.position + new Vector3(x * BlockSize, y * BlockSize, z * BlockSize);
    }

    public Block GetBlockLocationByWorldPosition(Vector3 worldPosition)
    {
        // Make sure to start above the top of the tallest block
        // Raycast will ignore colliders that overlap its origin
        worldPosition.y = transform.position.y + blocks.GetLength(1) * BlockSize;

        int blocksLayerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(worldPosition, Vector3.down, out hit, Mathf.Infinity, blocksLayerMask))
        {
            return hit.transform.GetComponent<Block>();
        }
        return null;
    }

    public Int3? GetHighestSolidLocation(Int3 location)
    {
        return GetHighestSolidLocation(location.x, location.z);
    }

    public Int3? GetHighestSolidLocation(int x, int z)
    {
        for(int y = blocks.GetLength(1) - 1; y >= 0; y--)
        {
            if (blocks[x, y, z]) return blocks[x, y, z].LocalLocation;
        }
        return null;
    }

    public Block GetHighestSolidBlock(Int3 location)
    {
        return GetHighestSolidBlock(location.x, location.z);
    }

    public Block GetHighestSolidBlock(int x, int z)
    {
        for (int y = blocks.GetLength(1) - 1; y >= 0; y--)
        {
            if (blocks[x, y, z]) return blocks[x, y, z];
        }
        return null;
    }

    public bool IsInBlockSystem(Int3 location)
    {
        return location.x >= 0 && location.y >= 0 && location.z >= 0 && location.x < blocks.GetLength(0) && location.y < blocks.GetLength(1) && location.z < blocks.GetLength(2);
    }

    public Int3 WorldToGridLocation(Vector3 worldLocation)
    {
        Vector3 distanceFromOrigin = worldLocation - this.transform.position;

        Vector3 blocksFromOrigin = distanceFromOrigin / BlockSize;

        Vector3 clampedBlocksFromOrigin = new Vector3(Mathf.Clamp(blocksFromOrigin.x, 0, blocks.GetLength(0)), Mathf.Clamp(blocksFromOrigin.y, 0, blocks.GetLength(1)), Mathf.Clamp(blocksFromOrigin.z, 0, blocks.GetLength(2)));

        return new Int3(Mathf.RoundToInt(clampedBlocksFromOrigin.x), Mathf.RoundToInt(clampedBlocksFromOrigin.y), Mathf.RoundToInt(clampedBlocksFromOrigin.z));
    }

    public void Break(Int3 location)
    {
        Break(location.x, location.y, location.z);
    }

    public void Break(int x, int y, int z)
    {
        Block block = blocks[x, y, z];
        blocks[x, y, z] = null;
        Destroy(block.gameObject);
    }

    public void Break(Block block)
    {
        blocks[block.LocalLocation.x, block.LocalLocation.y, block.LocalLocation.z] = null;
        Destroy(block.gameObject);
    }

    void Start()
    {
        blocks = blockGenerator.GenerateBlocks(this);
        AreBlocksGenerated = true;
    }
}
