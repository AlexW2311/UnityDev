using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputManager : MonoBehaviour
{
    //Reference to Input Actions
    //public InputActionReference mouseMovementAction;
    //public InputActionReference mouseClickAction;
    private PlayerControls playerControls;
    private Transform cameraTransform; //camera Transform value
    public float sensitivity = 0.2f; //mouse sensitivy
    private Vector2 currentRotation; //Tracks rotation
    private  Vector2 lookDelta = Vector2.zero;   

    void Awake()
    {
        playerControls = new PlayerControls();
        cameraTransform = transform.Find("fpv_camera");
        systemsCheck();
    }

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void OnEnable()
    {
        //enable the actions
        playerControls.Gameplay.MouseMovement.Enable();
        playerControls.Gameplay.MouseActions.Enable();

        //sub to the action events
        playerControls.Gameplay.MouseMovement.performed += OnMouseMove;
        playerControls.Gameplay.MouseActions.performed += OnMouseClick;


    }

    private void OnDisable()
    {
        //disable actions
        playerControls.Gameplay.MouseMovement.Disable();
        playerControls.Gameplay.MouseActions.Disable();

        //unsub to action events
        playerControls.Gameplay.MouseMovement.performed -= OnMouseMove;
        playerControls.Gameplay.MouseActions.performed -= OnMouseClick;

    }

    void Update()
    {
        Vector2 delta = lookDelta;
        lookDelta = Vector2.zero;
        //Debug.Log($"Delta Magnitude (Before): {delta.magnitude}");
        // Apply a deadzone to filter out small input values
        if (delta.magnitude < 0.01f){delta = Vector2.zero;}

        //Debug.Log($"Current Rotation (Before): Pitch: {currentRotation.x}, Yaw: {currentRotation.y}");
        currentRotation.x -= delta.y * sensitivity;
        currentRotation.y += delta.x * sensitivity;

        // clamp pitch (x-axis)
        currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
        //Debug.Log($"Current Rotation (After): Pitch: {currentRotation.x}, Yaw: {currentRotation.y}");
        //apply rotation
        if (cameraTransform != null)
        {
            //apply pitch to camera
            cameraTransform.localRotation = Quaternion.Euler(currentRotation.x, 0.0f, 0.0f);
            //apply yaw to player object
            transform.localRotation = Quaternion.Euler(0.0f, currentRotation.y, 0.0f);
        }
        //Debug.Log($"moved by:  {delta}");
    }


    private void OnMouseClick(InputAction.CallbackContext context)
    {
        //detect click
        Debug.Log("Mouse clicked");
    }
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        lookDelta = context.ReadValue<Vector2>();
    }
    private void systemsCheck()
    {
        foreach (Transform child in transform)
        {
            Debug.Log($"Child found: {child.name}"); //check on the kids
        }

        if (cameraTransform == null)  // report that mofo missing
        {
            Debug.Log("FPV Camera not found");
        }
        else
        {
            Debug.Log("FPV Camera initialized");
        }

        if (playerControls == null)  // report that mofo missing
        {
            Debug.Log("Mouse Controls not found");
        }
        else
        {
            Debug.Log("Mouse Controls initialized");
        }

        Debug.Log("Systems Check complete");
    }

}
