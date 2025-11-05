using UnityEngine;

/// <summary>
/// Represents a pick-up-able object that the player can hold, move, and throw.
/// Handles pickup, drop, and physics-based throws.
/// </summary>
public class Remote : MonoBehaviour, IPickupable
{
    [Header("Throw Settings")]
    [Tooltip("Set the forward momentum of the dropped object.")]
    public  float forwardThrowForce = 10.0f;

    [Header("Runtime State")]
    private bool isPickedUp = false;

    [Header("Component References")]
    private Transform holdPoint;
    private Rigidbody rb;
    private Collider col;
    private Vector3 rotationOffset = new Vector3(90, 0, 0);
    
    /// <summary>
    /// Initializes references to Rigidbody and Collider components.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    /// <summary>
    /// Called when the picks up the object.
    /// Sets the object to kinematic to disable physics while held and siables collisions with teh environment.
    /// </summary>
    /// <param name="holdPoint">The transform the object should follow.</param>
    public void Pickup(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
        isPickedUp = true;

        // Disable physics to move the object manually
        rb.isKinematic = true;

        // Smoothly interpolate the object's position/rotation for visual fidelity
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Disable collisions while held to prevent clipping with the player
        col.enabled = false;
    }

    /// <summary>
    /// Called when the player drops the object.
    /// Re-enables physics, applies forces for realistic throw, and allows collisions.
    /// </summary>
    public void Drop()
    {
        isPickedUp = false;

        // Re-enable physics and gravity
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;

        // Set Rigidbody properties for realistic weight and motion
        rb.mass = 2f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.1f;

        // Calculate throw direction
        Vector3 throwDirection =
            Camera.main.transform.forward * forwardThrowForce +
            Camera.main.transform.up * 0.05f +
            Camera.main.transform.right * Random.Range(-0.2f, 0.2f) +
            Vector3.down * 0.35f; ; 

        rb.AddForce(throwDirection, ForceMode.Impulse);

       // Add a small randomized spin for realistic drop feel
       Vector3 spin = new Vector3(
           Random.Range(-0.25f, 0.25f),
           Random.Range(-0.35f, 0.35f),
           Random.Range(-0.25f, 0.25f)
       );

        rb.AddTorque(spin, ForceMode.Impulse);

        // Extra downward impulse to ensure immediate plop and faster fall
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        // Clear the hold reference
        holdPoint = null;
    }

    /// <summary>
    /// Updates the object's position and rotation after the normal Update loop.
    /// Ensures smooth movement and applies a rotation offset while help.
    /// </summary>
    void LateUpdate()
    {
        if (isPickedUp && holdPoint != null)
        {
            // Match the hold point position
            transform.position = holdPoint.position;

            // Apply rotation offset so the object aligns naturally in hand
            transform.rotation = holdPoint.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
