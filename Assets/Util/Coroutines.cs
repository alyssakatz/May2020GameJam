using System.Collections;
using UnityEngine;

class Coroutines
{
    public static IEnumerator WaitThen(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action.Invoke();
    }
}

