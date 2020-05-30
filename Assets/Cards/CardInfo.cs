using UnityEngine;
using System;

public struct CardInfo
{
    public string Name;
    public string RulesText;
    public string FlavorText;
    public string IconLocation;
    public int? BaseRange;
    public bool CanTarget;
    public int TargetingRange;
    public Action Execute;

    public Sprite Icon => Resources.Load<Sprite>(IconLocation);
}
