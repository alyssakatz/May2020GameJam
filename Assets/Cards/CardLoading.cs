using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLoading : MonoBehaviour
{
    public CardInfo CardInfo;
    public CardTargettingInfo CardTargettingInfo;
    public Block source;
    public Image loadingBar;
    // Start is called before the first frame update
    float startTime;
    void Start()
    {
        transform.parent = source.gameObject.transform;
        startTime = Time.time;

        StartCoroutine(Coroutines.WaitThen(CardInfo.LoadTime, () =>
        {
            CardInfo.Execute(CardTargettingInfo);
        }));
    }

    // Update is called once per frame
    void Update()
    {
        loadingBar.fillAmount = (Time.time - startTime) / CardInfo.LoadTime;
    }
}
