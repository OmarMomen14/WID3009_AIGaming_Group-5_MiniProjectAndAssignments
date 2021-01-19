using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Genome : ICloneable
{
    private static float MUTATION_PROBABILITY = 0.05f;
    private static float ALPHA = 0.2f;

    private List<Gene> m_Genes;

    public Gene this[int index]
    {
        get { return m_Genes[index]; }
        set { m_Genes[index] = value; }
    }

    public int Length => m_Genes != null ? m_Genes.Count : 0;

    public float Fitness { get; set; }

    public Genome()
    {
        Fitness = 0.0f;
        m_Genes = new List<Gene>();
    }

    public Genome(int actionNumber) : this()
    {
        for (int i = 0; i < actionNumber; i++)
            Growth();
    }

    public void Growth()
    {
        m_Genes.Add(new Gene());
    }

    public Genome(Genome source) : this()
    {
        for (int i = 0; i < source.Length; i++)
            m_Genes.Add((Gene) source[i].Clone());
    }

    public object Clone()
    {
        return new Genome(this);
    }

    public void Mutate()
    {
        if (m_Genes == null)
            return;

        float rate = MUTATION_PROBABILITY / Length;

        for (int i = 0; i < Length; i++)
        {
            if (Random.Range(0, 1f) < rate * (i + 1))
            {
                m_Genes[i].Randomize();
            }
        }
    }

    public Genome Crossover(Genome other)
    {
        if (m_Genes == null || other == null)
            return null;

        var minLength = Math.Min(Length, other.Length);

        var result = new Genome(minLength);

        for (int i = 0; i < minLength; i++)
        {
            result[i].Vertical = m_Genes[i].Vertical + ALPHA * (other.m_Genes[i].Vertical - m_Genes[i].Vertical);
            result[i].Horizontal =
                m_Genes[i].Horizontal + ALPHA * (other.m_Genes[i].Horizontal - m_Genes[i].Horizontal);
        }

        return result;
    }
}