using UnityEngine;
using UnityEngine.UI;

public class CardIcon : MonoBehaviour
{
    public CardTemplate Card;
    private bool CardIconIsDirty;
    private Image UiIcon;

    // Start is called before the first frame update
    void Start()
    {
        UiIcon = GetComponent<Image>();
        if(!UiIcon)
        {
            Debug.Log("why");
        }
        Card = new UIExampleCard();
        CleanCardInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if(CardIconIsDirty)
        {
            CleanCardInfo();   
            CardIconIsDirty = false;
        }
    }

    void CleanCardInfo()
    {
        Debug.Log("here");
        UiIcon.sprite = Card.Info.Icon;
    }
}
