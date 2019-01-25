using UnityEngine;

namespace MISC
{
    public static class MathUtils
    {
        public static int Mod(int value, int modVal)
        {
            if (value >= 0)
                return value % modVal;
            return modVal + value % modVal;
        }
        
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
  
        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }
        
    }
}