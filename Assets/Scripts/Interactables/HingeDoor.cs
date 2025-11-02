using UnityEngine;

public class HingeDoor : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    public Transform hinge; 

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (hinge == null) hinge = transform; 

        closedRotation = hinge.rotation;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, hinge.up); 
    }

    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen));
    }

    private System.Collections.IEnumerator RotateDoor(bool open)
    {
        Quaternion targetRotation = open ? openRotation : closedRotation;

        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        hinge.rotation = targetRotation;
    }
}
