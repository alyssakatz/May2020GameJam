using UnityEngine;
using System.Collections;

public class BlockSystemManager : MonoSingleton<BlockSystemManager>
{
    public BlockSystem PlayerSystem;
    public BlockSystem EnemySystem;
    public BlockSystem AlignedSystem;

    void Start()
    {
        StartCoroutine(WaitUntilDependenciesInitialized());
        AlignedSystem.gameObject.SetActive(true);
    }

    IEnumerator WaitUntilDependenciesInitialized()
    {
        yield return new WaitUntil(() => PlayerSystem.AreBlocksGenerated && EnemySystem.AreBlocksGenerated);
    }
}
