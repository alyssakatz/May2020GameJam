using UnityEngine;
using UnityEngine.UI;

public class CardIcon : MonoBehaviour
{
    public Playspace.Hand Position;
    private CardInfo Card;
    private Image UiIcon;

    public Sprite defaultImage;

    // Start is called before the first frame update
    void Start()
    {
        UiIcon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        CardInfo? card = Playspace.Instance.CardAtPosition(Position);
        if (card)
        {
            UiIcon.sprite = card.Value.Icon;
        } else
        {
            UiIcon.sprite = defaultImage;
        }
    }
}
