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
    public Vector3 GetBlockPosition(Int3 location)
    {
        return GetBlockPosition(location.x, location.y, location.z);
    }
    public Vector3 GetBlockPosition(int x, int y, int z)
    {
        return transform.position + new Vector3(x * BlockSize, y * BlockSize, z * BlockSize);
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

        blocks = blockGenerator.GenerateBlocks();
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                for (int z = 0; z < blocks.GetLength(2); z++)
                {
                    blocks[x,y,z].name = $"{this.gameObject.name}: {blocks[x,y,z].name} {x} {y} {z}";
                    blocks[x, y, z].transform.parent = this.transform;
                    blocks[x, y, z].transform.position = GetBlockPosition(x, y, z);
                }
            }
        }
    }
}
