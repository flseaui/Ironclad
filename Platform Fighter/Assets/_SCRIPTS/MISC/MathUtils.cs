namespace MISC
{
    public static class MathUtils
    {
        public static int Mod(int value, int modVal)
        {
            if (value >= 0)
                return value % modVal;
            return modVal + (value % modVal);
        }
    }
}