using UnityEngine;

public class SlidingDoor : MonoBehaviour, IInteractable
{
    public float slideDistance = 3f;    
    public float openSpeed = 2f;       
    public bool slideAlongX = true;    

    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.position;

        if (slideAlongX)
            openPosition = closedPosition + transform.right * slideDistance;
        else
            openPosition = closedPosition + transform.forward * slideDistance;
    }

    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(MoveDoor(isOpen));
    }

    private System.Collections.IEnumerator MoveDoor(bool open)
    {
        Vector3 target = open ? openPosition : closedPosition;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.position = target;
    }
}
