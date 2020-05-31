using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct CardTargettingInfo
{
    public readonly Int3? targetLocation;
    public CardTargettingInfo(Int3? targetLocation)
    {
        this.targetLocation = targetLocation;
    }
}
