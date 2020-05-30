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

    Dictionary<Int3, Block> blocks;
    [SerializeField]
    BlockSystemGenerator blockGenerator;

    Block GetBlock(int x, int y, int z)
    {
        return blocks[new Int3(x, y, z)];
    }

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
        foreach(Int3 position in blocks.Keys)
        {
            Block block = blocks[position];
            block.gameObject.transform.position = GetBlockPosition(position);
        }
    }
}
