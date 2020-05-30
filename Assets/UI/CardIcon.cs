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
    }

    // Update is called once per frame
    void Update()
    {
        if(CardIconIsDirty)
        {
            UiIcon.sprite = Card.Icon;
            CardIconIsDirty = false;
        }
    }
}
