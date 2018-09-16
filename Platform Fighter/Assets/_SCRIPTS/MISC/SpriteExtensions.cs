using UnityEngine;

namespace MISC
{
    public static class SpriteExtensions
    {
        public static Sprite ToSprite(this Texture2D tex, Vector2? vec = null)
        {
            var vector = vec ?? new Vector2(.5f, .5f);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), vector);
        }
    }
}