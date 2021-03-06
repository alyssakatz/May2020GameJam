﻿using UnityEngine;
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
    public Action<CardTargettingInfo> Execute;
    public float LoadTime;

    public Sprite Icon => Resources.Load<Sprite>(IconLocation);

    public static implicit operator bool(CardInfo? info) => info != null;
}
