using UnityEngine;

public class SnapToStand : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the tablet collided with a stand
        Stand stand = other.GetComponent<Stand>();
        if (stand != null)
        {
            // Snap the tablet to the stand's snap position
            Transform snapPosition = stand.snapPosition;
            if (snapPosition != null)
            {
                transform.position = snapPosition.position;
                //transform.rotation = snapPosition.rotation; // Align rotation if needed

                // Optionally, disable movement of the tablet
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Disable physics movement
                }
            }
        }
    }
}
