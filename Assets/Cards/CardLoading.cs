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
    float startTime;
    void Start()
    {
        transform.parent = source.gameObject.transform;
        transform.localPosition = new Vector3(0, 1, 0);
        startTime = Time.time;
        loadingBar.sprite = CardInfo.Icon;

        StartCoroutine(Coroutines.WaitThen(CardInfo.LoadTime, () =>
        {
            CardInfo.Execute(CardTargettingInfo);
            Destroy(this.gameObject);
        }));
    }

    void Update()
    {
        loadingBar.fillAmount = (Time.time - startTime) / CardInfo.LoadTime;

        //transform.LookAt(Camera.main.transform);
    }
}
