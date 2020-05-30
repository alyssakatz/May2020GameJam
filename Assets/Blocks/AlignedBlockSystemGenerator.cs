using UnityEngine;
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

    public override Block[,,] GenerateBlocks(BlockSystem AlignedBlockSystem)
    {
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

                break;
        }


        Block[,,] blocks = new Block[dimensions.x, dimensions.y, dimensions.z];

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    if (x < positiveDimensions.x && z < positiveDimensions.z)
                    {
                        if ((AlignmentEdge == Alignment.PlayerPositiveXToEnemyNegativeX || AlignmentEdge == Alignment.EnemyPositiveXToPlayerNegativeX)
                            && z + positiveAlignmentOffset < dimensions.z)
                            blocks[x, y, z + positiveAlignmentOffset] = positiveSide.GetBlockByLocation(x, y, z);
                        else if ((AlignmentEdge == Alignment.PlayerPositiveZToEnemyNegativeZ || AlignmentEdge == Alignment.EnemyPositiveZToPlayerNegativeZ)
                            && x + positiveAlignmentOffset < dimensions.x)
                            blocks[x + positiveAlignmentOffset, y, z] = positiveSide.GetBlockByLocation(x, y, z);
                    }
                    else if (x >= positiveDimensions.x && (AlignmentEdge == Alignment.PlayerPositiveXToEnemyNegativeX || AlignmentEdge == Alignment.EnemyPositiveXToPlayerNegativeX))
                    {
                        if (z + negativeAlignmentOffset < dimensions.z)
                            blocks[x, y, z + negativeAlignmentOffset] = negativeSide.GetBlockByLocation(x - positiveDimensions.x, y, z);
                    }
                    else if (z >= positiveDimensions.z && (AlignmentEdge == Alignment.PlayerPositiveZToEnemyNegativeZ || AlignmentEdge == Alignment.EnemyPositiveZToPlayerNegativeZ))
                    {
                        if (x + negativeAlignmentOffset < dimensions.x)
                            blocks[x + negativeAlignmentOffset, y, z] = negativeSide.GetBlockByLocation(x, y, z - positiveDimensions.z);
                    }

                    if (blocks[x, y, z] != null)
                        blocks[x, y, z].AlignedLocation = new Int3(x, y, z);
                }
            }
        }

        return blocks;
    }
}
