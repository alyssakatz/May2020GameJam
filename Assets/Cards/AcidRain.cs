using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Execute(BlockSystem.EnemySystem);
    }
    public float duration = 5;
    public float percentage = 0.5f;
    void Execute(BlockSystem blocks)
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
            StartCoroutine(WaitAndBreak(blocks, block, Random.value * duration));
        }


    }

    private IEnumerator WaitThen(float waitTime, System.Action action){
        yield return new WaitForSeconds(waitTime);
        action.Invoke();
    }

    private IEnumerator WaitAndBreak(BlockSystem blocks, Int3 location, float time)
    {
        yield return new WaitForSeconds(time);
        blocks.Break(location);
    }
}
