using UnityEngine;

public class VerticalDoor : MonoBehaviour, IInteractable
{
    public float openHeight = 5f;         
    public float openSpeed = 2f;          

    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
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
