using UnityEngine;

/// <summary>
/// Represents an object that can be picked up and carried by the player.
/// Implementers define how the object behaves when picked up or dropped.
/// </summary>
public interface IPickupable
{
    /// <summary>
    /// Called when the player picks up the object.
    /// The object should attach to the provided parent trasform (e.g. player's hold point).
    /// </summary>
    void Pickup(Transform parent); 

    /// <summary>
    /// Called when the object is dropped and should return to the world.
    /// </summary>
    void Drop();                   
}
