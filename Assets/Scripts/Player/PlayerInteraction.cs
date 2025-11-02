using UnityEngine;
using UnityEngine.InputSystem;

// Handles the detection of interactable objects in range of the player and triggering interactions
public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // Max distance to interact
    public LayerMask interactableLayer; // Determines which layer can be interacted with

    public InteractionUI interactionUI; // Reference to the UI script

    private IInteractable currentInteractable; // The interactable object in range of the player

    void Update()
    {
        // Check for an interactable object in front of the player
        CheckForInteractable();

        // Trigger interaction when looking at an object and pressing E
        if (currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact();
        }
    }

    // Raycasts forward to detect interactables
    void CheckForInteractable()
    {
        currentInteractable = null;

        // Create a ray from the camera position forward
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the ray hits a collider on the interactable layer
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            // Retrieve IInteractable component from collider
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable; // Store current interactable
                interactionUI.Show("Press E to interact with " + hit.collider.name);
                return;
            }
        }

        // Hide UI if nothing is interactable
        interactionUI.Hide();
    }
}
