using UnityEngine;
using UnityEngine.U2D;

namespace MISC
{
    [CreateAssetMenu(menuName = "Sprite Bundle")]
    public class SpriteBundle : ScriptableObject
    {
        public SpriteAtlas[] Sprites;
    }
}