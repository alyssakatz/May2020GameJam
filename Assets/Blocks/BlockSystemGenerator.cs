using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlockSystem;

public abstract class BlockSystemGenerator : MonoBehaviour
{
    public abstract Block[,,] GenerateBlocks();
}
