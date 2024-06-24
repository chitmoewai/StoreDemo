using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCamera;  // The camera used by the FPS controller
    public float speed = 12f;
    public float pickUpRange = 5f;  // The range within which the player can pick up objects
    public float moveSpeed = 10f;  // Speed at which the object moves while being carried
    public Transform[] snapPoints;  // Array of snap points on the rack

    private GameObject pickedObject = null;  // The currently picked up object
    private Rigidbody pickedObjectRb = null;  // Rigidbody of the picked up object
    private bool isCarrying = false;  // Whether the player is currently carrying an object
    private bool isMoving = true;  // Whether the player is currently moving

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        if (isMoving)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }
    }

    private void HandleInteraction()
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
                pickedObjectRb = pickedObject.GetComponent<Rigidbody>();
                pickedObjectRb.isKinematic = true;  // Make the object kinematic to disable physics
                isCarrying = true;
                isMoving = false;  // Stop player movement while carrying an object
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

            pickedObjectRb.isKinematic = false;  // Re-enable physics
            pickedObject = null;
            pickedObjectRb = null;
            isCarrying = false;
            isMoving = true;  // Allow player movement again
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
