using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    public Transform rightHandPosition; // Transform to attach when grabbed by right hand
    public Transform leftHandPosition;  // Transform to attach when grabbed by left hand

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Store initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactor;
        if (interactor is XRDirectInteractor)
        {
            var controller = interactor.GetComponent<XRController>();
            if (controller != null)
            {
                if (controller.controllerNode == XRNode.LeftHand)
                {
                    AttachToHand(leftHandPosition);
                }
                else if (controller.controllerNode == XRNode.RightHand)
                {
                    AttachToHand(rightHandPosition);
                }
            }
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Reset the position and rotation when released
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    private void AttachToHand(Transform handTransform)
    {
        // Adjust the tablet's position and rotation to match the specified hand transform
        transform.position = handTransform.position;
        transform.rotation = handTransform.rotation;
    }
}
