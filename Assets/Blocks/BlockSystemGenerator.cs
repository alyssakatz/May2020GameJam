using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BlockSystemGenerator : MonoBehaviour
{
    public abstract Block[,,] GenerateBlocks();
}
