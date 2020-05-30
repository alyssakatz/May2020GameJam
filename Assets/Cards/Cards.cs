using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards
{
    public CardInfo[] CardList =
    {
        UITestCard
    };

    private static CardInfo UITestCard = new CardInfo
    {
        Name = "TEST CARD",
        RulesText = "This is the rules text for the card",
        FlavorText = "This is the flavor text for the card",
        Icon = Resources.Load<Sprite>("CardIcons/example.png"),
        Execute = CardEffects.DoNothing
    };
}
