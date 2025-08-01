using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerControls input;
    private bool isPaused = false;

    void Awake()
    {
        input = new PlayerControls();
    }

    void OnEnable()
    {
        input.Enable();
        input.Gameplay.Pause.performed += OnPause;
    }

    void OnDisable()
    {
        input.Gameplay.Pause.performed -= OnPause;
        input.Disable();
        Time.timeScale = 1f;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            input.Gameplay.Disable();         // mute ALL gameplay actions
            input.Gameplay.Pause.Enable();

        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
            input.Gameplay.Enable();
        }
        // Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = isPaused;
    }
}
