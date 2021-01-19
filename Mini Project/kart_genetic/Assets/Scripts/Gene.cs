using System;
using Random = UnityEngine.Random;

public class Gene : ICloneable
{
    public static readonly float MIN_HORIZONTAL = -1.0f;
    public static readonly float MAX_HORIZONTAL = 1.0f;
    public static readonly float MIN_VERTICAL = 0.0f;
    public static readonly float MAX_VERTICAL = 1.0f;

    public float Horizontal { get; set; }
    public float Vertical { get; set; }

    public Gene()
    {
        // Generate a new randomized gene
        Randomize();
    }

    public void Randomize()
    {
        Horizontal = Random.Range(MIN_HORIZONTAL, MAX_HORIZONTAL);
        Vertical = Random.Range(MIN_VERTICAL, MAX_VERTICAL);
    }

    public object Clone()
    {
        var gene = (Gene) MemberwiseClone();
        return gene;
    }
}