using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public static BlockSystem PlayerSystem;
    public static BlockSystem EnemySystem;

    public bool PlayerControlled;
    public const int BlockSize = 1;
    
    Block[,,] blocks;
    [SerializeField]
    BlockSystemGenerator blockGenerator;
    
    public Int3 GetDimensions()
    {
        return new Int3(blocks.GetLength(0), blocks.GetLength(1), blocks.GetLength(2));
    }
    public Vector3 GetBlockWWorldPosition(Int3 location)
    {
        return GetBlockWorldPosition(location.x, location.y, location.z);
    }
    public Vector3 GetBlockWorldPosition(int x, int y, int z)
    {
        return transform.position + new Vector3(x * BlockSize, y * BlockSize, z * BlockSize);
    }

    public Block GetBlockByWorldPosition(int x, int y, int z)
    {
        return GetBlockByWorldPosition(new Vector3(x, y, z));
    }

    public Block GetBlockByWorldPosition(Vector3 worldPosition)
    {
        // Make sure to start above the top of the tallest block
        // Raycast will ignore colliders that overlap its origin
        worldPosition.y = transform.position.y + blocks.GetLength(2) * BlockSize;

        int blocksLayerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(worldPosition, Vector3.down, out hit, Mathf.Infinity, blocksLayerMask))
        {
            return hit.transform.GetComponent<Block>();
        }
        return null;
    }

    public Int3? GetHighestSolidLocation(int x, int z)
    {
        for(int y = blocks.GetLength(2) - 1; y >= 0; y--)
        {
            if (blocks[x, y, z]) return new Int3(x, y, z);
        }
        return null;
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

    void Start()
    {
        if (PlayerControlled)
        {
            PlayerSystem = this;
        }else
        {
            EnemySystem = this;
        }

        blocks = blockGenerator.GenerateBlocks(this);
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                for (int z = 0; z < blocks.GetLength(2); z++)
                {
                    blocks[x,y,z].name = $"{this.gameObject.name}: {blocks[x,y,z].name} {x} {y} {z}";
                    blocks[x, y, z].transform.parent = this.transform;
                    blocks[x, y, z].transform.position = GetBlockWorldPosition(x, y, z);
                }
            }
        }
    }
}
