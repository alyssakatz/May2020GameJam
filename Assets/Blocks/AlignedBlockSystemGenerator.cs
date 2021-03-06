﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;

public class AlignedBlockSystemGenerator : BlockSystemGenerator
{
    public enum Alignment
    {
        PlayerPositiveXToEnemyNegativeX,
        EnemyPositiveXToPlayerNegativeX,
        PlayerPositiveZToEnemyNegativeZ,
        EnemyPositiveZToPlayerNegativeZ
    }

    public Alignment AlignmentEdge = Alignment.PlayerPositiveXToEnemyNegativeX;

    public bool DrawDebugCubes = false;
    public GameObject DebugBlockPrefab;

    public override Block[,,] GenerateBlocks(BlockSystem AlignedBlockSystem)
    {
        Transform smallerTransform =  BlockSystemManager.Instance.PlayerSystem.GetDimensions().x < BlockSystemManager.Instance.EnemySystem.GetDimensions().x ? BlockSystemManager.Instance.PlayerSystem.transform : BlockSystemManager.Instance.EnemySystem.transform;
   
        // Positive vs negative is a bit more generic than left/right or up/down
        BlockSystem positiveSide = null;
        BlockSystem negativeSide = null;
        Int3 dimensions = new Int3(0, 0, 0);
        Int3 positiveDimensions = new Int3(0, 0, 0);
        Int3 negativeDimensions = new Int3(0, 0, 0);
        int positiveAlignmentOffset = 0;
        int negativeAlignmentOffset = 0;

        switch (AlignmentEdge)
        {
            case Alignment.PlayerPositiveXToEnemyNegativeX:
                positiveSide = BlockSystemManager.Instance.PlayerSystem;
                negativeSide = BlockSystemManager.Instance.EnemySystem;
                
                positiveDimensions = positiveSide.GetDimensions();
                negativeDimensions = negativeSide.GetDimensions();
                
                dimensions = new Int3(positiveDimensions.x + negativeDimensions.x,
                    Math.Max(positiveDimensions.y, negativeDimensions.y),
                    Math.Max(positiveDimensions.z, negativeDimensions.z));

                if (positiveDimensions.z < negativeDimensions.z)
                    positiveAlignmentOffset = Mathf.FloorToInt(negativeDimensions.z / 2) - Mathf.FloorToInt(positiveDimensions.z / 2);
                else if (positiveDimensions.z > negativeDimensions.z)
                    negativeAlignmentOffset = Mathf.FloorToInt(positiveDimensions.z / 2) - Mathf.FloorToInt(negativeDimensions.z / 2);

                if (DrawDebugCubes)
                {
                    smallerTransform.localPosition += new Vector3(-smallerTransform.GetComponent<BlockSystem>().GetDimensions().x, 0, 0);
                    transform.localPosition += new Vector3(-smallerTransform.GetComponent<BlockSystem>().GetDimensions().x, 0, 0);
                }
                break;

            case Alignment.PlayerPositiveZToEnemyNegativeZ:
                positiveSide = BlockSystemManager.Instance.PlayerSystem;
                negativeSide = BlockSystemManager.Instance.EnemySystem;
                
                positiveDimensions = positiveSide.GetDimensions();
                negativeDimensions = negativeSide.GetDimensions();
                
                dimensions = new Int3(Math.Max(positiveDimensions.x, negativeDimensions.x),
                    Math.Max(positiveDimensions.y, negativeDimensions.y),
                    positiveDimensions.z + negativeDimensions.z);

                if (positiveDimensions.x < negativeDimensions.x)
                    positiveAlignmentOffset = Mathf.FloorToInt(negativeDimensions.x / 2) - Mathf.FloorToInt(positiveDimensions.x / 2);
                else if (positiveDimensions.x > negativeDimensions.x)
                    negativeAlignmentOffset = Mathf.FloorToInt(positiveDimensions.x / 2) - Mathf.FloorToInt(negativeDimensions.x / 2);

                if (DrawDebugCubes)
                {
                    smallerTransform.localPosition += new Vector3(0, 0, -smallerTransform.GetComponent<BlockSystem>().GetDimensions().z);
                    transform.localPosition += new Vector3(0, 0, -smallerTransform.GetComponent<BlockSystem>().GetDimensions().z);
                }
               break;

            case Alignment.EnemyPositiveXToPlayerNegativeX:
                positiveSide = BlockSystemManager.Instance.EnemySystem;
                negativeSide = BlockSystemManager.Instance.PlayerSystem;
                
                positiveDimensions = positiveSide.GetDimensions();
                negativeDimensions = negativeSide.GetDimensions();
                
                dimensions = new Int3(positiveDimensions.x + negativeDimensions.x,
                    Math.Max(positiveDimensions.y, negativeDimensions.y),
                    Math.Max(positiveDimensions.z, negativeDimensions.z));

                if (positiveDimensions.z < negativeDimensions.z)
                    positiveAlignmentOffset = Mathf.FloorToInt(negativeDimensions.z / 2) - Mathf.FloorToInt(positiveDimensions.z / 2);
                else if (positiveDimensions.z > negativeDimensions.z)
                    negativeAlignmentOffset = Mathf.FloorToInt(positiveDimensions.z / 2) - Mathf.FloorToInt(negativeDimensions.z / 2);

                if (DrawDebugCubes)
                {
                    smallerTransform.localPosition += new Vector3(-smallerTransform.GetComponent<BlockSystem>().GetDimensions().x, 0, 0);
                    transform.localPosition += new Vector3(-smallerTransform.GetComponent<BlockSystem>().GetDimensions().x, 0, 0);
                }
                break;

            case Alignment.EnemyPositiveZToPlayerNegativeZ:
                positiveSide = BlockSystemManager.Instance.EnemySystem;
                negativeSide = BlockSystemManager.Instance.PlayerSystem;
                
                positiveDimensions = positiveSide.GetDimensions();
                negativeDimensions = negativeSide.GetDimensions();
                
                dimensions = new Int3(Math.Max(positiveDimensions.x, negativeDimensions.x),
                    Math.Max(positiveDimensions.y, negativeDimensions.y),
                    positiveDimensions.z + negativeDimensions.z);

                if (positiveDimensions.x < negativeDimensions.x)
                    positiveAlignmentOffset = Mathf.FloorToInt(negativeDimensions.x / 2) - Mathf.FloorToInt(positiveDimensions.x / 2);
                else if (positiveDimensions.x > negativeDimensions.x)
                    negativeAlignmentOffset = Mathf.FloorToInt(positiveDimensions.x / 2) - Mathf.FloorToInt(negativeDimensions.x / 2);


                if (DrawDebugCubes)
                {
                    smallerTransform.localPosition += new Vector3(0, 0, -smallerTransform.GetComponent<BlockSystem>().GetDimensions().z);
                    transform.localPosition += new Vector3(0, 0, -smallerTransform.GetComponent<BlockSystem>().GetDimensions().z);
                }
                break;
        }


        Block[,,] blocks = new Block[dimensions.x, dimensions.y, dimensions.z];

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    if (x < positiveDimensions.x && y < positiveDimensions.y && z < positiveDimensions.z)
                    {
                        if ((AlignmentEdge == Alignment.PlayerPositiveXToEnemyNegativeX || AlignmentEdge == Alignment.EnemyPositiveXToPlayerNegativeX)
                            && z + positiveAlignmentOffset < dimensions.z)
                            blocks[x, y, z + positiveAlignmentOffset] = positiveSide.GetBlockByLocation(x, y, z);
                        else if ((AlignmentEdge == Alignment.PlayerPositiveZToEnemyNegativeZ || AlignmentEdge == Alignment.EnemyPositiveZToPlayerNegativeZ)
                            && x + positiveAlignmentOffset < dimensions.x)
                            blocks[x + positiveAlignmentOffset, y, z] = positiveSide.GetBlockByLocation(x, y, z);
                    }
                    else if (x >= positiveDimensions.x && y < negativeDimensions.y && z < negativeDimensions.z && (AlignmentEdge == Alignment.PlayerPositiveXToEnemyNegativeX || AlignmentEdge == Alignment.EnemyPositiveXToPlayerNegativeX))
                    {
                        if (z + negativeAlignmentOffset < dimensions.z)
                            blocks[x, y, z + negativeAlignmentOffset] = negativeSide.GetBlockByLocation(x - positiveDimensions.x, y, z);
                    }
                    else if (x < negativeDimensions.x && y < negativeDimensions.y && z >= positiveDimensions.z && (AlignmentEdge == Alignment.PlayerPositiveZToEnemyNegativeZ || AlignmentEdge == Alignment.EnemyPositiveZToPlayerNegativeZ))
                    {
                        if (x + negativeAlignmentOffset < dimensions.x)
                            blocks[x + negativeAlignmentOffset, y, z] = negativeSide.GetBlockByLocation(x, y, z - positiveDimensions.z);
                    }

                    if (blocks[x, y, z] != null)
                    {
                        blocks[x, y, z].AlignedLocation = new Int3(x, y, z);
                    }
                    else if (DrawDebugCubes)
                    {
                        Block block = Instantiate(DebugBlockPrefab).GetComponent<Block>();
                        block.Initialize(AlignedBlockSystem, new Int3(x, y, z));
                        block.AlignedLocation = new Int3(x, y, z);
                        blocks[x, y, z] = block;
                    }
                }
            }
        }
        return blocks;
    }
}
