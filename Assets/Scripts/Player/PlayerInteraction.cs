using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Handles player interactions such as using objects and picking items up.
/// Uses raycasting from the camera to detect objects that implement interaction.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Maximum distance the player can interact with objects")]
    public float interactionRange = 3f;

    [Tooltip("Layer mask for objects the player can interact with")]
    public LayerMask interactableLayer;

    [Header("References")]
    public InteractionUI interactionUI;

    [Tooltip("Where picked-up items will be held")]
    public Transform holdPoint; 

    // Interface references for objects the player can interact with or pick up
    private IInteractable currentInteractable;
    private IPickupable currentPickupable;

    // The item currently held by the player
    private IPickupable heldItem;

    void Update()
    {
        CheckForInteractable();
        
        // Handle interaction input
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Prioritize interacting over picking up
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
            else if (currentPickupable != null)
            {
                currentPickupable.Pickup(holdPoint);
                heldItem = currentPickupable;
            }
        }

        // Handle dropping input
        if (heldItem != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            heldItem.Drop();
            heldItem = null;
        }
    }

    /// <summary>
    /// Casts a ray from the camera forward to detect interactable or pickupable objects.
    /// Updates UI and references based on what's found.
    /// </summary>
    void CheckForInteractable()
    { 
        currentInteractable = null;
        currentPickupable = null;

        // Cast a ray from the center of the camera forward
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        { 
            // Check if the object supports interaction
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                interactionUI.Show($"Press E to interact with {hit.collider.name}");
                return;
            }

            // Check if the object can be picked up
            if (hit.collider.TryGetComponent(out IPickupable pickupable))
            {
                currentPickupable = pickupable;
                interactionUI.Show($"Press E to pick up {hit.collider.name}");
                return;
            }
        }

        interactionUI.Hide();
    }
}
