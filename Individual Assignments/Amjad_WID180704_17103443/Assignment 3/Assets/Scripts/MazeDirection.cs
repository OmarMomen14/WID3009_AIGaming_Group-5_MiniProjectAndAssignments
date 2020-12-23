using System;
using UnityEngine;

public enum MazeDirection
{
    North,
    South,
    East,
    West
}

public static class MazeDirections
{
    private const int Count = 4;

    private static readonly Vec2d[] Directions =
    {
        new Vec2d(0, 1),
        new Vec2d(0, -1),
        new Vec2d(1, 0),
        new Vec2d(-1, 0),
    };

    private static readonly Quaternion[] Rotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0, 180, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 270, 0),
    };

    private static readonly MazeDirection[] Opposites =
    {
        MazeDirection.South,
        MazeDirection.North,
        MazeDirection.West,
        MazeDirection.East,
    };

    public static Vec2d ToVec2d(this MazeDirection direction)
    {
        return Directions[(int) direction];
    }

    public static MazeDirection? FromVec2d(Vec2d diff)
    {
        var index = Array.IndexOf(Directions, diff);
        if (index == -1) return null;

        var values = Enum.GetValues(typeof(MazeDirection));
        if (values.GetValue(index) is MazeDirection)
            return (MazeDirection) values.GetValue(index);

        return null;
    }

    public static Quaternion Rotation(this MazeDirection direction)
    {
        return Rotations[(int) direction];
    }

    public static MazeDirection Opposite(this MazeDirection direction)
    {
        return Opposites[(int) direction];
    }
}