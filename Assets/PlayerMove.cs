using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float moveSpeed = 5f; // Speed of player movement
    public float mouseSensitivity = 2f; // Sensitivity for mouse movement
    public Transform cameraTransform; // Reference to the camera transform

    void Start() {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        HandleMovement();
        //HandleCameraRotation();
    }

    void HandleMovement() {
        // Get input from WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Calculate movement direction relative to the player
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    

}
