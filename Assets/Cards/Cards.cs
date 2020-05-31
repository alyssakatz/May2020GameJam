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
        Execute = (CardTargettingInfo info) =>
        {
            CardEffects.RandomlyDissolveTopLayer(BlockSystemManager.Instance.EnemySystem, 0.5f, 5);
        },
        LoadTime = 10
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
        Execute = (CardTargettingInfo info) =>
        {
            Int3? toBreak = BlockSystemManager.Instance.AlignedSystem.GetHighestSolidLocation((Int3) info.targetLocation);

            if (toBreak)
            {
                BlockSystemManager.Instance.AlignedSystem.Break((Int3)toBreak);
            }
        },
        LoadTime = 5
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
        Execute = CardEffects.DoNothing,
        LoadTime = 10
    };

    // this needs to be after all CardInfo
    public static CardInfo[] CardList =
    {
        UITestCard1,
        UITestCard2,
        UITestCard3
    };
}
