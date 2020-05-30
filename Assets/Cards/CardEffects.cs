using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects
{
    public static void DoNothing()
    {

    }

    public static void RandomlyDissolveTopLayer(BlockSystem blocks, float percentage, float duration)
    {
        List<Int3> toBreak = new List<Int3>();
        for (int x = 0; x < blocks.GetDimensions().x; x++)
        {
            for (int z = 0; z < blocks.GetDimensions().z; z++)
            {
                Int3? highest = blocks.GetHighestSolidLocation(x, z);
                if (Random.value > percentage && highest)
                {
                    toBreak.Add((Int3)highest);
                }
            }
        }

        foreach (Int3 block in toBreak)
        {
            CardEffectController.Instance.StartCoroutine(Coroutines.WaitThen(Random.value * duration, () =>
            {
                blocks.Break(block);
            }));
        }
    }
}
