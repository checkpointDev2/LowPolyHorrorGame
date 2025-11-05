using UnityEngine;

/// <summary>
/// Handles opening and closing a vertical door door when the player interacts with it.
/// Uses a coroutine to smoothly move the door between its open and closed positions.
/// </summary>
public class SlidingDoor : MonoBehaviour, IInteractable
{
    [Header("Slide Settings")]
    [Tooltip("How far the door moves when opening.")]
    public float slideDistance = 3f;

    [Tooltip("Speed at which the door opens and closes.")]
    public float openSpeed = 2f;

    [Tooltip("If true, the door slides along the local X axis. Othewise, it slides forward.")]
    public bool slideAlongX = true;

    [Header("Runtime State")]
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    
    /// <summary>
    /// Stores the door position and calculates the open position based on the slide direction.
    /// </summary>
    void Start()
    {
        closedPosition = transform.position;

        // Determine slide direction based on chosen axis
        openPosition = slideAlongX
            ? closedPosition + transform.right * slideDistance
            : closedPosition + transform.forward * slideDistance;
    }

    /// <summary>
    /// Called by the player when interacting. Toggles the door open/closed and starts its movement animation.
    /// </summary>
    public void Interact()
    {
        isOpen = !isOpen;

        // Stop any currently running movement to precent overlapping animations
        StopAllCoroutines();

        // Start smoothly moving the door to its new target position
        StartCoroutine(MoveDoor(isOpen));
    }

    /// <summary>
    /// Smoothly moves the door toward either the open or closed position over multiple frames.
    /// Uses a coroutine so the movement appears animated rather than instant.
    /// </summary>
    private System.Collections.IEnumerator MoveDoor(bool open)
    {
        Vector3 target = open ? openPosition : closedPosition;

        // Move until close enough to target
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * openSpeed);
            yield return null; // Wait for the next frame
        }

        // Snap exactly to target to avoide small floating-point offsets
        transform.position = target;
    }
}
