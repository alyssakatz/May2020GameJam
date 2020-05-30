using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public const int BlockSize = 1;
    public struct Int3
    {
        public readonly int x, y, z;
        
        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    Block[,,] blocks;
    [SerializeField]
    BlockSystemGenerator blockGenerator;

    public Vector3 GetBlockPosition(Int3 location)
    {
        return GetBlockPosition(location.x, location.y, location.z);
    }
    public Vector3 GetBlockPosition(int x, int y, int z)
    {
        return transform.position + new Vector3(x * BlockSize, y * BlockSize, z * BlockSize);
    }

    void Start()
    {
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
