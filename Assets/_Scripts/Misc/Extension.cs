using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static BattleSide Opposite(this BattleSide side)
    {
        if (side == BattleSide.Right)
            return BattleSide.Left;
        else
            return BattleSide.Right;
    }
}
