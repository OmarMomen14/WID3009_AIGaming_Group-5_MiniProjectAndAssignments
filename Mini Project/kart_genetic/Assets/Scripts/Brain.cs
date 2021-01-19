using UnityEngine;

public class Brain : MonoBehaviour
{
    public Genome Genome { get; set; }
    public bool Alive { get; set; }
    public bool HitWall { get; set; }
    public Material deadMaterial;
    private CarKinematics m_Controller;
    private int m_CurrentGene;
    private float m_StartTime;
    private Renderer[] m_Renderers;
    private RoadSegment m_CurrentRoadSegment;
    private int m_RoadNumbers;

    private void Awake()
    {
        m_Controller = GetComponent<CarKinematics>();
        m_Renderers = GetComponentsInChildren<Renderer>();
    }

    public void Initialize()
    {
        Alive = true;
        m_StartTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (!Alive) return;

        m_Controller.VerticalInput = Genome[m_CurrentGene].Vertical;
        m_Controller.HorizontalInput = Genome[m_CurrentGene].Horizontal;

        if (Time.time - m_StartTime >= 0.5)
        {
            m_CurrentGene++;
            if (m_CurrentGene == Genome.Length)
                Kill();
            else
                m_StartTime = Time.time;
        }
    }

    public void Kill()
    {
        Alive = false;
        foreach (Renderer render in m_Renderers)
            render.material = deadMaterial;

        m_Controller.Disable = true;

        EvaluateFitness();
    }

    public void EvaluateFitness()
    {
        var nextSegmentPosition = m_CurrentRoadSegment.nextRoadSegment.transform.position;

        var totalDistance =
            Vector3.Distance(m_CurrentRoadSegment.transform.position,
                nextSegmentPosition);
        var distanceToNextPosition =
            Vector3.Distance(transform.position, nextSegmentPosition);
        Genome.Fitness = (m_RoadNumbers * 100.0f) + (totalDistance - distanceToNextPosition);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Wall"))
        {
            HitWall = true;
            Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Road"))
        {
            RoadSegment roadSegment = other.GetComponent<RoadSegment>();
            if (m_CurrentRoadSegment == null)
            {
                m_CurrentRoadSegment = roadSegment;
            }
            else
            {
                if (roadSegment.m_SequenceId > m_CurrentRoadSegment.m_SequenceId ||
                    roadSegment.m_SequenceId == 1 && m_CurrentRoadSegment.m_SequenceId == 20)
                {
                    m_RoadNumbers++;
                }

                m_CurrentRoadSegment = roadSegment;
            }
        }
    }
}