using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Trainer : MonoBehaviour
{
    // Get the top N fittest genomes
    public static readonly int FITTEST_COUNT = 10;

    [Header("Prefab")] public GameObject kartPrefab;
    public Transform spawner;

    [Header("Parameters")] public int populationSize = 20;
    public int startGenomeLength = 5;
    public int growthSize = 1;

    [Header("Save")] public string fileName = "save.csv";
    public bool save = true;

    [Header("UI (User Interface)")] public Text generationText;
    public Text genomeLengthText;

    private List<Brain> m_Population = new List<Brain>();
    private int m_Alives;
    private int m_Generation = 1;
    private int m_GenomeLength;

    private bool m_Initialized;

    public void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_GenomeLength = startGenomeLength;

        for (int i = 0; i < populationSize; i++)
        {
            Brain brain = CreateKartWithGenome(new Genome(m_GenomeLength));
            m_Population.Add(brain);
        }

        WriteSaveHeader();

        InitializeBrains();
        m_Initialized = true;
    }

    private void InitializeBrains()
    {
        foreach (Brain brain in m_Population)
            brain.Initialize();
    }

    private Brain CreateKartWithGenome(Genome genome)
    {
        GameObject kart = Instantiate(kartPrefab, spawner.position, spawner.rotation);
        Brain brain = kart.GetComponent<Brain>();
        brain.Genome = genome;
        return brain;
    }

    private void UpdateUI()
    {
        if (generationText)
            generationText.text = $"GENERATION: {m_Generation}";

        if (genomeLengthText)
            genomeLengthText.text = $"GENOMES LENGTH: {m_GenomeLength}";
    }

    private void Update()
    {
        if (!m_Initialized) return;

        m_Alives = m_Population.Where(x => x.Alive).ToList().Count;

        if (m_Alives == 0)
        {
            NewPopulation();
        }

        UpdateUI();
    }

    private void KillAll()
    {
        foreach (var brain in m_Population)
        {
            if (brain.Alive)
                brain.Kill();
        }
    }

    private List<Genome> GetAllGenomes()
    {
        var genomes = new List<Genome>();
        for (int i = 0; i < populationSize; i++)
            genomes.Add(m_Population[i].Genome);

        return genomes;
    }

    private void NewPopulation()
    {
        // Grow if any did not hit wall
        var shouldGrow = m_Population.Any(brain => !brain.HitWall);
        KillAll();

        var genomes = GetAllGenomes();
        var newPopulation = new List<Brain>();

        if (save)
            WriteSaveData(genomes);

        var sumFitnesses = genomes.Sum(genome => genome.Fitness);

        // Normalize
        foreach (var genome in genomes)
        {
            genome.Fitness /= sumFitnesses;
        }

        // Add the fittest to the new population
        var bestGenomes = GetFittest(genomes);
        foreach (var genome in bestGenomes)
        {
            if (shouldGrow)
            {
                for (int j = 0; j < growthSize; j++)
                    genome.Growth();
            }

            newPopulation.Add(CreateKartWithGenome(genome));
        }

        // Add new children based on parents using roulettePicker
        for (int i = 0; i < populationSize - FITTEST_COUNT; i++)
        {
            var parent1 = RoulettePicker(genomes);
            var parent2 = RoulettePicker(genomes);

            var child = parent1.Crossover(parent2);
            child.Mutate();
            // Growth
            if (shouldGrow)
            {
                for (var j = 0; j < growthSize; j++)
                {
                    child.Growth();
                }
            }

            newPopulation.Add(CreateKartWithGenome(child));
        }

        for (int i = 0; i < populationSize; i++)
        {
            Destroy(m_Population[i].gameObject);
        }

        m_Population.Clear();
        m_Population = newPopulation;
        InitializeBrains();

        if (shouldGrow)
        {
            m_GenomeLength += growthSize;
        }

        m_Generation++;
    }

    public static List<Genome> GetFittest(List<Genome> genomes)
    {
        List<Genome> fittest = new List<Genome>();
        var candidates = genomes.OrderByDescending(g => g.Fitness).ToList();
        for (int i = 0; i < FITTEST_COUNT; i++)
            fittest.Add((Genome) candidates[i].Clone());

        return fittest;
    }

    private static Genome RoulettePicker(List<Genome> genomes)
    {
        var index = 0;
        var r = Random.value;
        while (r > 0)
        {
            r -= genomes[index].Fitness;
            index++;
        }

        if (index > 0)
            index--;
        if (index >= genomes.Count)
            throw new ArithmeticException("Could not apply roulette wheel selection");

        return genomes[index];
    }


    private void WriteSaveHeader()
    {
        using (StreamWriter file = new StreamWriter(fileName, false))
        {
            file.WriteLine("{0},{1}", "average fitness", "best fitness");
        }
    }

    private void WriteSaveData(List<Genome> genomes)
    {
        using (StreamWriter file = new StreamWriter(fileName, true))
        {
            float bestFitness = 0.0f;

            float averageFitness = 0.0f;
            foreach (var genome in genomes)
            {
                averageFitness += genome.Fitness;
                if (bestFitness < genome.Fitness)
                {
                    bestFitness = genome.Fitness;
                }
            }

            averageFitness /= genomes.Count;
            file.WriteLine("{0},{1}", averageFitness, bestFitness);
        }
    }
}