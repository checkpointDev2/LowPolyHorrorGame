using TMPro;
using UnityEngine;
using UnityEngine.UI;

// On-screen UI for interaction prompts
public class InteractionUI : MonoBehaviour
{
    // Reference the component that displays the message
    public TextMeshProUGUI interactionText;

    // Show the message on-screen
    public void Show(string message)
    {
        interactionText.text = message;
        interactionText.enabled = true;
    }

    // Hide the message
    public void Hide()
    {
        interactionText.enabled = false;
    }
}
