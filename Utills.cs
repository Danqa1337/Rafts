using Unity.Mathematics;
using UnityEngine;
public static class Utills
{
    public static bool CheckPath(float2 start, float2 end)
    {
        var hit = Physics2D.Linecast(start, end, LayerMask.GetMask("RaftWalls"));
        return hit.collider == null;
    }
    public static bool Chance(float percent)
    {
        if (percent == 0) return false;
        if (percent == 100) return true;
        if (percent >= UnityEngine.Random.Range(0, 10000) * 0.01) return true;
        return false;

    }

}
