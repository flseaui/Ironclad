using UnityEngine;

public abstract class FighterBase : ScriptableObject
{
    public virtual void Initialize(FighterDetails fighter) { }
    public abstract void Think(FighterDetails fighter);
}
