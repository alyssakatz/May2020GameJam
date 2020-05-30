using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cards.CardList[0].Execute();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
