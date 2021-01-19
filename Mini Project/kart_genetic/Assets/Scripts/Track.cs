using UnityEngine;

public class Track : MonoBehaviour
{
    public static Track Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private RoadSegment[] m_Roads;

    private void Start()
    {
        m_Roads = GetComponentsInChildren<RoadSegment>();
        for (int i = 0; i < m_Roads.Length; i++)
        {
            m_Roads[i].m_SequenceId = i + 1;
            m_Roads[i].nextRoadSegment = m_Roads[(i + 1) % m_Roads.Length];
        }
    }
}