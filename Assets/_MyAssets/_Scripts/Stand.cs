using UnityEngine;

public class Stand : MonoBehaviour
{
    public Transform snapPosition; // The position where the tablet should snap to

    void OnDrawGizmos()
    {
        if (snapPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(snapPosition.position, 0.1f);
        }
    }
}
