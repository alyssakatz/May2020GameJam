using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlockSystem;

public class Block : MonoBehaviour
{
    public BlockSystem LocalBlockSystem;
    public Int3 LocalLocation;

    public static BlockSystem AlignedBlockSystem;
    public Int3 AlignedLocation;

    public void Start()
    {
        AlignedBlockSystem = BlockSystemManager.Instance.AlignedSystem;
    }

    public void Initialize(BlockSystem localBlockSystem, Int3 localLocation)
    {
        LocalBlockSystem = localBlockSystem;
        LocalLocation = localLocation;
        name = $"{LocalBlockSystem.gameObject.name}: {name} {LocalLocation.x} {LocalLocation.y} {LocalLocation.z}";
        transform.parent = LocalBlockSystem.transform;
        transform.position = LocalBlockSystem.GetBlockWorldPositionByLocation(LocalLocation);
    }

    public Block Right(int numBlocks = 1)
    {
        if (AlignedLocation.x + numBlocks < BlockSystemManager.Instance.AlignedSystem.GetDimensions().x)
            return AlignedBlockSystem.GetBlockByLocation(AlignedLocation.x + numBlocks, AlignedLocation.y, AlignedLocation.z);
        else return null;
    }

    public Block Left(int numBlocks = 1)
    {
        return Right(-numBlocks);
    }

    public Block Up(int numBlocks = 1)
    {
        if (AlignedLocation.y + numBlocks < BlockSystemManager.Instance.AlignedSystem.GetDimensions().y)
            return AlignedBlockSystem.GetBlockByLocation(AlignedLocation.x, AlignedLocation.y + numBlocks, AlignedLocation.z);
        else return null;
    }

    public Block Down(int numBlocks = 1)
    {
        return Up(-numBlocks);
    }
    public Block Forward(int numBlocks = 1)
    {
        if (AlignedLocation.z + numBlocks < BlockSystemManager.Instance.AlignedSystem.GetDimensions().z)
            return AlignedBlockSystem.GetBlockByLocation(AlignedLocation.x, AlignedLocation.y, AlignedLocation.z + numBlocks);
        else return null;
    }

    public Block Backward(int numBlocks = 1)
    {
        return Backward(-numBlocks);
    }
}
