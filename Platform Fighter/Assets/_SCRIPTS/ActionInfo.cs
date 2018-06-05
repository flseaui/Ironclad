using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ActionInfo : ScriptableObject
{
    enum FrameType { Startup, Active, Recovery, Buffer }
    enum BlockType { Low, Mid, High, Unblockable }
    enum KnockdownType { None, Soft, Hard, SoftGB, HardGB, SoftWB, HardWB }

    [SerializeField]
    private List<FrameType> frames;

    [Space(10)]
    [SerializeField]
    private BlockType blockType;

    [SerializeField]
    private KnockdownType knockdownType;

    private bool infinite;

    private struct Box
    {
        public Vector2 knockbackAngle;
        public float x, y, width, height, damage, knockbackStrength;
        public int id;

        public Box(float x, float y, float width, float height, float damage, float knockbackStrength, Vector2 knockbackAngle, int id)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.damage = damage;
            this.knockbackStrength = knockbackStrength;
            this.knockbackAngle = knockbackAngle;
            this.id = id;
        }

        public Box(int id) : this (0, 0, 5, 5, 0, 0, new Vector2(), id) { }
    }

    [SerializeField]
    private List<Box> hitboxes, hurtboxes;

    public int getFrameCount()
    {
        return frames.Count;
    }
}
