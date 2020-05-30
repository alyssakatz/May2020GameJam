using UnityEngine.UI;
using UnityEngine;

public abstract class CardTemplate
{
    public abstract string Name { get; }
    public abstract string Text { get; }
    public abstract Sprite Icon { get; }
    public abstract void Execute();
}
