using DATA;
using UnityEngine;

namespace PLAYER
{
    public class BoxData : MonoBehaviour
    {
        public double Damage, KnockbackStrength;
        public Vector2 KnockbackAngle;
        public int Lifespan, X, Y, Width, Height;
        public ActionInfo.BoxType Type;

        public void setData(ActionInfo.Box box)
        {
            Damage = box.Damage;
            KnockbackStrength = box.KnockbackStrength;
            KnockbackAngle = box.KnockbackAngle;
            Lifespan = box.Lifespan;
            X = box.X;
            Y = box.Y;
            Width = box.Width;
            Height = box.Height;
            Type = box.Type;
        }
    }
}