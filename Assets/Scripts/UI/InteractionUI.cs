using TMPro;
using UnityEngine;

// Controls the UI prompt shown when the player can interact with something
public class InteractionUI : MonoBehaviour
{
    [Tooltip("UI text element used to display interaction prompts.")]
    public TextMeshProUGUI interactionText; 

    // Show an interaction prompt to the player
    public void Show(string message)
    {
        interactionText.text = message; 
        interactionText.enabled = true; 
    }

    // Hide the interaction prompt
    public void Hide()
    {
        interactionText.enabled = false; 
    }
}
