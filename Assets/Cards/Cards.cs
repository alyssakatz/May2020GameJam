public class Cards
{
    private static CardInfo UITestCard1 = new CardInfo
    {
        Name = "Acid Rain",
        RulesText = "Dissolves 50% of the Enemy's top layer of blocks over the next 5 seconds.",
        FlavorText = "Better bring at least three umbrellas.",
        IconLocation = "CardIcons/acidRain",
        BaseRange = null,
        CanTarget = false,
        TargetingRange = 0,
        Execute = () =>
        {
            CardEffects.RandomlyDissolveTopLayer(BlockSystemManager.Instance.EnemySystem, 0.5f, 5);
        }
    };

    private static CardInfo UITestCard2 = new CardInfo
    {
        Name = "Lightning",
        RulesText = "Lightning strikes 4 tiles away. Immobilize yourself to reposition the strike by up to 2 tiles.",
        FlavorText = "Thunderbolt and lightning, very very frightning!",
        IconLocation = "CardIcons/example2",
        BaseRange = 4,
        CanTarget = true,
        TargetingRange = 2,
        Execute = CardEffects.DoNothing
    };

    private static CardInfo UITestCard3 = new CardInfo
    {
        Name = "TEST CARD",
        RulesText = "This is the rules text for the card",
        FlavorText = "This is the flavor text for the card",
        IconLocation = "CardIcons/example3",
        BaseRange = 2,
        CanTarget = false,
        TargetingRange = 0,
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
