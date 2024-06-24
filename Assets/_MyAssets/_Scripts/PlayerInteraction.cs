using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;  // The camera used by the FPS controller
    public float pickUpRange = 5f;  // The range within which the player can pick up objects
    public float moveSpeed = 10f;  // Speed at which the object moves while being carried
    public Transform[] snapPoints;  // Array of snap points on the rack

    private GameObject pickedObject = null;  // The currently picked up object
    private bool isCarrying = false;  // Whether the player is currently carrying an object

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left mouse button to pick up/drop objects
        {
            if (isCarrying)
            {
                DropObject();
            }
            else
            {
                TryPickUpObject();
            }
        }

        if (isCarrying)
        {
            MoveObject();
        }
    }

    void TryPickUpObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Tablet"))  // Assuming all tablets have the tag "Tablet"
            {
                pickedObject = hit.collider.gameObject;
                pickedObject.GetComponent<Rigidbody>().isKinematic = true;  // Make the object kinematic
                isCarrying = true;
            }
        }
    }

    void DropObject()
    {
        if (pickedObject != null)
        {
            Transform closestSnapPoint = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform snapPoint in snapPoints)
            {
                float distance = Vector3.Distance(pickedObject.transform.position, snapPoint.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSnapPoint = snapPoint;
                }
            }

            if (closestSnapPoint != null)
            {
                pickedObject.transform.position = closestSnapPoint.position;
                pickedObject.transform.rotation = closestSnapPoint.rotation;
            }

            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject = null;
            isCarrying = false;
        }
    }

    void MoveObject()
    {
        if (pickedObject != null)
        {
            Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            pickedObject.transform.position = Vector3.Lerp(pickedObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
