// 8/1/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.InputSystem;

public class SuspendInputOnFocus : MonoBehaviour
{
    // Reference to the InputActionMap
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputActionMap playerActionMap;

    private void Awake()
    {
        // Get the action map for player controls (replace "Player" with your action map name)
        playerActionMap = inputActionAsset.FindActionMap("Gameplay");

        if (playerActionMap == null)
        {
            Debug.LogError("Player action map not found! Ensure the name matches your Input Action Map.");
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (playerActionMap == null) return;

        if (hasFocus)
        {
            // Enable the action map when the window is focused
            playerActionMap.Enable();
            Debug.Log("Game window focused: Input enabled.");
        }
        else
        {
            // Disable the action map when the window is unfocused
            playerActionMap.Disable();
            Debug.Log("Game window unfocused: Input disabled.");
        }
    }
}
