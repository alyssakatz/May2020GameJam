using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
