using UnityEngine;

/// <summary>
/// Handles opening and closing a vertical door door when the player interacts with it.
/// Uses a coroutine to smoothly move the door between its open and closed positions.
/// </summary>
public class VerticalDoor : MonoBehaviour, IInteractable
{
    [Header("Open Settings")]
    [Tooltip("How high the door moves when opening.")]
    public float openHeight = 5f;

    [Tooltip("Speed at which the door opens and closes.")]
    public float openSpeed = 2f;

    [Header("Runtime State")]
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    /// <summary>
    /// Stores the door position and calculates the open position based on the desired height.
    /// </summary>
    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
    }

    /// <summary>
    /// Called by the player when interacting. Toggles the door open/closed and starts its movement animation.
    /// </summary>
    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
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
            yield return null;
        }

        // Snap exactly to target to avoide small floating-point offsets
        transform.position = target; 
    }
}
