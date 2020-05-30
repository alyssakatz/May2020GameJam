using UnityEngine;
using UnityEngine.UI;

public class CardIcon : MonoBehaviour
{
    public int CardIndex;
    public Playspace.Hand Position;
    private CardInfo Card;
    private bool CardIconIsDirty;
    private Image UiIcon;

    // Start is called before the first frame update
    void Start()
    {
        UiIcon = GetComponent<Image>();
        Card = Cards.CardList[CardIndex];
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
        Debug.Log(Card.Icon);
        UiIcon.sprite = Card.Icon;
    }
}
