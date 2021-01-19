using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public int m_SequenceId;

    public RoadSegment nextRoadSegment;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        Gizmos.DrawSphere(transform.position, 1.0f);

        if (nextRoadSegment)
            Gizmos.DrawLine(transform.position, nextRoadSegment.transform.position);
    }
}