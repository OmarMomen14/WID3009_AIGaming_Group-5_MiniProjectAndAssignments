using System;

[Serializable]
public struct Vec2d : IComparable<Vec2d>
{
    public int x, z;

    public Vec2d(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static Vec2d operator +(Vec2d a, Vec2d b)
    {
        return new Vec2d(a.x + b.x, a.z + b.z);
    }

    public static Vec2d operator -(Vec2d a, Vec2d b)
    {
        return new Vec2d(a.x - b.x, a.z - b.z);
    }

    public int CompareTo(Vec2d other)
    {
        var xComparison = x.CompareTo(other.x);
        return xComparison != 0 ? xComparison : z.CompareTo(other.z);
    }

    public override string ToString()
    {
        return "Vec2d(" + x + ", " + z + ")";
    }

    public Vec2d Copy()
    {
        return new Vec2d(x, z);
    }
}