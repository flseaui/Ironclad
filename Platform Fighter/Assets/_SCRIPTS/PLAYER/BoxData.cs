using DATA;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class BoxData : MonoBehaviour
    {
        public double KnockbackAngle { get; private set; }
        public double KnockbackStrength { get; private set; }
        public double Damage { get; private set; }

        public int Lifespan { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ActionInfo.Box.BoxType Type { get; private set; }

        public Types.ActionType ParentAction { get; private set; }
        public int ParentFrame { get; private set; }

        public void SetAsNull() => Type = ActionInfo.Box.BoxType.Null;

        public void SetData(ActionInfo.Box box, Types.ActionType actionType, int frame)
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

            ParentAction = actionType;
            ParentFrame = frame;
        }
    }
}