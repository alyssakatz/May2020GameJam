using UnityEngine;

public abstract class BlockSystemGenerator : MonoBehaviour
{
    public abstract Block[,,] GenerateBlocks();
}
