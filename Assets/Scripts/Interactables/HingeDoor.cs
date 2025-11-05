using UnityEngine;

public class HingeDoor : MonoBehaviour, IInteractable
{
    [Header("Open Settings")]

    [Tooltip("How far the door rotates when opening.")]
    public float openAngle = 90f;

    [Tooltip("Speed at which the door opens and closes.")]
    public float openSpeed = 2f;
    public Transform hinge;

    [Header("Runtime State")]
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    /// <summary>
    /// Stores the door position amd calculates the rotation based on the current and hinge position.
    /// </summary>
    void Start()
    {
        if (hinge == null) hinge = transform; 

        closedRotation = hinge.rotation;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, hinge.up); 
    }

    /// <summary>
    /// Called by the player when interacting. Toggles the door open/closed and starts its movement animation.
    /// </summary>
    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen));
    }

    /// <summary>
    /// Smoothly rotates the door toward either the open or closed position over multiple frames.
    /// Uses a coroutine so the movement appears animated rather than instant.
    /// </summary>
    private System.Collections.IEnumerator RotateDoor(bool open)
    {
        Quaternion targetRotation = open ? openRotation : closedRotation;

        // Move until close enough to target
        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        // Snap exactly to target to avoid small floating-point offsets
        hinge.rotation = targetRotation;
    }
}
