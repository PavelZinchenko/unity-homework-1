using UnityEngine;

public static class Helpers
{
    public static bool Contains(this LayerMask mask, int layerId)
    {
        var layer = 1 << layerId;
        return (mask.value & layer) != 0;
    }

    public static void UpdateDirection(this Transform transform, float speed, float speedThreshold, ref bool rightToLeft)
    {
        if (speed < -speedThreshold && !rightToLeft ||
            speed > speedThreshold && rightToLeft)
        {
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
            rightToLeft = !rightToLeft;
        }
    }
}