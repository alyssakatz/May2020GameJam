using UnityEngine;

public class Cards
{
    private static CardInfo UITestCard1 = new CardInfo
    {
        Name = "Acid Rain",
        RulesText = "Dissolves 50% of the Enemy's top layer of blocks over the next 5 seconds.",
        FlavorText = "Better bring at least three umbrellas.",
        IconLocation = "CardIcons/acidRain",
        Execute = () =>
        {
            CardEffects.RandomlyDissolveTopLayer(BlockSystem.EnemySystem, 0.5f, 5);
        }
    };

    private static CardInfo UITestCard2 = new CardInfo
    {
        Name = "TEST CARD",
        RulesText = "This is the rules text for the card",
        FlavorText = "This is the flavor text for the card",
        IconLocation = "CardIcons/example2",
        Execute = CardEffects.DoNothing
    };

    private static CardInfo UITestCard3 = new CardInfo
    {
        Name = "TEST CARD",
        RulesText = "This is the rules text for the card",
        FlavorText = "This is the flavor text for the card",
        IconLocation = "CardIcons/example3",
        Execute = CardEffects.DoNothing
    };

    // this needs to be after all CardInfo
    public static CardInfo[] CardList =
    {
        UITestCard1,
        UITestCard2,
        UITestCard3
    };
}
