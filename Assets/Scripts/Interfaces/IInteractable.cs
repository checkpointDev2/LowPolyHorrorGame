/// <summary>
/// Represents an object that can be interacted with by the player.
/// Any class that implements this interface must define what happens when the object is interacted with.
/// </summary>
public interface IInteractable
{
    void Interact();
}