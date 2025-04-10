using UnityEngine;

public class BossMovementRestrictionsManager : Singleton<BossMovementRestrictionsManager>
{
    int j;
    bool inside;
    float distance;

    public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
    {
        Instance.j = polyPoints.Length - 1;
        Instance.inside = false;
        for (int i = 0; i < polyPoints.Length; Instance.j = i++)
        {
            var pi = polyPoints[i];
            var pj = polyPoints[Instance.j];
            if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                Instance.inside = !Instance.inside;
        }
        return Instance.inside;
    }

    public static BossAndPlayerDistance CurrentDistance(Vector3 bossPosition, Vector3 playerPosition, float innerCircleRadius, float outerCircleRadius)
    {
        Instance.distance = (playerPosition - bossPosition).magnitude;
        
        if (Instance.distance < innerCircleRadius)
        {
            return BossAndPlayerDistance.Close;
        }
        else if (Instance.distance < outerCircleRadius)
        {
            return BossAndPlayerDistance.Medium;
        }
        else
        {
            return BossAndPlayerDistance.Long;
        }
    }
}

public enum BossAndPlayerDistance
{
    None,
    Close,
    Medium,
    Long,
}
